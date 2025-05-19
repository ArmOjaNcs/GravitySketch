using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _boostSpeed;
    [SerializeField] private int _boostCount;
    [SerializeField] private float _boostTime = 0.5f;
    [SerializeField] private int _boostReloadTime;

    private CharacterController _characterController;
    private bool _isBoosted;
    private float _currentBoostTime;
    private float _currentBoostReloadTime;
    private float _currentSpeed;
    private int _currentBoostCount;

    private float HorizontalInput => Input.GetAxis(UserUtils.Horizontal);
    private float VerticalInput => Input.GetAxis(UserUtils.Vertical);
    private bool IsBoosted => Input.GetKeyDown(KeyCode.Mouse1);

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _currentBoostCount = _boostCount;
        _currentSpeed = _moveSpeed;
    }

    private void Update()
    {
        Move();
        Boost();
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(HorizontalInput, 0, VerticalInput).normalized;
        Vector3 newPosition = moveDirection * _currentSpeed * Time.deltaTime;
        _characterController.Move(newPosition);
    }

    private void Boost()
    {
        if (_currentBoostCount < _boostCount)
        {
            _currentBoostReloadTime += Time.deltaTime;

            if (_currentBoostReloadTime > _boostReloadTime)
                ReloadBoost();
        }

        if (IsBoosted && _isBoosted == false && _currentBoostCount > 0)
            ApplyBoost();

        if (_isBoosted)
        {
            _currentBoostTime += Time.deltaTime;

            if (_currentBoostTime > _boostTime)
                StopBoost();
        }
    }

    private void StopBoost()
    {
        _isBoosted = false;
        _currentBoostTime = 0;
        _currentSpeed = _moveSpeed;
    }

    private void ReloadBoost()
    {
        _currentBoostCount++;
        _currentBoostReloadTime = 0;
    }

    private void ApplyBoost()
    {
        _currentBoostCount--;
        _isBoosted = true;
        _currentSpeed = _boostSpeed;
    }
}