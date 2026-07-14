using UnityEngine;

namespace Assets.Sources.Pause
{
    public abstract class PauseableObject : MonoBehaviour, IPauseable
    {
        private protected bool IsPaused;

        public bool IsInitialized { get; protected set; }

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

        public virtual void Init(PauseHandler pauseHandler)
        {
            pauseHandler.Register(this);
        }
    }
}