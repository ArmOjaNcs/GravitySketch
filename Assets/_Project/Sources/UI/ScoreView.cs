using Assets.Sources.ColorizerScripts;
using Assets.Sources.Level;
using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class ScoreView : SmoothedText
    {
        [SerializeField] private Validator _validator;
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private AudioSource _calculateSound;

        private int _startScore;

        private void OnEnable()
        {
            _validator.Matched += OnMatched;
        }

        private protected override void OnDisable()
        {
            _validator.Matched -= OnMatched;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);

            StartText = "";
            IsNeedToSplit = false;
            IsInitialized = true;
        }

        public void SetStartScore(int startScore)
        {
            if(startScore < 0)
                startScore = 0;
            
            _startScore = startScore;
            UpdateView(0, _startScore);
        }

        private void OnMatched()
        {
            TargetValue = _validator.MatchScore + _startScore;
            UpdateView(Duration);
        }
    }
}