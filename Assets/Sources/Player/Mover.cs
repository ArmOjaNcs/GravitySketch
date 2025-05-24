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

    public event Action<Vector3> PositionChanged; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        _currentSpeed = _moveSpeed;
        _transform = transform;
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
        Vector3 velocity = new Vector3(_moveDirection.x, 0, _moveDirection.y).normalized * _currentSpeed;
        _rigidbody.velocity = velocity;
    }
}