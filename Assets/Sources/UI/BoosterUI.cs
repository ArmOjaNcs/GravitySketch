using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class BoosterUI : MonoBehaviour
    {
        [SerializeField] private Booster _booster;
        [SerializeField] private SmoothedImage _boostTimeImage;
        [SerializeField] private SmoothedImage _reloadTimeImage;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private SmoothedFade _boostTimeFade;
        [SerializeField] private SmoothedFade _reloadTimeFade;

        private Tween _shakeAnimation;

        private void OnEnable()
        {
            _booster.BoostApplied += OnBoostApplied;
            _booster.Reloading += OnReloading;
            _booster.Reloaded += OnReloaded;
            _boostTimeImage.Updated += OnBoostUpdated;
            _reloadTimeImage.Updated += OnReloadUpdated;
        }

        private void OnDisable()
        {
            _booster.BoostApplied -= OnBoostApplied;
            _booster.Reloading -= OnReloading;
            _booster.Reloaded -= OnReloaded;
            _boostTimeImage.Updated -= OnBoostUpdated;
            _reloadTimeImage.Updated -= OnReloadUpdated;

            if (_shakeAnimation != null)
                _shakeAnimation.Kill();
        }

        private void Start()
        {
            _shakeAnimation = AnimationSpawner.GetShakeAnimation(_text.transform, 0.5f);
            _boostTimeFade.HideElements();
            _reloadTimeFade.HideElements();
        }

        private void OnReloadUpdated()
        {
            if (_booster.CurrentBoostCount == _booster.BoostCount)
                _reloadTimeFade.FadeOut();
        }

        private void OnBoostUpdated()
        {
            _boostTimeFade.FadeOut();
        }

        private void OnBoostApplied(float speed)
        {
            if (speed <= 0)
                return;

            _boostTimeFade.ShowElements();
            _boostTimeImage.SetValue(1);
            _boostTimeImage.UpdateView(_booster.BoostTime, 0);
        }

        private void OnReloading()
        {
            _reloadTimeFade.ShowElements();
            _reloadTimeImage.SetValue(1);
            _reloadTimeImage.UpdateView(_booster.BoostReloadTime, 0);
        }

        private void OnReloaded()
        {
            _text.color = _reloadTimeImage.Color;
            _shakeAnimation.OnComplete(() => _text.color = Color.black);
            _shakeAnimation.Restart();
        }
    }
}