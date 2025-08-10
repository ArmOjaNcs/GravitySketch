using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class ScoreUI : SmoothedText
    {
        [SerializeField] private PlayerScore _playerScore;
        [SerializeField] private PopUpText _popUpText;

        private void OnEnable()
        {
            _playerScore.ScoreChanged += OnScoreChanged;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _playerScore.ScoreChanged -= OnScoreChanged;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _popUpText.Init(pauseHandler);
            StartText = "Score ";
            SplitSign = UserUtils.DefaultChar;
            IsNeedToSplit = false;
            Text.text = GetTotalText();
            IsInitialized = true;
        }

        private void OnScoreChanged(int reward)
        {
            TargetValue = _playerScore.Value;
            _popUpText.SetPreviousValue(0);
            _popUpText.ShowText(reward);
            OnUpdate();
        }
    }
}