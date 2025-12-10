using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class CameraFollower : PauseableRoutine
    {
        [Header("Follow")]
        [SerializeField] private Player _player;
        [SerializeField] private float _followSmoothness = 5f;
        [SerializeField] private float _rotateSmoothness = 5f;

        [Header("Grow")]
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private float _offsetByGrow;

        [Header("Shake")]
        [SerializeField] private float _shakeIntensity = 1f;
        [SerializeField] private float _shakeTime = 0.2f;

        private Transform _transform;
        private Camera _camera;

        private float _targetYOffset;
        private float _startYOffset;
        private float _currentYOffset;

        private float _shakeTimer;
        private float _shakeTimerTotal;
        private float _currentShakeIntensity;

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
        }

        private protected override void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
            _player.Damaged -= ShakeCamera;
            base.OnDisable();
        }

        private void Start()
        {
            _transform.SetParent(null);
        }

        private void LateUpdate()
        {
            //if (IsPaused)
            //    return;

            FollowPlayer();
            RotateTowardsPlayer();
            UpdateShake();
        }

        private void FollowPlayer()
        {
            Vector3 pos = _transform.position;
            Vector3 target = _player.Position;
            target.y = _currentYOffset;

            _transform.position = Vector3.Lerp(
                pos,
                target,
                _followSmoothness * Time.deltaTime
            );
        }

        private void RotateTowardsPlayer()
        {
            Quaternion rot = _transform.rotation;

            _transform.rotation = Quaternion.Slerp(
                rot,
                _player.transform.rotation,
                _rotateSmoothness * Time.deltaTime
            );
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