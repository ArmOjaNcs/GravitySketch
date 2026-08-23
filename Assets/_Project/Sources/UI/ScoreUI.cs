using Audio;
using Pause;
using PlayerScripts;
using UI.PauseableRoutineUI;
using UnityEngine;

namespace UI
{
    public class ScoreUI : SmoothedText
    {
        [SerializeField] private PlayerScore _playerScore;
        [SerializeField] private AudioPlayer _audioPlayer;

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
            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = true;
            IsNeedToSplit = false;
            Text.SetText("{0}", 0);
            IsInitialized = true;
        }

        private protected override void OnRoutineEnd()
        {
            _audioPlayer.Stop();
            base.OnRoutineEnd();
        }

        private void OnScoreChanged(int reward)
        {
            TargetValue = _playerScore.Value;
            _audioPlayer.Play();
            OnUpdate();
        }
    }
}