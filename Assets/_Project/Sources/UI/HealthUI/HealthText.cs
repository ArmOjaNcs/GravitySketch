using UI.PauseableRoutineUI;
using Utils;
using UnityEngine;

namespace UI.HealthUI
{
    public class HealthText : SmoothedText
    {
        [SerializeField] private protected Health Health;

        private void OnEnable()
        {
            Health.Updated += OnUpdate;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            Health.Updated -= OnUpdate;
        }

        private protected override void OnUpdate()
        {
            TargetValue = Health.CurrentValue;
            base.OnUpdate();
        }
    }
}