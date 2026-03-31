using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class HealthImageView : SmoothedImage
    {
        [SerializeField] private protected Health Health;
        [SerializeField] private SmoothedImage _duplicate;

        private protected virtual void OnEnable()
        {
            Health.Updated += OnUpdate;
            Updated += OnSelfUpdated;
        }

        private protected override void OnDisable()
        {
            Health.Updated -= OnUpdate;
            Updated -= OnSelfUpdated;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            Image.fillAmount = Health.CurrentValue / Health.MaxValue;
            _duplicate.Init(pauseHandler);
            IsInitialized = true;
        }

        private protected override void OnUpdate()
        {
            TargetValue = Health.CurrentValue / Health.MaxValue;
            
            base.OnUpdate();
        }

        private void OnSelfUpdated()
        {
            _duplicate.UpdateValue(Duration, TargetValue);
        }
    }
}