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

        private void OnDefendStart()
        {
            ActiveTimeImage.gameObject.SetActive(true);
            ActiveTimeImage.SetValue(1);
            ActiveTimeImage.UpdateView(_shield.DefendTime, 0);
            _backgroundFade.ShowElements();
        }

        private void OnReloadingStart()
        {
            ReloadTimeImage.gameObject.SetActive(true);
            ReloadTimeImage.SetValue(1);
            float reloadDuration = _shield.CycleTime - _shield.DefendTime;
            ReloadTimeImage.UpdateView(reloadDuration, 0);
        }

        private protected override Tween GetAnimation()
        {
            return AnimationSpawner.GetShakeAnimation(Text.rectTransform, 0.5f);
        }

        private protected override void OnActiveImageUpdated()
        {
            ActiveTimeImage.gameObject.SetActive(false);
            _backgroundFade.FadeOut();
        }

        private protected override void OnReloadImageUpdated()
        {
            Text.color = ReloadTimeImage.Color;
            Animation.OnComplete(() => Text.color = Color.black);
            Animation.Restart();
            ReloadTimeImage.gameObject.SetActive(false);
        }
    }
}