using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        private bool _isFinished;
        private bool _isTutorial;

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
            _takeOverLimit.ObstacleDissolved += OnObstacleDissolved;
            _takeOverLimit.BarrierDissolved += OnBarrierDissolved; 
        }

        private void OnDisable()
        {
            _takeOverLimit.MedAidAbsorbed -= OnMedAidAbsorbed;
            _takeOverLimit.ObstacleDissolved -= OnObstacleDissolved;
            _takeOverLimit.BarrierDissolved -= OnBarrierDissolved;
        }

        public void Init(PauseHandler pauseHandler)
        {
            foreach (PauseableObject pauseableObject in _objects)
                pauseableObject.Init(pauseHandler);

            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = false;
        }

        public void SetFinished() => _isFinished = true;
        public void SetTutorial() => _isTutorial = true;

        public void TakeDamage(float damage)
        {
            if (_isFinished)
                return;

            if (damage <= 0 || _shield.IsDefended)
                return;

            float defencePercent = _shield.Defence / 10;
            defencePercent = Mathf.Clamp(defencePercent, 0, 0.75f);
            damage = damage - defencePercent * damage;
            damage = Mathf.Round(damage);
            _health.TakeDamage(damage);
            _audioPlayer.Play();
            Damaged?.Invoke();

            if (_health.CurrentValue == 0)
            {
                if (_isTutorial)
                {
                    _health.TakeHeal(50);
                    return;
                }

                Die();
                IsDead?.Invoke();
            }
        }

        private void Die()
        {
            foreach (PauseableObject pauseableObject in _objects)
                pauseableObject.gameObject.SetActive(false);

            _catcher.SetDie();
            enabled = false;
        }

        private void OnMedAidAbsorbed(float healPower)
        {
            _health.TakeHeal(healPower);
        }

        private void OnBarrierDissolved() => _health.TakeHeal(UserUtils.One);

        private void OnObstacleDissolved(int size)
        {
            float heal = size/2;
            Mathf.Round(heal);
            _health.TakeHeal(heal);
        }
    }
}