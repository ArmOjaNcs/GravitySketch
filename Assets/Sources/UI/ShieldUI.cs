using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class ShieldUI : CircleBar
    {
        [SerializeField] private Shield _shield;
        [SerializeField] private SmoothedFade _backgroundFade;

        private protected override void OnEnable()
        {
            _shield.DefendApplied += OnDefendStart;
            _shield.Reloading += OnReloadingStart;
            base.OnEnable();
        }

        private protected override void OnDisable()
        {
            _shield.DefendApplied -= OnDefendStart;
            _shield.Reloading -= OnReloadingStart;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _backgroundFade.Init(pauseHandler);
            IsInitialized = true;
        }

        private void OnDefendStart()
        {
            ActiveTimeImage.gameObject.SetActive(true);
            ActiveTimeImage.SetValue(UserUtils.HalfUnit);
            ActiveTimeImage.UpdateView(_shield.DefendTime, 0);
            _backgroundFade.ShowElements();
        }

        private void OnReloadingStart()
        {
            ReloadTimeImage.gameObject.SetActive(true);
            ReloadTimeImage.SetValue(UserUtils.HalfUnit);
            float reloadDuration = _shield.CycleTime - _shield.DefendTime;
            ReloadTimeImage.UpdateView(reloadDuration, 0);
        }

        private protected override void OnActiveImageUpdated()
        {
            ActiveTimeImage.gameObject.SetActive(false);
            _backgroundFade.FadeOut();
        }

        private protected override void OnReloadImageUpdated()
        {
            _emblem.color = ReloadTimeImage.Color;
            Animation.OnComplete(() => _emblem.color = Color.white);
            Animation.Restart();
            ReloadTimeImage.gameObject.SetActive(false);
        }
    }
}