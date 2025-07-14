using Assets.Sources.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class HealthImageView : SmoothedImage
    {
        [SerializeField] private protected Health Health;
        [SerializeField] private SmoothedImage _duplicate;

        private protected override void Start()
        {
            base.Start();
            Image.fillAmount = Health.CurrentValue / Health.MaxValue;
        }

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

        private protected override void OnUpdate()
        {
            TargetValue = Health.CurrentValue / Health.MaxValue;
            
            base.OnUpdate();
        }

        private void OnSelfUpdated()
        {
            _duplicate.UpdateView(Duration, TargetValue);
        }
    }
}