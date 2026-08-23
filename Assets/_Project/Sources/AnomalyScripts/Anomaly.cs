using Dissolvable;
using Pause;
using PlayerScripts;
using Utils;
using UnityEngine;

namespace AnomalyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class Anomaly : DissolvableObstacle
    {
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private WaitRoutine _routine;
        [SerializeField] private PointMover _mover;

        private Player _player;
        private bool _isAttack;
        private bool _isDowned;

        private int Damage => Size;

        private protected override void Awake()
        {
            SetPhysicalIndicators();
            CollidersHolder.SetActive(false);
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _routine.Updated -= OnRoutineUpdated;
        }

        private protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (_isDowned || _isAttack || IsInitialized == false)
                return;

            if (collision.gameObject.tag == UserUtils.Player)
            {
                if (_player == null)
                    _player = collision.gameObject.GetComponent<Player>();

                if (_player.CurrentSize <= Size)
                {
                    _player.TakeDamage(Damage);
                    _isAttack = true;
                    _routine.Wait(UserUtils.DamageRate);
                }
                else if (Size < _player.CurrentSize)
                {
                    DropDown();
                }
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _routine.Init(pauseHandler);
            _routine.Updated += OnRoutineUpdated;
            _mover.Init(pauseHandler);
            IsInitialized = true;
        }

        public void InitFromConfig(AnomalyConfig anomalyConfig)
        {
            transform.localScale = anomalyConfig.Scale;
            transform.position = anomalyConfig.StartPosition;
            SetSize(anomalyConfig.Size);
            _mover.InitFromConfig(anomalyConfig.PointMoverConfig);
        }

        public override void Pause()
        {
            base.Pause();
            _effect.Pause();
        }

        public override void Resume()
        {
            base.Resume();
            _effect.Play();
        }

        public override void DropDown()
        {
            base.DropDown();

            _isDowned = true;
            _mover.Stop();
            Collider.isTrigger = true;
            CollidersHolder.SetActive(true);
        }

        private void OnRoutineUpdated()
        {
            _isAttack = false;
            Collider.enabled = false;
            Collider.enabled = true;
        }
    }
}