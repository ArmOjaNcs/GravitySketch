using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyZone : PauseableObject
    {
        private protected SphereCollider Collider;

        public event Action PlayerIn;
        public event Action PlayerOut;

        public Player Player { get; protected set; }
        public bool PlayerIsDead { get; protected set; }

        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
            Collider.isTrigger = true;
        }

        private protected virtual void OnTriggerEnter(Collider other)
        {
            if (PlayerIsDead)
                return;

            if (other.CompareTag(UserUtils.Player))
                PlayerDetected(other);
        }

        private protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(UserUtils.Player))
                PlayerLosed(other);
        }

        public void Refresh()
        {
            Collider.enabled = false;
            Collider.enabled = true;
        }

        public override void Pause()
        {
            base.Pause();
            Collider.enabled = false;
        }

        public override void Resume()
        {
            base.Resume();
            Collider.enabled = true;
        }

        private protected virtual void PlayerDetected(Collider playerCollider)
        {
            if (Player == null)
            {
                if (playerCollider.TryGetComponent(out Player player))
                {
                    Player = player;
                    Player.IsDead += OnPlayerIsDead;
                }
            }

            PlayerIn?.Invoke();
        }

        private void OnPlayerIsDead()
        {
            Player.IsDead -= OnPlayerIsDead;
            PlayerIsDead = true;
        }

        private protected virtual void PlayerLosed(Collider playerCollider)
        {
            PlayerOut?.Invoke();
        }
    }
}