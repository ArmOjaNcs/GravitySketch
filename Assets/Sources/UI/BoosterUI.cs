using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class BoosterUI : CircleBar
    {
        [SerializeField] private Booster _booster;

        private protected override void OnEnable()
        {
            _booster.Applied += OnBoostApplied;
            _booster.Reloading += OnReloading;
            _booster.Reloaded += OnReloaded;
            base.OnEnable();
        }

        private protected override void OnDisable()
        {
            _booster.Applied -= OnBoostApplied;
            _booster.Reloading -= OnReloading;
            _booster.Reloaded -= OnReloaded;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            IsInitialized = true;
        }

        private protected override void OnReloadImageUpdated()
        {
            if (_booster.CurrentBoostCount == _booster.BoostCount)
                ReloadTimeImage.gameObject.SetActive(false);
        }

        private protected override void OnActiveImageUpdated()
        {
            ActiveTimeImage.gameObject.SetActive(false);
        }

        private void OnBoostApplied()
        {
            ActiveTimeImage.gameObject.SetActive(true);
            ActiveTimeImage.SetValue(UserUtils.HalfUnit);
            ActiveTimeImage.UpdateView(_booster.BoostTime, 0);
        }

        private void OnReloading()
        {
            ReloadTimeImage.gameObject.SetActive(true);
            ReloadTimeImage.SetValue(UserUtils.HalfUnit);
            ReloadTimeImage.UpdateView(_booster.BoostReloadTime, 0);
        }

        private void OnReloaded()
        {
            _emblem.color = ReloadTimeImage.Color;
            Animation.OnComplete(() => _emblem.color = Color.white);
            Animation.Restart();
        }
    }
}