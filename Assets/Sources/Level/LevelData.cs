using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private int _index;
        [SerializeField] private int _score;

        public LevelData(int levelIndex)
        {
            _index = levelIndex;
        }

        public int Index => _index;
        public int Score => _score;

        public void UpdateScore(int score)
        {
            if (score < 0)
                return;

            if (_score < score)
                _score = score;
        }
    }
}