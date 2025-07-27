using System;
using UnityEngine;

namespace Assets.Sources.Pause
{
    public class PauseInput : MonoBehaviour
    {
        private bool IsPaused => Input.GetKeyUp(KeyCode.Escape);

        public event Action Paused;

        private void Update()
        {
            if(IsPaused)
                Paused?.Invoke();
        }
    }
}