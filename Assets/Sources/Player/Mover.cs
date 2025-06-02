using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Booster _boster;
    [SerializeField] private PlayerInput _playerInput;
    
    private Rigidbody _rigidbody;
    private float _currentSpeed;
    private Transform _transform;
    private Vector3 _moveDirection;
    private float _defaultY;

    public event Action<Vector3> PositionChanged; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
        _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;
        _currentSpeed = _moveSpeed;
        _transform = transform;
        _defaultY = _transform.position.y;
    }

    private void OnEnable()
    {
        _boster.BoostApplied += OnBoostApplied;
        _playerInput.DirectionChanged += OnDirectionChanged;
    }

    private void OnDisable()
    {
        _boster.BoostApplied -= OnBoostApplied;
        _playerInput.DirectionChanged -= OnDirectionChanged;
    }

    private void Update()
    {
        PositionChanged?.Invoke(_transform.position);
        FixYPosition();
    }

    private void FixedUpdate()
    {
        MoveR();
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

    private void MoveR()
    {
        Vector3 velocity = new Vector3(_moveDirection.x, _defaultY, _moveDirection.y).normalized * _currentSpeed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
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