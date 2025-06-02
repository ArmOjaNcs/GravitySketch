using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxValue;

    public event Action HealthUpdate;

    public float MaxValue => _maxValue;
    public float CurrentValue { get; private set; }

    private void Awake()
    {
        CurrentValue = MaxValue;
    }

    public void TakeDamage(float damage)
    {
        if (Mathf.Approximately(CurrentValue, 0))
            return;

        if (damage <= 0)
            return;

        CurrentValue -= damage;

        if (CurrentValue <= 0)
            CurrentValue = 0;

        HealthUpdate?.Invoke();
    }
}