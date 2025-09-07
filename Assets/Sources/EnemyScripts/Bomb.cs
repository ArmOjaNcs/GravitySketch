using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class Bomb : EnemyMissileWithRenderer
    {   
        private float _blinkTimer;
        private bool _blinkState;
        private Vector3 _currentVelocity;
        private Collider _collider;

        private protected BombConfig BombConfig;
        private protected Rigidbody Rigidbody;

        private protected override void OnEnable()
        {
            base.OnEnable();

            if(BombConfig != null)
                SetColor(BombConfig.Color);

            _blinkTimer = 0;
            _blinkState = false;
            gameObject.layer = UserUtils.DefaultLayer;
        }

        private protected override void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            base.Update();
            Blink();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(gameObject.layer == UserUtils.DefaultLayer)
            {
                if(collision.gameObject.layer == UserUtils.ShieldLayer)
                {
                    Interact();
                    return;
                }
                
                if (collision.gameObject.CompareTag(UserUtils.Obstacle) ||
               collision.gameObject.CompareTag(UserUtils.DissolvableObstacle)
               || collision.gameObject.CompareTag(UserUtils.Dropped))
                {
                    gameObject.layer = UserUtils.PhysicalMissileLayer;
                    _collider.enabled = false;
                    _collider.enabled = true;
                }
            }
           
            if (collision.gameObject.CompareTag(UserUtils.Player))
                Interact();
        }

        public override void Pause()
        {
            base.Pause();

            if(Rigidbody != null)
            {
                _currentVelocity = Rigidbody.velocity;
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.isKinematic = true;
            }
        }

        public override void Resume()
        {
            base.Resume();

            if (Rigidbody != null)
            {
                Rigidbody.isKinematic = false;
                Rigidbody.velocity = _currentVelocity;
            }
        }

        public override void InitFromConfig(MissileConfig config, EnemyAttackZone attackZone)
        {
            base.InitFromConfig(config, attackZone);
            Rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            BombConfig = config.SafeCast<BombConfig>();

            if(config != null)
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

        private void Blink()
        {
            _blinkTimer += Time.deltaTime;

            if (_blinkTimer >= 1f / BombConfig.BlinkFrequency)
            {
                _blinkTimer = 0;
                _blinkState = !_blinkState;
                SetColor(_blinkState ? BombConfig.Color : BombConfig.WarningColor);
            }
        }
    }
}