using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class CameraFollower : PauseableRoutine
    {
        [SerializeField] private Player _player;
        [SerializeField] private Booster _booster;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _followSmoothness = 5f;
        [SerializeField] private float _rotateSmoothness = 5f;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _lookAheadSmoothness = 5f;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private float _offsetByGrow;
        [SerializeField] private float _shakeIntensity = 1f;
        [SerializeField] private float _shakeTime = 0.2f;

        private Transform _transform;
        private Camera _camera;
        private Vector3 _currentLookAhead;

        private float _targetYOffset;
        private float _startYOffset;
        private float _currentYOffset;

        private float _shakeTimer;
        private float _shakeTimerTotal;
        private float _currentShakeIntensity;
        private float _rotateAxis;
        private float _aheadDelta = 1;

        public Transform Transform => _transform;
        private float _lookAheadDistance => _player.Radius + _aheadDelta;

        private void Awake()
        {
            _transform = transform;
            _camera = Camera.main;

            _targetYOffset = _transform.position.y;
            _currentYOffset = _targetYOffset;
            _transform.SetParent(_player.transform);
        }

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
            _player.Damaged += ShakeCamera;
            _playerInput.Rotated += OnRotated;
            _booster.Applied += OnBoosterApplied;
            _booster.Discarded += OnBoosterDiscarded;
        }

        private protected override void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
            _player.Damaged -= ShakeCamera;
            _playerInput.Rotated -= OnRotated;
            _booster.Applied -= OnBoosterApplied;
            _booster.Discarded -= OnBoosterDiscarded;
            base.OnDisable();
        }

        private void Start()
        {
            _transform.SetParent(null);
        }

        private void LateUpdate()
        {
            FollowPlayer();
            Rotate();
            UpdateShake();
        }

        private void OnBoosterApplied()
        {
            _aheadDelta += _player.Radius;
        }

        private void OnBoosterDiscarded()
        {
            _aheadDelta = UserUtils.One;
        }

        private void OnRotated(float rotateAxis) => _rotateAxis = Mathf.Clamp(rotateAxis, -1, 1);

        private void FollowPlayer()
        {
            Vector3 playerPos = _player.Position;
            Vector3 velocity = _player.Velocity;

            Vector3 lookDir = new Vector3(velocity.x, 0f, velocity.z);

            Vector3 targetLookAhead = Vector3.zero;

            if (lookDir.sqrMagnitude > 0.01f)
                targetLookAhead = lookDir.normalized * _lookAheadDistance;

            _currentLookAhead = Vector3.Lerp(
                _currentLookAhead,
                targetLookAhead,
                _lookAheadSmoothness * Time.deltaTime
            );

            Vector3 target = playerPos + _currentLookAhead;
            target.y = _currentYOffset;

            _transform.position = Vector3.Lerp(
                _transform.position,
                target,
                _followSmoothness * Time.deltaTime
            );
        }

        private void Rotate()
        {
            if (Mathf.Abs(_rotateAxis) < 0.001f || IsPaused || _player.Dead)
                return;
      
            float deltaAngle = _rotateAxis * _rotationSpeed;
            Quaternion deltaRotation = Quaternion.Euler(0f, deltaAngle, 0f);
            Quaternion rot = _transform.rotation;
            Quaternion targetRot = rot * deltaRotation;
            _transform.rotation = Quaternion.Slerp(rot, targetRot, _rotateSmoothness * Time.deltaTime);
        }

        private void ShakeCamera()
        {
            _currentShakeIntensity = _shakeIntensity;
            _shakeTimer = _shakeTime;
            _shakeTimerTotal = _shakeTime;
        }

        private void UpdateShake()
        {
            if (_shakeTimer <= 0)
                return;

            _shakeTimer -= Time.deltaTime;

            float progress = 1 - (_shakeTimer / _shakeTimerTotal);
            float strength = Mathf.Lerp(_currentShakeIntensity, 0f, progress);

            Vector3 shakeOffset = Random.insideUnitSphere * strength;
            shakeOffset.z = 0; 

            _camera.transform.localPosition = shakeOffset;
        }

        public override void Pause()
        {
            base.Pause();
            _camera.transform.localPosition = Vector3.zero;
        }

        private void OnGrowing()
        {
            _startYOffset = _currentYOffset;
            _targetYOffset = _currentYOffset + _offsetByGrow;
            OnUpdate();
        }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;

            _currentYOffset = Mathf.Lerp(
                _startYOffset,
                _targetYOffset,
                progress
            );
        }

        private protected override void OnRoutineEnd()
        {
            _currentYOffset = _targetYOffset;
            base.OnRoutineEnd();
        }
    }
}