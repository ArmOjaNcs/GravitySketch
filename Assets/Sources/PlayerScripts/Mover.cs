using Assets.Sources.Pause;
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
        private Transform _transform;
        private Vector3 _moveDirection;
        private Vector3 _currentVelocity;
        private float _rotateAxis;

        public event Action<Vector3> PositionChanged;

        public float MoveSpeed => _moveSpeed;

        private void OnEnable()
        {
            _booster.BoostApplied += OnBoostApplied;
            _playerInput.DirectionChanged += OnDirectionChanged;
            _playerInput.Rotated += OnRotated;
        }

        private void OnDisable()
        {
            _booster.BoostApplied -= OnBoostApplied;
            _playerInput.DirectionChanged -= OnDirectionChanged;
            _playerInput.Rotated -= OnRotated;
        }

        private void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            PositionChanged?.Invoke(_transform.position);
        }

        private void FixedUpdate()
        {
            if (IsPaused || IsInitialized == false)
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
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
            _currentSpeed = _moveSpeed;
            _transform = transform;
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();

            if(_rigidbody != null)
            {
                _currentVelocity = _rigidbody.velocity;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }
        }

        public override void Resume()
        {
            base.Resume();

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _currentVelocity;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UpgradeMoveSpeed()
        {
            _moveSpeed += _moveSpeedOnUpgrade;
        }

        private void OnBoostApplied(float boostSpeed)
        {
            if (boostSpeed < 0)
            {
                Debug.LogError("Boost speed can not be less than 0");
                return;
            }

            _currentSpeed = Mathf.Approximately(boostSpeed, 0) ? _moveSpeed : boostSpeed;
        }

        private void OnDirectionChanged(Vector2 moveDirection) => _moveDirection = moveDirection;

        private void OnRotated(float rotateAxis) => _rotateAxis = Mathf.Clamp(rotateAxis, -1, 1);

        private void Move()
        {
            if (_moveDirection.sqrMagnitude < 0.001f)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            Vector3 localMovement = new Vector3(_moveDirection.x, 0, _moveDirection.y).normalized;
            Vector3 worldDirection = _transform.TransformDirection(localMovement);

            _rigidbody.velocity = worldDirection * _currentSpeed;
        }

        private void Rotate()
        {
            if (Mathf.Abs(_rotateAxis) < 0.001f)
                return;

            float rotationAmount = _rotateAxis * _rotationSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
    }
}