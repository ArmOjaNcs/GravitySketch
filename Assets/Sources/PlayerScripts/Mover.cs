using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
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
        private float _rotateAxis;
        private float _defaultY;

        public event Action<Vector3> PositionChanged;

        public float MoveSpeed => _moveSpeed;

        private void Awake()
        {
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
            _defaultY = _transform.position.y;
        }

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
            FixYPosition();
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }

        private void LateUpdate()
        {
            PositionChanged?.Invoke(_transform.position);
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
            Vector3 localMovement = new Vector3(_moveDirection.x, 0, _moveDirection.y).normalized;
            Vector3 forwardDirection = _transform.forward.normalized;
            Vector3 rightDirection = _transform.right.normalized;

            Vector3 worldDirection = (forwardDirection * localMovement.z + rightDirection * localMovement.x);
            worldDirection.y = _defaultY;

            _rigidbody.velocity = worldDirection * _currentSpeed;
        }

        private void Rotate()
        {
            float rotationAmount = _rotateAxis * _rotationSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }

        private void FixYPosition()
        {
            if (Mathf.Abs(_rigidbody.position.y - _defaultY) > 0.0001f)
            {
                Vector3 fixedYposition = _rigidbody.position;
                fixedYposition.y = _defaultY;
                _rigidbody.MovePosition(fixedYposition);
            }
        }
    }
}