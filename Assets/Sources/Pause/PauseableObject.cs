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
            if (IsPaused || isActiveAndEnabled == false)
                return;

            IsPaused = true;
        }

        public virtual void Resume()
        {
            if (IsPaused == false || isActiveAndEnabled == false)
                return;

            IsPaused = false;
        }
    }
}