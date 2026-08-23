using System;
using UnityEngine;

namespace Level
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private string _name;
        [SerializeField] private int _score;

        public LevelData(string name)
        {
            _name = name;
        }

        public string Name => _name;
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