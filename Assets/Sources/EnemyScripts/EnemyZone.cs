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

        private void OnDisable()
        {
            if(Player != null)
            {
                Player.IsDead -= OnPlayerIsDead;
                Player.IsRevived -= OnPlayerRevived;
            }
        }

        private protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(UserUtils.Player))
            {
                if (Player == null)
                {
                    if (other.TryGetComponent(out Player player))
                    {
                        Player = player;
                        Player.IsDead += OnPlayerIsDead;
                        Player.IsRevived += OnPlayerRevived;
                    }
                }

                if(Player.Dead == false)
                    PlayerDetected();
            }
        }

        private protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(UserUtils.Player))
                PlayerLosed();
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

        private void OnPlayerIsDead()
        {
            PlayerLosed();
        }

        private void OnPlayerRevived()
        {
            Refresh();
        }

        private protected virtual void PlayerDetected()
        {
            PlayerIn?.Invoke();
        }

        private protected virtual void PlayerLosed()
        {
            PlayerOut?.Invoke();
        }
    }
}