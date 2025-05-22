using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxExternalForce;
    [SerializeField] private float _pushDecaySpeed;
    [SerializeField] private Booster _boster;
    [SerializeField] private PlayerInput _playerInput;
    
    private CharacterController _characterController;
    private float _currentSpeed;
    private Transform _transform;
    private Vector3 _externalForce;
    private Vector3 _moveDirection;
    private float _yPosition;

    public event Action<Vector3> PositionChanged; 

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _currentSpeed = _moveSpeed;
        _transform = transform;
        _yPosition = _transform.position.y;
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
        Move();
        SaveYPosition();
        DecayExternalForce();
    }

    public void ApplyExternalForce(Vector3 newForce)
    {
        _externalForce = new Vector3(
            (Mathf.Abs(newForce.x) > Mathf.Abs(_externalForce.x)) ? newForce.x : _externalForce.x,
            (Mathf.Abs(newForce.y) > Mathf.Abs(_externalForce.y)) ? newForce.y : _externalForce.y,
            (Mathf.Abs(newForce.z) > Mathf.Abs(_externalForce.z)) ? newForce.z : _externalForce.z
        );

        _externalForce = Vector3.ClampMagnitude(_externalForce, _maxExternalForce);
    }

    private void OnBoostApplied(float boostSpeed)
    {
        if (boostSpeed < 0)
        {
            Debug.LogError("Boost speed can not be less 0");
            return;
        }

        if (Mathf.Approximately(boostSpeed, 0))
        {
            _currentSpeed = _moveSpeed;
            return;
        }

        _currentSpeed = boostSpeed;
    }

    private void OnDirectionChanged(Vector2 moveDirection) => _moveDirection = moveDirection;

    private void DecayExternalForce()
    {
        _externalForce = Vector3.MoveTowards(
           _externalForce,
           Vector3.zero,
           _pushDecaySpeed * Time.deltaTime
       );
    }

    private void SaveYPosition()
    {
        if (_transform.position.y > _yPosition || _transform.position.y < _yPosition)
        {
            Vector3 currentPosition = _transform.position;
            currentPosition.y = _yPosition;
            _transform.position = currentPosition;
        }
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(_moveDirection.x, 0, _moveDirection.y).normalized;
        Vector3 newPosition = moveDirection * _currentSpeed * Time.deltaTime;
        _characterController.Move(newPosition + _externalForce);
        PositionChanged?.Invoke(_transform.position);
    }
}