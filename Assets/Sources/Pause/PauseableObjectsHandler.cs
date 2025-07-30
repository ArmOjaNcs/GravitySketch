using System.Collections.Generic;

namespace Assets.Sources.Pause
{
    public static class PauseableObjectsHandler
    {
        private static List<IPauseable> _pauseableObjects = new List<IPauseable>();

        public static bool IsPaused {  get; private set; }

        public static void Register(IPauseable pauseable) => _pauseableObjects.Add(pauseable);

        public static void Pause()
        {
            foreach (IPauseable pauseable in _pauseableObjects)
            {
                if(pauseable.IsActive())
                    pauseable.Pause();
            }

            IsPaused = true;
        }

        public static void Resume()
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