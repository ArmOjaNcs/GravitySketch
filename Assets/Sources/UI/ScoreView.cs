using Assets.Sources.ColorizerScripts;
using Assets.Sources.Level;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class ScoreView : SmoothedText
    {
        [SerializeField] private TextMeshProUGUI _totalScore;
        [SerializeField] private TextMeshProUGUI _collectScore;
        [SerializeField] private PaintStage _paintStage;
        [SerializeField] private Validator _validator;
        [SerializeField] private Colorizer _colorizer;

        private void Start()
        {
            _totalScore.text = "";
            _collectScore.text = UserUtils.CollectScore + _paintStage.CurrentScore;
            Text.text = UserUtils.PaintScore;
        }

        private void OnEnable()
        {
            _validator.Matched += OnMatched;
            _paintStage.TotalScoreUpdated += OnTotalScoreUpdated;
        }

        private protected override void OnDisable()
        {
            _validator.Matched -= OnMatched;
            _paintStage.TotalScoreUpdated -= OnTotalScoreUpdated;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            StartText = UserUtils.PaintScore;
            IsNeedToSplit = false;
            IsInitialized = true;
        }

        private void OnMatched()
        {
            TargetValue = _validator.MatchScore;
            UpdateView(Duration);
        }

        private void OnTotalScoreUpdated(int totalScore)
        {
            StartCoroutine(ShowFinalResult(totalScore));
        }

        private IEnumerator ShowFinalResult(int totalScore)
        {
            float elapsedTime = 0;
            float totalTime = UserUtils.TotalScore.Length * UserUtils.ShowTime;
            int index = 0;
            string message = string.Empty;

            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime > UserUtils.ShowTime * index)
                {
                    if (index < UserUtils.TotalScore.Length)
                    {
                        message += UserUtils.TotalScore[index];
                        _totalScore.text = message;
                        index++;
                    }
                }

                yield return null;
            }

            yield return CalculateTotalScore(totalScore);
        }

        private IEnumerator CalculateTotalScore(int totalScore)
        {
            float elapsedTime = 0;

            while (elapsedTime < UserUtils.CalculateTime)
            {
                elapsedTime += Time.deltaTime;
                float normalizedPosition = elapsedTime / UserUtils.CalculateTime;
                float result = Mathf.Lerp(_paintStage.CurrentScore, totalScore, normalizedPosition);
                _totalScore.text = UserUtils.TotalScore + Mathf.Round(result);

                yield return null;
            }
        }
    }
}