using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Bullet : EnemyMissileWithRenderer
    {
        private protected BulletConfig BulletConfig;

        private Vector3 _direction;
        private bool _isMoving;

        private protected override void Update()
        {
            if (IsCanLive() == false)
                return;

            base.Update();

            if (_isMoving)
                Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(UserUtils.Obstacle)
                || other.CompareTag(UserUtils.DissolvableObstacle)
                || other.CompareTag(UserUtils.Dropped))
                Interact();
        }

        public override void InitFromConfig(MissileConfig missileConfig, EnemyAttackZone attackZone)
        {
            base.InitFromConfig(missileConfig, attackZone);

            BulletConfig = missileConfig.SafeCast<BulletConfig>();

            if (BulletConfig != null)
            {
                IsConfigurated = true;
                return;
            }

            IsConfigurated = false;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            IsInitialized = true;
        }

        public void Send(Vector3 startPosition, Vector3 destination)
        {
            Transform.position = startPosition;
            _direction = (destination - Transform.position).normalized;
            _isMoving = true;
        }

        private protected override void Interact()
        {
            base.Interact();
            _isMoving = false;
        }

        private protected virtual void Move()
        {
            Transform.Translate(_direction * BulletConfig.Speed * Time.deltaTime);
        }
    }
}