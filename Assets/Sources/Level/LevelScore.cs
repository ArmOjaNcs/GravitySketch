using Assets.Sources.Save;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Level
{
    public abstract class LevelScore : MonoBehaviour
    {
        private PlayerProgress _progress;

        public int TotalScore => _progress.TotalScore;
        public int CurrentLevelIndex => _progress.CurrentLevelIndex;
        public int CurrentScore => _progress.CurrentScore;
        public IReadOnlyList<Color> CurrentColors => _progress.CurrentColors;
        public int ColorsCount => _progress.ColorsCount;

        private protected virtual void Awake()
        {
            LoadProgress();
        }

        private protected void SetIntermediateResult(int index, int value, List<Color> colors)
        {
            _progress.SetIntermediateResult(index, value, colors);
        }

        private protected void LoadProgress()
        {
            _progress = SaveSystem.Load();
        }

        private protected void SaveProgress()
        {
            SaveSystem.Save(_progress);
        }

        private protected void UpdateProgress(int levelIndex, int score)
        {
            _progress.UpdateLevelScore(levelIndex, score);
        }
    }
}