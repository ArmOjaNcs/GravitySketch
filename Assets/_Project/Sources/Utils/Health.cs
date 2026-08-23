using System;
using UnityEngine;

namespace Utils
{
    public class Health : MonoBehaviour
    {
        private const float MinValue = 10;

        private float _maxValue;

        public event Action Updated;

        public float MaxValue => _maxValue;
        public float CurrentValue { get; private set; }

        public void Initialize(float maxValue)
        {
            if (maxValue <= 0)
                maxValue = MinValue;

            _maxValue = maxValue;
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

            Updated?.Invoke();
        }

        public void TakeHeal(float heal)
        {
            if (heal <= 0)
                return;

            CurrentValue += heal;

            if (CurrentValue > MaxValue)
                CurrentValue = MaxValue;

            Updated?.Invoke();
        }
    }
}