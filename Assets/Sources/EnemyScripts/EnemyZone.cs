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
        [SerializeField] private protected bool IsInitialized;
        private protected SphereCollider Collider;

        public event Action PlayerIn;
        public event Action PlayerOut;

        public Player Player { get; protected set; }

        private protected override void Awake()
        {
            base.Awake();
            Collider = GetComponent<SphereCollider>();
            Collider.isTrigger = true;
        }

        private protected virtual void OnTriggerEnter(Collider other)
        {
            if (IsInitialized == false)
                return;

            if (other.CompareTag(UserUtils.Player))
                PlayerDetected(other);
        }

        private protected virtual void OnTriggerExit(Collider other)
        {
            if (IsInitialized == false)
                return;

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
                if (playerCollider.TryGetComponent(out Player player))
                    Player = player;

            PlayerIn?.Invoke();
        }

        private protected virtual void PlayerLosed(Collider playerCollider)
        {
            PlayerOut?.Invoke();
        }
    }
}