using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class EnemySniperZone : EnemyAttackZone
    {
        private AimCross _cross;
        private bool _isCanShoot = true;

        private protected override void Update()
        {
            if (_isCanShoot == false)
                return;

            base.Update();
        }

        public override void Initialize(EnemyAttackConfig config, Transform firePoint)
        {
            base.Initialize(config, firePoint);

            SniperConfig sniperConfig = config.SafeCast<SniperConfig>();

            if(sniperConfig != null)
            {
                _cross = Instantiate(sniperConfig.AimCrossPrefab).GetComponent<AimCross>();
                _cross.Initialize(sniperConfig.AimCrossConfig, this);
                IsInitialized = true;
                return; 
            }

            IsInitialized = false;
        }

        public override void Return(GameObject gameObject)
        {
            _isCanShoot = true;
        }

        private protected override void Attack()
        {
            _isCanShoot = false;
            CurrentTime = 0;
            Debug.Log("attack");
            _cross.StartAimWarning();
        }  
    }
}