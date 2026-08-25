using System;
using CameraScripts;
using Pause;
using PlayerScripts.Ability;
using Utils;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : PauseableObject
    {
        [SerializeField]
        [Min(1)] private float _moveSpeed;
        [SerializeField]
        [Min(50)] private float _rotationSpeed;
        [SerializeField]
        [Min(0)] private float _moveSpeedOnUpgrade;
        [SerializeField] private Booster _booster;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CameraFollower _cameraFollower;

        private Rigidbody _rigidbody;
        private float _currentSpeed;
        private float _accelerationSpeed;
        private float _accelerationTime = 1;
        private Transform _transform;
        private Vector3 _moveDirection;
        private Vector3 _currentVelocity;
        private float _decelerationTime = 0.5f;
        private bool _isBoosted;
        private bool _pauseRequested;
        private bool _pauseApplied;

        public event Action<Vector3> PositionChanged;

        public float MoveSpeed => _moveSpeed;

        private void OnEnable()
        {
            _booster.Applied += OnBoostApplied;
            _booster.Discarded += OnBoostDiscarded;
            _playerInput.DirectionChanged += OnDirectionChanged;
        }

        private void OnDisable()
        {
            _booster.Applied -= OnBoostApplied;
            _booster.Discarded -= OnBoostDiscarded;
            _playerInput.DirectionChanged -= OnDirectionChanged;
        }

        private void Update()
        {
            if (IsInitialized == false)
                return;

            PositionChanged?.Invoke(_transform.position);
        }

        private void FixedUpdate()
        {
            if (IsInitialized == false)
                return;

            if (_pauseRequested && _pauseApplied == false)
            {
                _currentVelocity = _rigidbody.velocity;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;

                _pauseApplied = true;
                return;
            }

            if (IsPaused)
                return;

            Move();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            _currentSpeed = _moveSpeed;
            _transform = transform;
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();
            _pauseRequested = true;
            _pauseApplied = false;
        }

        public override void Resume()
        {
            base.Resume();
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = _currentVelocity;

            _pauseRequested = false;
            _pauseApplied = false;
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }

        public void UpgradeMoveSpeed(bool isGrowing)
        {
            if (isGrowing)
                _moveSpeed += _moveSpeedOnUpgrade;
            else
                _moveSpeed += _moveSpeedOnUpgrade * UserUtils.MoverUpgradeReducedCoefficient;
        }

        private void OnBoostApplied()
        {
            _isBoosted = true;
            _currentSpeed = _moveSpeed * UserUtils.BoostMultiplier;
        }

        private void OnBoostDiscarded() => _isBoosted = false;

        private void OnDirectionChanged(Vector2 moveDirection) => _moveDirection = moveDirection;

        private void Move()
        {
            if (_moveDirection.sqrMagnitude < 0.001f)
            {
                float speed = _accelerationSpeed;

                speed = Mathf.MoveTowards(speed, 0f, Time.fixedDeltaTime * (_moveSpeed / _decelerationTime));

                _accelerationSpeed = speed;

                if (speed < 0.01f)
                    speed = 0f;

                Vector3 currentDir = _rigidbody.velocity.normalized;
                _rigidbody.velocity = currentDir * speed;

                return;
            }

            Vector3 localMovement = new Vector3(_moveDirection.x, 0, _moveDirection.y).normalized;
            Vector3 worldDirection = _cameraFollower.Transform.TransformDirection(localMovement);

            if (_accelerationSpeed < _moveSpeed)
            {
                float newSpeed = _accelerationSpeed;
                _accelerationSpeed = Mathf.MoveTowards(
                    newSpeed, _moveSpeed, Time.fixedDeltaTime * (_moveSpeed / _accelerationTime));
            }

            if (_isBoosted)
                _accelerationSpeed = _moveSpeed;
            else
                _currentSpeed = _accelerationSpeed;

            _rigidbody.velocity = worldDirection * _currentSpeed;
        }
    }
}