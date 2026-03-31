using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class BoosterUI : AbilityBar
    {
        [SerializeField] private Booster _booster;

        private void OnEnable()
        {
            _booster.Applied += OnBoostApplied;
            _booster.Reloading += OnReloading;
            _booster.Reloaded += OnReloaded;
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

        private void OnBoostApplied()
        {
            ReloadTimeImage.SetValue(0);
            ActiveTimeImage.SetValue(UserUtils.Unit);

            if(_booster.CurrentBoostCount == 0)
                ActiveTimeImage.UpdateValue(_booster.BoostTime, 0);
        }

        private void OnReloading()
        {
            ReloadTimeImage.SetValue(0);
            ReloadTimeImage.UpdateValue(_booster.BoostReloadTime, UserUtils.Unit);
        }

        private void OnReloaded()
        {
            ActiveTimeImage.SetValue(UserUtils.Unit);
            Animation.Restart();
        }
    }
}