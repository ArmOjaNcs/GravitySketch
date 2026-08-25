using Pause;
using PlayerScripts;
using PlayerScripts.Ability;
using Utils;
using UnityEngine;

namespace CameraScripts
{
    public class CameraFollower : PauseableObject
    {
        [SerializeField] private Player _player;
        [SerializeField] private Booster _booster;
        [SerializeField] private PlayerInput _playerInput;

        [Header("Follow")]
        [SerializeField] private float _minFollowSmoothness = 2.5f;
        [SerializeField] private float _maxFollowSmoothness = 7f;
        [SerializeField] private float _radiusForMaxSpeed = 4f;

        [SerializeField] private float _rotateSmoothness = 5f;
        [SerializeField] private float _rotationSpeed;

        [Header("Look Ahead")]
        [SerializeField] private float _lookAheadSmoothness = 5f;
        [SerializeField] private float _aheadFactor = 0.18f;
        [SerializeField] private float _minAhead = 1f;
        [SerializeField] private float _speedLookAheadMultiplier = 1.2f;
        [SerializeField] private float _maxSpeedForLookAhead = 10f;

        [Header("Shake")]
        [SerializeField] private float _shakeIntensity = 1f;
        [SerializeField] private float _shakeTime = 0.2f;

        private Transform _transform;
        private Camera _camera;

        private Vector3 _currentLookAhead;

        private float _shakeTimer;
        private float _shakeTimerTotal;
        private float _currentShakeIntensity;
        private float _rotateAxis;
        private float _aheadDelta = 1.2f;
        private float _defaultAheadDelta = 1.2f;
        private float _defaultYOffset = 5f;

        public Transform Transform => _transform;

        private float YDistance => (_player.Radius * _defaultYOffset) + _defaultYOffset;

        private void Awake()
        {
            _transform = transform;
            _camera = Camera.main;

            _transform.SetParent(_player.transform);
        }

        private void OnEnable()
        {
            _player.Damaged += ShakeCamera;
            _playerInput.Rotated += OnRotated;
            _booster.Applied += OnBoosterApplied;
            _booster.Discarded += OnBoosterDiscarded;
        }

        private void OnDisable()
        {
            _player.Damaged -= ShakeCamera;
            _playerInput.Rotated -= OnRotated;
            _booster.Applied -= OnBoosterApplied;
            _booster.Discarded -= OnBoosterDiscarded;
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

        public override void Pause()
        {
            base.Pause();
            _camera.transform.localPosition = Vector3.zero;
        }

        private void OnBoosterApplied()
        {
            _aheadDelta += UserUtils.BoostAheadDistance;
        }

        private void OnBoosterDiscarded()
        {
            _aheadDelta = _defaultAheadDelta;
        }

        private void OnRotated(float rotateAxis)
        {
            _rotateAxis = Mathf.Clamp(rotateAxis, -1, 1);
        }

        private void FollowPlayer()
        {
            Vector3 velocity = new Vector3(
                _player.Velocity.x,
                0f,
                _player.Velocity.z);

            Vector3 lookDir = velocity;

            if (velocity.sqrMagnitude < 0.02f)
                lookDir = Vector3.zero;

            float speed = velocity.sqrMagnitude;

            if (speed < 0.0225f)
                _currentLookAhead = Vector3.zero;

            Vector3 targetLookAhead = Vector3.zero;

            if (lookDir.sqrMagnitude > 0.0001f)
                targetLookAhead = lookDir.normalized * GetAheadDistance();

            float lookSmooth = Damp(_lookAheadSmoothness);

            _currentLookAhead = Vector3.Lerp(
                _currentLookAhead,
                targetLookAhead,
                lookSmooth);

            Vector3 target = _player.Position + _currentLookAhead;
            target.y = YDistance;

            float radiusT = Mathf.Clamp01(_player.Radius / _radiusForMaxSpeed);

            float followSmooth = Mathf.Lerp(
                _minFollowSmoothness,
                _maxFollowSmoothness,
                radiusT);

            _transform.position = Vector3.Lerp(
                    _transform.position,
                    target,
                    Damp(followSmooth));
        }

        private void Rotate()
        {
            if (Mathf.Abs(_rotateAxis) < 0.001f || IsPaused || _player.Dead)
                return;

            float deltaAngle = _rotateAxis * _rotationSpeed;

            Quaternion deltaRotation = Quaternion.Euler(0f, deltaAngle, 0f);

            Quaternion rot = _transform.rotation;
            Quaternion targetRot = rot * deltaRotation;

            _transform.rotation = Quaternion.Slerp(
                rot,
                targetRot,
                _rotateSmoothness * Time.deltaTime);
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

            float strength = Mathf.Lerp(
                _currentShakeIntensity,
                0f,
                progress);

            Vector3 shakeOffset = Random.insideUnitSphere * strength;

            shakeOffset.z = 0;

            _camera.transform.localPosition = shakeOffset;
        }

        private float GetAheadDistance()
        {
            float radius = Mathf.Max(_player.Radius, UserUtils.MinRadius);
            float height = (radius * _defaultYOffset) + _defaultYOffset;
            float baseAhead = height * _aheadFactor * _aheadDelta;
            float speed = _player.Velocity.sqrMagnitude;
            float speedT = Mathf.Clamp01(speed / (_maxSpeedForLookAhead * _maxSpeedForLookAhead));
            float speedMultiplier = Mathf.Lerp(1f, _speedLookAheadMultiplier, speedT);
            float ahead = baseAhead * speedMultiplier;

            return Mathf.Max(ahead, _minAhead);
        }

        private float Damp(float smooth)
        {
            return 1f - Mathf.Exp(-smooth * Time.deltaTime);
        }
    }
}