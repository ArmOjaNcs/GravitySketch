using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class HoleGrowText : PauseableRoutine
    {
        private const string GrowUp = "Grow UP";

        [SerializeField] private TextMeshProUGUI _growText;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private SmoothedFade _smoothedFade;

        public event Action Updated;

        private void OnEnable()
        {
            _growHandler.Growing += OnUpdate;
        }

        private protected override void OnDisable()
        {
            _growHandler.Growing -= OnUpdate;
            base.OnDisable();
        }

        private protected override void Start()
        {
            base.Start();
            _growText.text = GrowUp;
            _smoothedFade.SetStartAplpha(UserUtils.HalfUnit);
            _smoothedFade.FadeOut();
        }

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            float elapsedTime = 0;
            _smoothedFade.ShowElements();

            while (elapsedTime < Duration)
            {
                elapsedTime += Time.deltaTime;

                if (CurrentTime < elapsedTime)
                    CurrentTime = elapsedTime;

                yield return null;
            }

            Routine = null;
            CurrentTime = 0;
            _smoothedFade.FadeOut();
        }
    }
}