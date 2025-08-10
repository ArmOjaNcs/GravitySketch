using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class BoosterUI : CircleBar
    {
        [SerializeField] private Booster _booster;

        private protected override void OnEnable()
        {
            _booster.BoostApplied += OnBoostApplied;
            _booster.Reloading += OnReloading;
            _booster.Reloaded += OnReloaded;
            base.OnEnable();
        }

        private protected override void OnDisable()
        {
            _booster.BoostApplied -= OnBoostApplied;
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

        private void OnBoostApplied(float speed)
        {
            if (speed <= 0)
                return;

            ActiveTimeImage.gameObject.SetActive(true);
            ActiveTimeImage.SetValue(1);
            ActiveTimeImage.UpdateView(_booster.BoostTime, 0);
        }

        private void OnReloading()
        {
            ReloadTimeImage.gameObject.SetActive(true);
            ReloadTimeImage.SetValue(1);
            ReloadTimeImage.UpdateView(_booster.BoostReloadTime, 0);
        }

        private void OnReloaded()
        {
            Text.color = ReloadTimeImage.Color;
            Animation.OnComplete(() => Text.color = Color.black);
            Animation.Restart();
        }
    }
}