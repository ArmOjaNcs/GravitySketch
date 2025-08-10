using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class SmoothedGrowBar : SmoothedImage
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private SmoothedFade _smoothedFade;

        private float _startImageValue;

        private void OnEnable()
        {
            _cubesCollector.CubesCountChanged += OnCubesUpdate;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();

            _cubesCollector.CubesCountChanged -= OnCubesUpdate;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            Image.fillAmount = 0;
            _smoothedFade.Init(pauseHandler);
            _smoothedFade.FadeOut();
            _smoothedFade.SetStartAplpha(UserUtils.HalfUnit);
            IsInitialized = true;
        }

        private void OnCubesUpdate(int cubesCount)
        {
            _smoothedFade.ShowElements();
            int previousGrowThreshold = _growHandler.CubesOnNextGrow - _growHandler.GrowDelta;
            TargetValue = ((float)cubesCount - previousGrowThreshold) / _growHandler.GrowDelta;
            SetValue(_startImageValue);
            OnUpdate();
            _startImageValue = ((float)cubesCount - previousGrowThreshold) / _growHandler.GrowDelta;

        }

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            yield return base.UpdateRoutine(duration);
            _smoothedFade.FadeOut();
        }
    }
}