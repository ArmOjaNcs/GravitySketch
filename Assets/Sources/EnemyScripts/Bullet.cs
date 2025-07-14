using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Bullet : EnemyMissileWithRenderer
    {
        private Vector3 _direction;

        private protected BulletConfig BulletConfig;

        private protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(UserUtils.Obstacle) || other.CompareTag(UserUtils.DissolvableObject))
                Interact();
        }

        private protected override void Update()
        {
            base.Update();

            Move();
        }

        public override void Initialize(MissileConfig missileConfig, EnemyAttackZone attackZone)
        {
            base.Initialize(missileConfig, attackZone);

            BulletConfig = missileConfig.SafeCast<BulletConfig>();

            if (BulletConfig != null)
            {
                IsInitialized = true;
                return;
            }

            IsInitialized = false;
        }

        public void Send(Vector3 startPosition, Vector3 destination)
        {
            Transform.position = startPosition;
            _direction = (destination - Transform.position).normalized;
        }

        private protected override void Live()
        {
            base.Live();
        }

        private protected virtual void Move()
        {
            Transform.Translate(_direction * BulletConfig.Speed * Time.deltaTime);
        }
    }
}