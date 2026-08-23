using EnemyScripts.EnemyZones;
using Missile.Configs;
using Pause;
using Utils;
using UnityEngine;

namespace Missile
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class Bomb : EnemyMissileWithRenderer
    {
        private protected BombConfig BombConfig;
        private protected Rigidbody Rigidbody;

        private float _blinkTimer;
        private bool _blinkState;
        private Vector3 _currentVelocity;
        private Collider _collider;
        private float _defaultMass;
        private float _minMass = 0.0001f;

        private protected override void OnEnable()
        {
            base.OnEnable();

            if (BombConfig != null)
                SetColor(BombConfig.Color);

            if (Rigidbody != null)
                Rigidbody.mass = _defaultMass;

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
            if (Rigidbody.mass > _minMass)
                Rigidbody.mass = _minMass;

            if (gameObject.layer == UserUtils.DefaultLayer)
            {
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

            if (Rigidbody != null)
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
            _defaultMass = Rigidbody.mass;

            if (config != null)
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