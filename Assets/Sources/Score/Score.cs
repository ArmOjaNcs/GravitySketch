using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private Validator _validator;

    public event Action ValueChanged;

    public int Value { get; private set; }

    private void OnEnable()
    {
        _validator.ValidateConfirmed += OnValidateConfirmed;
    }

    private void OnDisable()
    {
        _validator.ValidateConfirmed -= OnValidateConfirmed;
    }

    private void OnValidateConfirmed()
    {
        Value++;
        ValueChanged?.Invoke();
    }
}