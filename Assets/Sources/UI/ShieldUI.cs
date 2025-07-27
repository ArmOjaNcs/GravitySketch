using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class ShieldUI : MonoBehaviour
    {
        [SerializeField] private Shield _shield;
        [SerializeField] private SmoothedImage _defendFill;
        [SerializeField] private SmoothedImage _reloadFill;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private SmoothedFade _defendFade;
        [SerializeField] private SmoothedFade _reloadFade;

        private Tween _shakeAnimation;

        private void OnEnable()
        {
            _shield.DefendApplied += OnDefendStart;
            _shield.Reloading += OnReloadingStart;
            _defendFill.Updated += OnDefendUpdated;
            _reloadFill.Updated += OnReloadUpdated;
        }

        private void OnDisable()
        {
            _shield.DefendApplied -= OnDefendStart;
            _shield.Reloading -= OnReloadingStart;
            _defendFill.Updated -= OnDefendUpdated;
            _reloadFill.Updated -= OnReloadUpdated;

            if (_shakeAnimation != null)
                _shakeAnimation.Kill();
        }

        private void Start()
        {
            _shakeAnimation = AnimationSpawner.GetShakeAnimation(_text.rectTransform, 0.5f);
            _defendFade.HideElements();
            _reloadFade.HideElements();
        }

        private void OnDefendStart()
        {
            _defendFade.ShowElements();
            _defendFill.SetValue(1);
            _defendFill.UpdateView(_shield.DefendTime, 0);
        }

        private void OnReloadingStart()
        {
            _reloadFade.ShowElements();
            _reloadFill.SetValue(1);
            float reloadDuration = _shield.CycleTime - _shield.DefendTime;
            _reloadFill.UpdateView(reloadDuration, 0);
        }

        private void OnReloadUpdated()
        {
            _text.color = _reloadFill.Color;
            _shakeAnimation.OnComplete(() => _text.color = Color.black);
            _shakeAnimation.Restart();
            _reloadFade.FadeOut();
        }

        private void OnDefendUpdated()
        {
            _defendFade.FadeOut();
        }
    }
}