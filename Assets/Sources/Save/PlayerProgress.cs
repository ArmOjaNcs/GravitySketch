using Assets.Sources.Level;
using Assets.Sources.Utils;
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
        [SerializeField] private int _currentScore;
        [SerializeField] private string _stageName;

        public PlayerProgress()
        {
            _levels = new List<LevelData>();
            _currentColors = new List<Color>();
            _stageName = UserUtils.Telescope;
        }

        public int TotalScore => _totalScore;
        public int LevelsCount => _levels.Count;
        public int CurrentScore => _currentScore;
        public IReadOnlyList<Color> CurrentColors => _currentColors;
        public int ColorsCount => _currentColors.Count;
        public string StageName => _stageName;

        public void SetIntermediateResult(int score, List<Color> colors)
        {
            _currentScore = score;
            _currentColors = colors;
        }

        public void UpdateLevelScore(string levelName, int score)
        {
            if (IsHasLevel(levelName, out LevelData levelScore))
            {
                levelScore.UpdateScore(score);
                UpdateTotalScore();
                Debug.Log($"level is in list, score updated, level name {levelName} new score {score}");
            }
            else
            {
                LevelData levelToAdd = new LevelData(levelName);
                levelToAdd.UpdateScore(score);
                _levels.Add(levelToAdd);
                UpdateTotalScore();
                Debug.Log($"level is NOT in list, score updated, level name {levelToAdd.Name} new score {score}");
            }
        }

        public int GetLevelScore(string levelName)
        {
            foreach (var level in _levels)
            {
                if (level.Name.Equals(levelName))
                    return level.Score;
            }

            return 0;
        }

        public void SetStageName(string name)
        {
            _stageName = name;
            Debug.Log($"stage name setted as {_stageName}");
        }

        private bool IsHasLevel(string levelName, out LevelData levelData)
        {
            levelData = _levels.Find(levelScore => levelScore.Name.Equals(levelName));

            if (levelData != null)
                return true;

            return false;
        }

        private void UpdateTotalScore()
        {
            _totalScore = _levels.Sum(level => level.Score);
        }
    }
}