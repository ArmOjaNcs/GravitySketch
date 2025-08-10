using Assets.Sources.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Save
{
    [Serializable]
    public class PlayerProgress
    {
        [SerializeField] private int _totalScore;
        [SerializeField] private List<LevelData> _levels;
        [SerializeField] private List<Color> _currentColors;
        [SerializeField] private int _currentLevelIndex;
        [SerializeField] private int _currentScore;

        public PlayerProgress()
        {
            _levels = new List<LevelData>();
            _currentColors = new List<Color>();
        }

        public int TotalScore => _totalScore;
        public int CurrentLevelIndex => _currentLevelIndex;
        public int LevelsCount => _levels.Count;
        public int CurrentScore => _currentScore;
        public IReadOnlyList<Color> CurrentColors => _currentColors;
        public int ColorsCount => _currentColors.Count;

        public void SetIntermediateResult(int levelIndex, int score, List<Color> colors)
        {
            _currentLevelIndex = levelIndex;
            _currentScore = score;
            _currentColors = colors;
        }

        public void UpdateLevelScore(int levelIndex, int score)
        {
            if (IsHasLevel(levelIndex, out LevelData levelScore))
            {
                levelScore.UpdateScore(score);
                UpdateTotalScore();
            }
            else
            {
                LevelData levelToAdd = new LevelData(levelIndex);
                levelToAdd.UpdateScore(score);
                _levels.Add(levelToAdd);
                UpdateTotalScore();
            }
        }

        public int GetLevelScore(int levelIndex)
        {
            foreach (var level in _levels)
            {
                if (level.Index == levelIndex)
                    return level.Score;
            }

            return 0;
        }

        private bool IsHasLevel(int levelIndex, out LevelData levelScore)
        {
            levelScore = _levels.Find(levelScore => levelScore.Index == levelIndex);

            if (levelScore != null)
                return true;

            return false;
        }

        private void UpdateTotalScore()
        {
            _totalScore = _levels.Sum(level => level.Score);
        }
    }
}