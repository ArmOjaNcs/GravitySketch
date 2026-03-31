using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class HealthSliderView : SmoothedSlider
    {
        [SerializeField] private protected Health Health;

        private protected virtual void OnEnable()
        {
            Health.Updated += OnUpdate;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();

            Health.Updated -= OnUpdate;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            Slider.value = Health.CurrentValue / Health.MaxValue;
            IsInitialized = true;
        }

        private protected override void OnUpdate()
        {
            TargetValue = Health.CurrentValue / Health.MaxValue;
            base.OnUpdate();
        }
    }
}