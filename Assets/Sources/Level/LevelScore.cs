using Assets.Sources.Save;
using Assets.Sources.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Sources.Level
{
    public abstract class LevelScore : MonoBehaviour
    {
        [SerializeField] private protected int Index;

        private PlayerProgress _progress;

        public int TotalScore => _progress.TotalScore;
        public int LevelsCount => _progress.LevelsCount;
        public int CurrentLevelIndex => _progress.CurrentLevelIndex;
        public int CurrentScore => _progress.CurrentScore;
        public IReadOnlyList<Color> CurrentColors => _progress.CurrentColors;
        public int ColorsCount => _progress.ColorsCount;

        private void Awake()
        {
            LoadProgress();
        }

        private protected void SetIntermediateResult(int index, int value, List<Color> colors)
        {
            _progress.SetIntermediateResult(index, value, colors);
        }

        private protected void LoadProgress()
        {
            _progress = SaveSystem.LoadPlayerProgress();
        }

        private protected void SaveProgress()
        {
            SaveSystem.SavePlayerProgress(_progress);
        }

        private protected void UpdateProgress(int levelIndex, int score)
        {
            _progress.UpdateLevelScore(levelIndex, score);
        }
        
        private protected int GetLevelScore(int index)=> _progress.GetLevelScore(index);

        private protected void LoadScene(string sceneName)
        {
            if (sceneName != string.Empty)
                SceneManager.LoadScene(sceneName);
            else
                SceneManager.LoadScene(UserUtils.MainMenu);
        }

        private protected void LoadNextScene()
        {
            string nextSceneName = UserUtils.GetSceneName(Index + (int)UserUtils.One);

            if (nextSceneName != string.Empty)
                SceneManager.LoadScene(nextSceneName);
            else
                SceneManager.LoadScene(UserUtils.MainMenu);
        }

        private protected void RestartScene()
        {
            string sceneName = UserUtils.GetSceneName(Index);

            if (sceneName != string.Empty)
                SceneManager.LoadScene(sceneName);
            else
                SceneManager.LoadScene(UserUtils.MainMenu);
        }
    }
}