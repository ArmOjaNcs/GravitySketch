using Assets.Sources.Save;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Level
{
    public abstract class LevelScore : MonoBehaviour
    {
        private protected PlayerProgress Progress;

        public int TotalScore => Progress.TotalScore;
        public int LevelsCount => Progress.LevelsCount;
        public int CurrentScore => Progress.CurrentScore;
        public IReadOnlyList<Color> CurrentColors => Progress.CurrentColors;
        public int ColorsCount => Progress.ColorsCount;
        public string StageName => Progress.StageName;
        public bool IsTutorial => Progress.IsTutorial; 

        private protected virtual void Awake()
        {
            Progress = SaveSystem.LoadPlayerProgress();
        }

        public int GetLevelScore(string levelName)
        {
            return Progress.GetLevelScore(levelName);
        }
    }
}