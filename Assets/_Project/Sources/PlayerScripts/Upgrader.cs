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

        private float _totalPower;

        public event Action Started;

        public event Action Upgraded;

        public float MoveSpeed => _mover.MoveSpeed;

        public float DefendTime => _shield.DefendTime;

        public float Defence => _shield.Defence;

        public float Damage => _catcher.Damage;

        public int CurrentSize => _growHandler.CurrentSize;

        public int Power { get; private set; }

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
            _takeOverLimit.EnemyDissolved += OnEnemyDissolved;
            _takeOverLimit.AnomalyDissolved += OnAnomalyDissolved;
            CalculatePower();
        }

        private void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
            _takeOverLimit.EnemyDissolved -= OnEnemyDissolved;
            _takeOverLimit.AnomalyDissolved -= OnAnomalyDissolved;
        }

        private void Start()
        {
            CalculatePower();
            Started?.Invoke();
        }

        private void OnAnomalyDissolved()
        {
            _shield.UpgradeActiveTime();
            UpdatePower();
        }

        private void OnGrowing()
        {
            _mover.UpgradeMoveSpeed(true);
            _shield.Upgrade();
            _booster.Upgrade();
            UpdatePower();
        }

        private void OnEnemyDissolved()
        {
            _catcher.UpgradeDamage();
            _mover.UpgradeMoveSpeed(false);
            _shield.UpgradeDefend();
            UpdatePower();
        }

        private void UpdatePower()
        {
            CalculatePower();
            Upgraded?.Invoke();
        }

        private void CalculatePower()
        {
            float moveSpeedPower = MoveSpeed * 0.75f;
            float damagePower = Damage * 0.5f;
            float defencePower = Defence * 1.5f;
            float defendTimePower = DefendTime * 5;
            _totalPower = moveSpeedPower + damagePower + defencePower + defendTimePower;
            Power = Mathf.RoundToInt(_totalPower);
        }
    }
}