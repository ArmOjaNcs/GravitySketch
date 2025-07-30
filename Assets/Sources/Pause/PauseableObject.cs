using UnityEngine;

namespace Assets.Sources.Pause
{
    public abstract class PauseableObject : MonoBehaviour, IPauseable
    {
        private protected bool IsPaused;

        private protected virtual void Awake()
        {
            PauseableObjectsHandler.Register(this);
        }

        public virtual void Pause()
        {
            if (IsPaused == false)
                IsPaused = true;
        }

        public virtual void Resume()
        {
            if (IsPaused)
                IsPaused = false;
        }

        public bool IsActive() => gameObject.activeInHierarchy;
    }
}