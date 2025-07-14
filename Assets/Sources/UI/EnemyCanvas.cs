using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.EnemyScripts;

namespace Assets.Sources.UI
{
    public class EnemyCanvas : SmoothedFade
    {
        [SerializeField] private Enemy _enemy;

        private bool _fadeRoutineStarted;

        private void OnEnable()
        {
            _enemy.Detected += OnDetected;
        }

        private void OnDisable()
        {
            _enemy.Detected -= OnDetected;
        }

        private void Start()
        {
            UserUtils.SetActiveElements(false, Elements);
        }

        private void OnDetected(bool isDetected)
        {
            if (_enemy.IsDowned)
            {
                if(CanvasGroup.alpha > 0 && _fadeRoutineStarted == false)
                {
                    StartFadeRoutine();
                    _fadeRoutineStarted = true;
                }

                return;
            }

            if (isDetected)
            {
                if (FadeRoutine != null)
                    StopCoroutine(FadeRoutine);

                UserUtils.SetActiveElements(true, Elements);
                CanvasGroup.alpha = 1f;
            }
            else
            {
                StartFadeRoutine();
            }
        }

        private void StartFadeRoutine()
        {
            if (FadeRoutine != null)
                StopCoroutine(FadeRoutine);

            FadeRoutine = StartCoroutine(FadeOut(FadeDuration, CanvasGroup, Elements));
        }
    }
}