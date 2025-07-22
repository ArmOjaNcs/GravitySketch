using Assets.Sources.PlayerScripts;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class EnemyAttackZone : EnemyZone, IEnemyAttack
    {
        private protected Transform FirePoint;
        private protected float CurrentTime;
        private protected float AttackRate;
        private protected bool IsAttacking;
        
        private protected virtual void Update()
        {
            if (IsInitialized == false || IsAttacking == false || IsPaused || Player == null)
                return;

            if (Player.gameObject.activeSelf == false)
                return;

            CurrentTime += Time.deltaTime;

            if (CurrentTime > AttackRate)
                Attack();
        }

        public virtual void Initialize(EnemyAttackConfig config, Transform firePoint)
        {
            FirePoint = firePoint;
            AttackRate = config.AttackRate;
        }

        public virtual void Return(GameObject gameObject) => gameObject.SetActive(false);

        private protected override void PlayerDetected(Collider playerCollider)
        {
            base.PlayerDetected(playerCollider);
            IsAttacking = true;
        }

        private protected virtual void Attack()
        {
            if (Player == null)
                return;

            CurrentTime = 0;
        }
       
        private protected override void PlayerLosed(Collider playerCollider)
        {
            IsAttacking = false;
            CurrentTime = 0;
        }
    }
}