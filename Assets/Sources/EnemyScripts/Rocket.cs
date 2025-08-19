using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class Rocket : Bullet
    {
        [SerializeField] private ParticleSystem _flame;
        [SerializeField] private AudioPlayer _nozzleSound;

        private RocketConfig _config;
        private Vector3 _delayedTargetPosition;
        private bool _isLaunched;
        private Rigidbody _rigidbody;
        private Vector3 _currentVelocity;

        private protected override void Update()
        {
            if (IsCanLive() == false)
                return;

            if (_isLaunched)
            {
                FindPosition();
                Live();
            }

            if (IsInteracted)
                EndLife();
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;

            if (IsLaunched() == false)
                return;

            Move();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(UserUtils.Obstacle)
               || collision.gameObject.CompareTag(UserUtils.DissolvableObject)
               || collision.gameObject.layer == UserUtils.ShieldLayer)
                Interact();
        }

        public override void InitFromConfig(MissileConfig missileConfig, EnemyAttackZone attackZone)
        {
            base.InitFromConfig(missileConfig, attackZone);
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;

            _config = missileConfig.SafeCast<RocketConfig>();

            if (_config != null)
            {
                IsConfigurated = true;
                return;
            }

            IsConfigurated = false;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _nozzleSound.Init(pauseHandler);
            _nozzleSound.AudioSource.playOnAwake = false;
            _nozzleSound.AudioSource.loop = true;
            _nozzleSound.AudioSource.spatialBlend = 1;
            IsInitialized = true;
        }

        public void Launch()
        {
            _delayedTargetPosition = AttackZone.Player.Position;
            _isLaunched = true;
            _flame.Play();
            _nozzleSound.Play();
        }

        public override void Pause()
        {
            base.Pause();
            _flame.Pause();

            if (_rigidbody != null)
            {
                _currentVelocity = _rigidbody.velocity;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }
        }

        public override void Resume()
        {
            base.Resume();
            _flame.Play();

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _currentVelocity;
            }
        }

        private protected override void Interact()
        {
            Effect.transform.SetParent(null);
            Effect.transform.localScale = Vector3.one + Vector3.one * Transform.localScale.x;
            _isLaunched = false;
            _flame.Stop();
            _nozzleSound.Stop();
            base.Interact();
        }

        private protected override void Move()
        {
            Vector3 directionToTarget = (_delayedTargetPosition - Transform.position).normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, directionToTarget);

            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, targetRotation,
                _config.MaxTurnAngle * Time.fixedDeltaTime * _config.RotationSpeed);
            _rigidbody.velocity = Transform.up * _config.Speed;
        }

        private void FindPosition()
        {
            _delayedTargetPosition = Vector3.Lerp(_delayedTargetPosition,
                AttackZone.Player.Position, Time.deltaTime / _config.ReactionDelay);
        }

        private bool IsLaunched()
        {
            if (IsInitialized == false || _isLaunched == false)
                return false;

            return true;
        }
    }
}