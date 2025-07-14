using System;
using System.Collections;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Dissolvable;
using Assets.Sources.PlayerScripts;

namespace Assets.Sources.AnomalyScpipts
{
    [RequireComponent(typeof(SphereCollider))]
    public class Anomaly : DissolvableObstacle
    {
        private const float Force = 500;

        [SerializeField] private float _damageRate;
        [SerializeField] private GameObject _collidersHolder;

        private Player _player;
        private Rigidbody _playerRigidbody;
        private Coroutine _coroutine;
        private WaitForSeconds _rateTime;
        private SphereCollider _collider;
        private bool _isDowned;

        public event Action IsDowned;

        private int Damage => Size;

        private protected override void Awake()
        {
            base.Awake();

            _collider = GetComponent<SphereCollider>();
            _rateTime = new WaitForSeconds(_damageRate);
            _collidersHolder.SetActive(false);
        }

        private void FixedUpdate()
        {
            _collider.enabled = false;
            _collider.enabled = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isDowned)
                return;

            if (collision.gameObject.tag == UserUtils.Player)
            {
                if (_player == null)
                    _player = collision.gameObject.GetComponent<Player>();

                if (_playerRigidbody == null)
                    _playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

                if (_coroutine == null)
                    _coroutine = StartCoroutine(AttackPlayerRoutine());
            }
        }

        public override void DropDown()
        {
            base.DropDown();

            _isDowned = true;
            IsDowned?.Invoke();
            _collider.isTrigger = true;
            _collidersHolder.SetActive(true);
        }

        private IEnumerator AttackPlayerRoutine()
        {
            _player.TakeDamage(Damage, transform.position, Force);

            yield return _rateTime;

            _coroutine = null;
        }
    }
}