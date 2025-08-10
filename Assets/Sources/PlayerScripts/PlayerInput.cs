using System;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Pause;

namespace Assets.Sources.PlayerScripts
{
    public class PlayerInput : PauseableObject
    {
        public event Action<Vector2> DirectionChanged;
        public event Action<bool> Boosted;
        public event Action Defended;
        public event Action<float> Rotated;

        private float HorizontalInput => Input.GetAxis(UserUtils.Horizontal);
        private float VerticalInput => Input.GetAxis(UserUtils.Vertical);
        private bool IsBoosted => Input.GetKeyDown(KeyCode.Mouse1);
        private bool IsDefended => Input.GetKeyDown(KeyCode.Mouse0);

        private void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            DirectionChanged?.Invoke(new Vector2(HorizontalInput, VerticalInput));
            Boosted?.Invoke(IsBoosted);

            if (IsDefended)
                Defended?.Invoke();

            Rotated?.Invoke(Input.GetAxis("Mouse X"));
        }

        public void StartInput() => IsInitialized = true;
    }
}