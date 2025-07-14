using Assets.Sources.ColorizerScripts;
using Assets.Sources.Level;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Sources.ScoreScripts
{
    public class ScoreView : MonoBehaviour
    {
        private const string TotalScore = "Total score: ";
        private const string CollectScore = "Collect score: ";
        private const string PaintScore = "Paint score: ";
        private const float ShowTime = 0.1f;
        private const float CalculateTime = 2f;

        [SerializeField] private TextMeshProUGUI _totalScore;
        [SerializeField] private TextMeshProUGUI _collectScore;
        [SerializeField] private TextMeshProUGUI _paintScore;
        [SerializeField] private PaintStage _paintStage;
        [SerializeField] private Validator _validator;
        [SerializeField] private Colorizer _colorizer;

        private void Start()
        {
            _totalScore.text = "";
            _collectScore.text = CollectScore + _paintStage.CurrentScore;
            _paintScore.text = PaintScore;
        }

        private void OnEnable()
        {
            _validator.Matched += OnMatched;
            _paintStage.TotalScoreUpdated += OnTotalScoreUpdated;
        }

        private void OnDisable()
        {
            _validator.Matched -= OnMatched;
            _paintStage.TotalScoreUpdated -= OnTotalScoreUpdated;
        }

        private void OnMatched()
        {
            _paintScore.text = PaintScore + _validator.MatchScore;
        }

        private void OnTotalScoreUpdated(int totalScore)
        {
            StartCoroutine(ShowFinalResult(totalScore));
        }

        private IEnumerator ShowFinalResult(int totalScore)
        {
            float elapsedTime = 0;
            float totalTime = TotalScore.Length * ShowTime;
            int index = 0;
            string message = string.Empty;

            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime > ShowTime * index)
                {
                    if (index < TotalScore.Length)
                    {
                        message += TotalScore[index];
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

            while (elapsedTime < CalculateTime)
            {
                elapsedTime += Time.deltaTime;
                float normalizedPosition = elapsedTime / CalculateTime;
                float result = Mathf.Lerp(_paintStage.CurrentScore, totalScore, normalizedPosition);
                _totalScore.text = TotalScore + Mathf.Round(result);

                yield return null;
            }
        }
    }
}