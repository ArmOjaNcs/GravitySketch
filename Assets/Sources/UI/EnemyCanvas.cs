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

        private protected override void OnDisable()
        {
            base.OnDisable();
            _enemy.Detected -= OnDetected;
        }

        private protected override void Start()
        {
            UserUtils.SetActiveElements(false, Elements);
        }

        private void OnDetected(bool isDetected)
        {
            if (_enemy.IsDowned)
            {
                if(CanvasGroup.alpha > 0 && _fadeRoutineStarted == false)
                {
                    UpdateView(Duration);
                    _fadeRoutineStarted = true;
                }

                return;
            }

            if (isDetected)
            {
                if (Routine != null)
                    StopCoroutine(Routine);

                UserUtils.SetActiveElements(true, Elements);
                CanvasGroup.alpha = 1f;
            }
            else
            {
                UpdateView(Duration);
            }
        }
    }
}