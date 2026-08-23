using System.Collections.Generic;
using UnityEngine;

namespace Pause
{
    public class PauseHandler
    {
        private List<IPauseable> _pauseableObjects;

        public PauseHandler()
        {
            _pauseableObjects = new List<IPauseable>();
        }

        public bool IsPaused { get; private set; }

        public void Register(IPauseable pauseable) => _pauseableObjects.Add(pauseable);

        public void Pause()
        {
            foreach (IPauseable pauseable in _pauseableObjects)
            {
                if (pauseable.IsActive())
                    pauseable.Pause();
            }

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            IsPaused = true;
        }

        public void Resume()
        {
            foreach (IPauseable pauseable in _pauseableObjects)
            {
                if (pauseable.IsActive())
                    pauseable.Resume();
            }

            IsPaused = false;
        }
    }
}