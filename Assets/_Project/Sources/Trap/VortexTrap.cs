using System;
using Audio;
using EnemyScripts.EnemyZones;
using Pause;
using Utils;
using UnityEngine;

namespace Trap
{
    public class VortexTrap : PauseableObject
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _inactiveDuration;
        [SerializeField] private float _activeDuration;
        [SerializeField] private EnemyZone _zone;
        [SerializeField] private AudioClip _activeSound;
        [SerializeField] private WaitRoutine _attackRoutine;
        [SerializeField] private WaitRoutine _lifeRoutine;
        [SerializeField] private TrapGrowUpAnimation _growUpAnimation;
        [SerializeField] private TrapGrowDownAnimation _growDownAnimation;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private ParticleSystem _vortexEffect;

        private Transform _transform;
        private AudioPlayerSpawner _audioPlayerSpawner;
        private Action _currentSubscription;
        private AudioPlayer _activePlayer;
        private bool _isPlayerInZone;
        private bool _isActive;

        private void OnEnable()
        {
            _zone.PlayerIn += OnPlayerIn;
            _zone.PlayerOut += OnPlayerOut;
            _attackRoutine.Updated += OnAttackRoutineUpdated;
        }

        private void OnDisable()
        {
            _zone.PlayerIn -= OnPlayerIn;
            _zone.PlayerOut -= OnPlayerOut;
            _attackRoutine.Updated -= OnAttackRoutineUpdated;
            UnsubscribeOnCurrentAction();
        }

        public void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            Init(pauseHandler);
            _zone.Init(pauseHandler);
            _attackRoutine.Init(pauseHandler);
            _attackRoutine.Wait(UserUtils.DamageRate);
            _lifeRoutine.Init(pauseHandler);
            _currentSubscription = OnInactiveRoutineUpdated;
            SubscribeOnCurrentAction();
            _lifeRoutine.Wait(_inactiveDuration);
            _growUpAnimation.Init(pauseHandler);
            _growDownAnimation.Init(pauseHandler);
            _audioPlayerSpawner = audioPlayerSpawner;
            _transform = transform;
        }

        public override void Pause()
        {
            base.Pause();
            _effect.Pause();
            _vortexEffect.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (_isActive)
                _effect.Play();

            _vortexEffect.Play();
        }

        private void OnPlayerIn() => _isPlayerInZone = true;

        private void OnPlayerOut() => _isPlayerInZone = false;

        private void OnAttackRoutineUpdated()
        {
            Attack();
            _attackRoutine.Wait(UserUtils.DamageRate);
        }

        private void OnGrowUpRoutineUpdated()
        {
            UnsubscribeOnCurrentAction();
            _currentSubscription = OnActiveRoutineUpdated;
            SubscribeOnCurrentAction();
            _lifeRoutine.Wait(_activeDuration);
        }

        private void OnActiveRoutineUpdated()
        {
            UnsubscribeOnCurrentAction();
            _currentSubscription = OnGrowDownRoutineUpdated;
            SubscribeOnCurrentAction();
            _lifeRoutine.Wait(UserUtils.GrowDuration);
            _growDownAnimation.Play();
        }

        private void OnGrowDownRoutineUpdated()
        {
            UnsubscribeOnCurrentAction();
            _currentSubscription = OnInactiveRoutineUpdated;
            SubscribeOnCurrentAction();
            _lifeRoutine.Wait(_inactiveDuration);
            _activePlayer?.Stop();
            _isActive = false;
            _effect.Pause();
        }

        private void OnInactiveRoutineUpdated()
        {
            UnsubscribeOnCurrentAction();
            _currentSubscription = OnGrowUpRoutineUpdated;
            SubscribeOnCurrentAction();
            _lifeRoutine.Wait(UserUtils.GrowDuration);
            _activePlayer = GetAudioPlayer();
            _activePlayer?.SetAudioClip(_activeSound)?.Play();
            _isActive = true;
            _effect.Play();
            _growUpAnimation.Play();
        }

        private void Attack()
        {
            if (_isActive == false)
                return;

            if (_isPlayerInZone && _zone.Player.isActiveAndEnabled)
                _zone.Player.TakeDamage(_damage);
        }

        private AudioPlayer GetAudioPlayer() => _audioPlayerSpawner.GetAudioPlayerOnPosition(_transform.position);

        private void SubscribeOnCurrentAction() => _lifeRoutine.Updated += _currentSubscription;

        private void UnsubscribeOnCurrentAction() => _lifeRoutine.Updated -= _currentSubscription;
    }
}