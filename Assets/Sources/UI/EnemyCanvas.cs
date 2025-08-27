using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;

namespace Assets.Sources.UI
{
    public class EnemyCanvas : SmoothedFade
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private BillboardUI _billboardUI;

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

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            UserUtils.SetActiveElements(false, Elements);
            _billboardUI.IsStop(true);
            IsInitialized = true;
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

                _billboardUI.IsStop(false);
                UserUtils.SetActiveElements(true, Elements);
                CanvasGroup.alpha = 1f;
            }
            else
            {
                UpdateView(Duration);
            }
        }

        private protected override void OnRoutineEnd()
        {
            base.OnRoutineEnd();
            _billboardUI.IsStop(true);
        }
    }
}