using TMPro;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.EnemyScripts;
using Assets.Sources.PlayerScripts;

namespace Assets.Sources.UI
{
    public class EnemiesCountUI : MonoBehaviour
    {
        private const string EnemiesCount = "Enemies killed: ";

        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private TextMeshProUGUI _text;

        private int _totalEnemies;

        private void OnEnable()
        {
            _takeOverLimit.EnemyDissolved += OnEnemyDissolved;
        }

        private void OnDisable()
        {
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
        }

        private void Start()
        {
            OnEnemyDissolved();
        }

        private void OnEnemyDissolved()
        {
            if (_totalEnemies == 0)
                _totalEnemies = _enemyFactory.TotalEnemies;

            float percent = _totalEnemies > 0 ? (float)_takeOverLimit.EnemiesDissolvedCount / _totalEnemies : 0f;
            percent = Mathf.Clamp01(percent);

            _text.color = UserUtils.GetColorByPercentage(percent);

            _text.text = EnemiesCount + _takeOverLimit.EnemiesDissolvedCount + "/" + _totalEnemies + " "
                + (percent * 100).ToString("F2") + "%";
        }
    }
}