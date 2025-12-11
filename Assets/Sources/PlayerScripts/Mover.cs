using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : PauseableObject
    {
        [SerializeField, Min(1)] private float _moveSpeed;
        [SerializeField, Min(50)] private float _rotationSpeed;
        [SerializeField, Min(0)] private float _moveSpeedOnUpgrade;
        [SerializeField] private Booster _booster;
        [SerializeField] private PlayerInput _playerInput;

        private Rigidbody _rigidbody;
        private float _currentSpeed;
        private float _accelerationSpeed;
        private float _accelerationTime = 1;
        private Transform _transform;
        private Vector3 _moveDirection;
        private Vector3 _currentVelocity;
        private float _rotateAxis;
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
            _playerInput.Rotated += OnRotated;
        }

        private void OnDisable()
        {
            _booster.Applied -= OnBoostApplied;
            _booster.Discarded -= OnBoostDiscarded;
            _playerInput.DirectionChanged -= OnDirectionChanged;
            _playerInput.Rotated -= OnRotated;
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
                _rigidbody.isKinematic = true;
                _rigidbody.velocity = Vector3.zero;

                _pauseApplied = true;
                return;
            }

            if (IsPaused)
                return;

            Move();
            Rotate();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
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

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UpgradeMoveSpeed(bool isGrowing)
        {
            if (isGrowing)
                _moveSpeed += _moveSpeedOnUpgrade;
            else
                _moveSpeed += (_moveSpeedOnUpgrade / UserUtils.Three);
        }

        private void OnBoostApplied()
        {
            _isBoosted = true;
            _currentSpeed = _moveSpeed * 1.5f;
        }

        private void OnBoostDiscarded() => _isBoosted = false;
        private void OnDirectionChanged(Vector2 moveDirection) => _moveDirection = moveDirection;
        private void OnRotated(float rotateAxis) => _rotateAxis = Mathf.Clamp(rotateAxis, -1, 1);

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
            Vector3 worldDirection = _transform.TransformDirection(localMovement);

            if (_accelerationSpeed < _moveSpeed)
            {
                float newSpeed = _accelerationSpeed;
                _accelerationSpeed = Mathf.MoveTowards(newSpeed, _moveSpeed, 
                    Time.fixedDeltaTime * (_moveSpeed / _accelerationTime));
            }

            if (_isBoosted)
                _accelerationSpeed = _moveSpeed;
            else
                _currentSpeed = _accelerationSpeed;

            _rigidbody.velocity = worldDirection * _currentSpeed;
        }

        private void Rotate()
        {
            if (Mathf.Abs(_rotateAxis) < 0.001f)
            {
                Vector3 currentAngularVelocity = _rigidbody.angularVelocity;
                _rigidbody.angularVelocity = Vector3.MoveTowards(currentAngularVelocity, Vector3.zero,
                    5 * Time.fixedDeltaTime);
                return;
            }

            float radPerSec = _rotateAxis * _rotationSpeed * Mathf.Deg2Rad;
            _rigidbody.angularVelocity = Vector3.up * radPerSec;
        }
    }
}