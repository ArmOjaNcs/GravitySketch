using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class PlayerScore : MonoBehaviour
    {
        [SerializeField] private TakeOverLimit _takeOverLimit;

        public event Action<int> ScoreChanged;

        public int Value { get; private set; }

        private void OnEnable()
        {
            _takeOverLimit.Rewarded += OnRewarded;
        }

        private void OnDisable()
        {
            _takeOverLimit.Rewarded -= OnRewarded;
        }

        private void OnRewarded(int reward)
        {
            if (reward <= 0)
                return;

            Value += reward;
            ScoreChanged?.Invoke(reward);
        }
    }
}