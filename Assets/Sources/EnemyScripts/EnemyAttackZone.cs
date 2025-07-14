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
        
        public Player Player {  get; protected set; }

        private protected virtual void Update()
        {
            if (IsInitialized == false || IsAttacking == false || IsPaused || Player == null)
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
            if(Player == null)
                if (playerCollider.TryGetComponent(out Player player))
                    Player = player;

            IsAttacking = true;
        }

        private protected abstract void Attack();
       
        private protected override void PlayerLosed(Collider playerCollider)
        {
            IsAttacking = false;
            CurrentTime = 0;
        }
    }
}