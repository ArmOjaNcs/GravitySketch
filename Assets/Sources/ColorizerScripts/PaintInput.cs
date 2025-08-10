using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.ColorizerScripts
{
    public class PaintInput : PauseableObject
    {
        private bool _isStarted;

        public event Action<bool> Coloring;

        private bool IsColoring => Input.GetMouseButton(0);

        private void Update()
        {
            if (IsPaused || IsInitialized == false || _isStarted == false)
                return;

            Coloring?.Invoke(IsColoring);
        }

        public void StartInput() => _isStarted = true;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            IsInitialized = true;
        }
    }
}