using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class HealthSliderView : SmoothedSlider
    {
        [SerializeField] private protected Health Health;

        private protected override void Start()
        {
            Slider.value = Health.CurrentValue / Health.MaxValue;
        }

        private protected virtual void OnEnable()
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
            TargetValue = Health.CurrentValue / Health.MaxValue;
            base.OnUpdate();
        }
    }
}