using System;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private float _boostSpeed;
    [SerializeField] private int _boostCount;
    [SerializeField] private float _boostTime = 0.5f;
    [SerializeField] private int _boostReloadTime;
    [SerializeField] private PlayerInput _playerInput;

    private bool _isBoostApplied;
    private bool _isBoosted;
    private float _currentBoostTime;
    private float _currentBoostReloadTime;
    private int _currentBoostCount;

    public event Action<float> BoostApplied;

    private void Awake()
    {
        _currentBoostCount = _boostCount;
    }

    private void OnEnable()
    {
        _playerInput.Boosted += OnBoosted;
    }

    private void OnDisable()
    {
        _playerInput.Boosted -= OnBoosted;
    }

    private void OnBoosted(bool isBoosted) => _isBoosted = isBoosted;

    private void Update()
    {
        Boost();
    }

    private void Boost()
    {
        if (_currentBoostCount < _boostCount)
        {
            _currentBoostReloadTime += Time.deltaTime;

            if (_currentBoostReloadTime > _boostReloadTime)
                ReloadBoost();
        }

        if (_isBoosted && _isBoostApplied == false && _currentBoostCount > 0)
            ApplyBoost();

        if (_isBoostApplied)
        {
            _currentBoostTime += Time.deltaTime;

            if (_currentBoostTime > _boostTime)
                StopBoost();
        }
    }

    private void StopBoost()
    {
        _isBoostApplied = false;
        _currentBoostTime = 0;
        BoostApplied?.Invoke(0);
    }

    private void ReloadBoost()
    {
        _currentBoostCount++;
        _currentBoostReloadTime = 0;
    }

    private void ApplyBoost()
    {
        _currentBoostCount--;
        _isBoostApplied = true;
        BoostApplied?.Invoke(_boostSpeed);
    }
}