using System;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Dissolvable;
using System.Collections.Generic;
using Assets.Sources.Pause;
using Assets.Sources.Audio;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Shield _shield;
        [SerializeField] private Catcher _catcher;
        [SerializeField] private Health _health;
        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private List<PauseableObject> _objects;
        [SerializeField] private AudioPlayer _audioPlayer;

        private Transform _transform;
        private Rigidbody _rigidbody;
        private SphereCollider _sphereCollider;

        public event Action IsDead;

        public Vector3 Position => _transform == null ? transform.position : _transform.position;
        public float Radius => _sphereCollider.radius * _transform.localScale.x;
        public float CurrentSize => _growHandler.CurrentSize;
        public bool IsDefended => _shield.IsDefended;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _sphereCollider = GetComponent<SphereCollider>();
            _health.Initialize(UserUtils.PlayerStartHealth);
        }

        private void OnEnable()
        {
            _takeOverLimit.MedAidAbsorbed += OnMedAidAbsorbed;
        }

        private void OnDisable()
        {
            _takeOverLimit.MedAidAbsorbed -= OnMedAidAbsorbed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(UserUtils.DissolvableObstacle))
            {
                DissolvableObstacle obstacle = collision.gameObject.GetComponentInParent<DissolvableObstacle>();

                if (obstacle != null)
                {
                    if (_growHandler.CurrentSize > obstacle.Size)
                    {
                        obstacle.DropDown();
                        _catcher.RefreshSensor();
                    }
                }
            }
        }

        public void Init(PauseHandler pauseHandler)
        {
            foreach (PauseableObject pauseableObject in _objects)
                pauseableObject.Init(pauseHandler);

            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = false;
        }

        public void TakeDamage(float damage, Vector3 forcePosition, float force)
        {
            if (damage <= 0 || _shield.IsDefended)
                return;

            Vector3 forceVector = (Position - forcePosition).normalized;
            forceVector.y = 0;
            _health.TakeDamage(damage);
            _audioPlayer.Play();

            if (_health.CurrentValue == 0)
            {
                _catcher.SetDie();
                IsDead?.Invoke();
                gameObject.SetActive(false);
                return;
            }

            _rigidbody.AddForceAtPosition(forceVector * force, forcePosition, ForceMode.Impulse);
        }

        private void OnMedAidAbsorbed(float healPower)
        {
            _health.TakeHeal(healPower);
        }
    }
}