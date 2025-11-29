using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Upgrader : MonoBehaviour
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private Booster _booster;
        [SerializeField] private Shield _shield;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Catcher _catcher;
        [SerializeField] private TakeOverLimit _takeOverLimit;

        public float MoveSpeed => _mover.MoveSpeed;
        public float DefendTime => _shield.DefendTime;
        public float Defence => _shield.Defence;
        public float Damage => _catcher.Damage;

        public event Action Upgraded;

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
            _takeOverLimit.EnemyDissolved += OnEnemyDissolved;
            _takeOverLimit.AnomalyDissolved += OnAnomalyDissolved;
        }

        private void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
            _takeOverLimit.AnomalyDissolved -= OnAnomalyDissolved;
        }

        private void OnAnomalyDissolved()
        {
            _shield.UpgradeActiveTime();
            Upgraded?.Invoke();
        }

        private void OnGrowing()
        {
            _mover.UpgradeMoveSpeed(true);
            _shield.Upgrade();
            _booster.Upgrade();
            Upgraded?.Invoke();
        }

        private void OnEnemyDissolved()
        {
            _catcher.UpgradeDamage();
            _mover.UpgradeMoveSpeed(false);
            _shield.UpgradeDefend();
            Upgraded?.Invoke();
        }
    }
}