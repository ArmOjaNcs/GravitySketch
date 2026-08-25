using DG.Tweening;
using PlayerScripts.Ability;
using UI.PauseableRoutineUI;
using Utils;
using UnityEngine;

namespace UI.Ability
{
    public class ShieldUI : AbilityBar
    {
        [SerializeField] private Shield _shield;
        [SerializeField] private SmoothedFade _backgroundFade;

        private void OnEnable()
        {
            _shield.DefendApplied += OnDefendStart;
            _shield.Reloading += OnReloadingStart;
            ActiveTimeImage.Updated += OnActiveImageUpdated;
            ReloadTimeImage.Updated += OnReloadImageUpdated;
        }

        private protected override void OnDisable()
        {
            _shield.DefendApplied -= OnDefendStart;
            _shield.Reloading -= OnReloadingStart;
            ActiveTimeImage.Updated -= OnActiveImageUpdated;
            ReloadTimeImage.Updated -= OnReloadImageUpdated;
            base.OnDisable();
        }

        private void OnDefendStart()
        {
            ActiveTimeImage.SetValue(UserUtils.MaxFilledImageValue);
            ActiveTimeImage.UpdateValue(_shield.DefendTime, 0);
            ReloadTimeImage.SetValue(0);
            _backgroundFade.FadeIn(0, UserUtils.MaxFilledImageValue);
        }

        private void OnReloadingStart()
        {
            ReloadTimeImage.SetValue(0);
            float reloadDuration = _shield.CycleTime - _shield.DefendTime;
            ReloadTimeImage.UpdateValue(reloadDuration, 1);
        }

        private void OnActiveImageUpdated() => _backgroundFade.FadeOut(0);

        private void OnReloadImageUpdated()
        {
            ActiveTimeImage.SetValue(UserUtils.MaxFilledImageValue);
            Animation.Restart();
        }
    }
}