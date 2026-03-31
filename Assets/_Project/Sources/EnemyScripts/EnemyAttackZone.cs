using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class EnemyAttackZone : EnemyZone, IEnemyAttack
    {
        private AudioPlayerSpawner _audioPlayerSpawner;

        private protected PauseHandler PauseHandler;
        private protected AudioClip AudioClip;
        private protected AudioPlayer AudioPlayer;
        private protected Transform FirePoint;
        private protected float CurrentTime;
        private protected float AttackRate;
        private protected bool IsAttacking;

        private protected virtual void Update()
        {
            if (IsActivated() == false)
                return;

            if (Player.gameObject.activeSelf == false)
                return;

            CurrentTime += Time.deltaTime;

            if (CurrentTime > AttackRate)
                Attack();
        }

        public virtual void InitFromConfig(EnemyAttackConfig config, Transform firePoint, 
            AudioPlayerSpawner audioPlayerSpawner, PauseHandler pauseHandler)
        {
            FirePoint = firePoint;
            AttackRate = config.AttackRate;
            _audioPlayerSpawner = audioPlayerSpawner;
            Init(pauseHandler);
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            SetAudioClip();
            PauseHandler = pauseHandler;
        }

        public virtual void Return(GameObject gameObject) => gameObject.SetActive(false);

        public AudioPlayer GetAudioPlayer(Vector3 position)
        {
            return _audioPlayerSpawner.GetAudioPlayer(position);
        }

        private protected override void PlayerDetected()
        {
            base.PlayerDetected();
            IsAttacking = true;
        }

        private protected virtual void Attack()
        {
            if (Player == null)
                return;

            AudioPlayer = GetAudioPlayer(FirePoint.position);
            AudioPlayer?.SetAudioClip(AudioClip);
            CurrentTime = 0;
        }

        private protected bool IsActivated()
        {
            return IsInitialized && IsAttacking && IsPaused == false && Player != null;
        }
       
        private protected override void PlayerLosed()
        {
            IsAttacking = false;
            CurrentTime = 0;
        }

        private protected abstract void SetAudioClip();
    }
}