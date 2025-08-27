using Assets.Sources.Dissolvable;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.AnomalyScpipts
{
    [RequireComponent(typeof(SphereCollider))]
    public class Anomaly : DissolvableObstacle
    {
        private const float Force = 200;

        [SerializeField] private float _damageRate;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private PauseableRoutine _routine;

        private Player _player;
        private Rigidbody _playerRigidbody;
        private bool _isAttack;
        private bool _isDowned;

        public event Action IsDowned;

        private int Damage => Size;

        private protected override void Awake()
        {
            base.Awake();
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

                if (_playerRigidbody == null)
                    _playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

                if (_player.CurrentSize <= Size && _player.IsDefended == false)
                {
                    _player.TakeDamage(Damage, transform.position, Force);
                    _isAttack = true;
                    _routine.UpdateView(_damageRate);
                }
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _routine.Init(pauseHandler);
            _routine.Updated += OnRoutineUpdated;
            IsInitialized = true;
        }

        private void OnRoutineUpdated()
        {
            _isAttack = false;
            Collider.enabled = false;
            Collider.enabled = true;
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
            IsDowned?.Invoke();
            Collider.isTrigger = true;
            CollidersHolder.SetActive(true);
        }
    }
}