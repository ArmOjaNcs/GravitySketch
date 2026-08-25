using EnemyScripts.Factory;
using PlayerScripts;
using TMPro;
using UI.PauseableRoutineUI;
using Utils;
using UnityEngine;

namespace UI.Stats
{
    public class EnemiesCountUI : MonoBehaviour
    {
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private TextMeshProUGUI _maxText;
        [SerializeField] private SmoothedSlider _slider;

        private int _totalEnemies;

        private void OnEnable()
        {
            _takeOverLimit.EnemyDissolved += OnEnemyDissolved;
            _enemyFactory.EnemiesSpawned += OnEnemiesSpawned;
        }

        private void OnDisable()
        {
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
            _enemyFactory.EnemiesSpawned -= OnEnemiesSpawned;
        }

        private void Start()
        {
            _slider.SetStartValue(0);
            _valueText.text = _takeOverLimit.EnemiesDissolvedCount.ToString();
        }

        private void OnEnemyDissolved()
        {
            _valueText.text = _takeOverLimit.EnemiesDissolvedCount.ToString();
            float target = _totalEnemies > 0 ? (float)_takeOverLimit.EnemiesDissolvedCount / _totalEnemies : 0f;
            _slider.UpdateValue(UserUtils.SliderUpdateDuration, target);
        }

        private void OnEnemiesSpawned()
        {
            _totalEnemies = _enemyFactory.TotalEnemies;
            _maxText.text = _totalEnemies.ToString();
        }
    }
}