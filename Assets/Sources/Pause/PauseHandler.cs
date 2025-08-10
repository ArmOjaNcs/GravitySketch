using System;
using System.Collections.Generic;

namespace Assets.Sources.Pause
{
    public class PauseHandler 
    {
        private List<IPauseable> _pauseableObjects;

        public bool IsPaused {  get; private set; }

        public PauseHandler()
        {
            _pauseableObjects = new List<IPauseable>();
        }

        public void Register(IPauseable pauseable) => _pauseableObjects.Add(pauseable);

        public void Pause()
        {
            foreach (IPauseable pauseable in _pauseableObjects)
            {
                if(pauseable.IsActive())
                    pauseable.Pause();
            }

            IsPaused = true;
        }

        public void Resume()
        {
            foreach (IPauseable pauseable in _pauseableObjects)
            {
                if(pauseable.IsActive())
                    pauseable.Resume();
            }

            IsPaused = false;
        }
    }
}