using Assets.Sources.Pause;
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

        public float MoveSpeed => _mover.MoveSpeed;
        public float BoostSpeed => _booster.BoostSpeed;
        public float DefendTime => _shield.DefendTime;
        public float Damage => _catcher.Damage;

        public event Action Upgraded;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
                PauseableObjectsHandler.Pause();

            if (Input.GetKeyDown(KeyCode.E))
                PauseableObjectsHandler.Resume();
        }

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
        }

        private void OnGrowing()
        {
            _mover.UpgradeMoveSpeed();
            _shield.UpgradeShield();
            _booster.UpgradeBoost();
            Upgraded?.Invoke();
        }
    }
}