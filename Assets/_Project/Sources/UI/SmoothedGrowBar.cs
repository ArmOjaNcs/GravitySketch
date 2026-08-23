using System.Collections;
using Pause;
using PlayerScripts;
using UI.GrowerUI;
using UI.PauseableRoutineUI;
using Utils;
using UnityEngine;

namespace UI
{
    public class SmoothedGrowBar : SmoothedImage
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private SmoothedFade _smoothedFade;
        [SerializeField] private GrowBarBillboard _growBarBillboard;

        private bool _isStarting;
        private bool _isShowed;

        private int PreviousGrowThreshold => _growHandler.CubesOnNextGrow - _growHandler.GrowDelta;

        private void OnEnable()
        {
            _cubesCollector.CubesCountChanged += OnCubesUpdate;
            _smoothedFade.Updated += OnFadeUpdated;
            _growHandler.Growing += OnGrowing;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();

            _cubesCollector.CubesCountChanged -= OnCubesUpdate;
            _smoothedFade.Updated -= OnFadeUpdated;
            _growHandler.Growing -= OnGrowing;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            Image.fillAmount = 0;
            _smoothedFade.Init(pauseHandler);
            _smoothedFade.FadeOut(0);
            _growBarBillboard.IsStop(true);
            IsInitialized = true;
        }

        private protected override void OnRoutineStart()
        {
            if (_isStarting)
            {
                StartImageValue = 0;
                _isStarting = false;
            }
            else
            {
                StartImageValue = Image.fillAmount;
            }

            SetValue(StartImageValue);
        }

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            yield return base.UpdateRoutine(duration);
            _isShowed = false;
            _smoothedFade.FadeOut(UserUtils.HalfOfUnit);
        }

        private void OnCubesUpdate(int cubesCount)
        {
            if (_growHandler.IsCanGrow == false)
                return;

            _smoothedFade.FadeIn(UserUtils.HalfOfUnit, UserUtils.Unit);
            _isShowed = true;
            TargetValue = ((float)_cubesCollector.CubesCount - PreviousGrowThreshold) / _growHandler.GrowDelta;
            _growBarBillboard.IsStop(false);
            OnUpdate();
        }

        private void OnGrowing()
        {
            _isStarting = true;
        }

        private void OnFadeUpdated()
        {
            if (_isShowed)
                return;

            _growBarBillboard.IsStop(true);
        }
    }
}