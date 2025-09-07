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
        private SphereCollider _sphereCollider;

        public event Action IsDead;
        public event Action Damaged;

        public Vector3 Position => _transform == null ? transform.position : _transform.position;
        public float Radius => _sphereCollider.radius * _transform.localScale.x;
        public float CurrentSize => _growHandler.CurrentSize;
        public bool IsDefended => _shield.IsDefended;

        private void Awake()
        {
            _transform = transform;
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

        public void Init(PauseHandler pauseHandler)
        {
            foreach (PauseableObject pauseableObject in _objects)
                pauseableObject.Init(pauseHandler);

            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = false;
        }

        public void TakeDamage(float damage)
        {
            if (damage <= 0 || _shield.IsDefended)
                return;

            _health.TakeDamage(damage);
            _audioPlayer.Play();
            Damaged?.Invoke();

            if (_health.CurrentValue == 0)
            {
                _catcher.SetDie();
                IsDead?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void OnMedAidAbsorbed(float healPower)
        {
            _health.TakeHeal(healPower);
        }
    }
}