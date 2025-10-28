using Assets.Sources.UI;
using Assets.Sources.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Level
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private LevelButton[] _levels;
        [SerializeField] private Button _play;
        [SerializeField] private TextMeshProUGUI _totalScore;

        private LevelScore _levelScore;
        private LevelButton _currentButton;

        public event Action<string> PlayClicked;

        private void OnDisable()
        {
            _play.onClick.RemoveListener(OnPlayClicked);

            foreach (LevelButton level in _levels)
            {
                if (level.gameObject.activeSelf)
                {
                    level.Chosen -= OnLevelChosen;
                    level.Dispose();
                }
            }
        }

        public void Init(LevelScore levelScore)
        {
            _levelScore = levelScore;
            string translatedText = Translator.Get(UserUtils.TotalScore);
            _totalScore.text = translatedText + " " + _levelScore.TotalScore;
            _play.onClick.AddListener(OnPlayClicked);
            _play.gameObject.SetActive(false);

            //for (int i = 0; i <= _levelScore.LevelsCount && i < _levels.Length; i++)
            //{
            //    _levels[i].gameObject.SetActive(true);
            //    _levels[i].Init();
            //    _levels[i].Chosen += OnLevelChosen;
            //}

            foreach(LevelButton level in _levels)
            {
                level.gameObject.SetActive(true);
                level.Init();
                level.Chosen += OnLevelChosen;
            }
        }

        private void OnLevelChosen(LevelButton level)
        {
            if (_currentButton != null)
                _currentButton.Hide();

            _currentButton = level;
            level.SetScore(_levelScore.GetLevelScore(level.Name));
            level.Show();

            if (_play.gameObject.activeSelf == false)
                _play.gameObject.SetActive(true);
        }

        private void OnPlayClicked() => PlayClicked?.Invoke(_currentButton.Name); 
    }
}