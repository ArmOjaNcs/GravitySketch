using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class EnemyZone : PauseableObject
    {
        private protected bool IsInitialized;
        private protected SphereCollider Collider;

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

        private protected abstract void PlayerDetected(Collider playerCollider);
        private protected abstract void PlayerLosed(Collider playerCollider);
    }
}