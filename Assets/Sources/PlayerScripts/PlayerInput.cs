using Assets.Sources.Pause;
using Assets.Sources.Save;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class PlayerInput : PauseableObject
    {
        public event Action<Vector2> DirectionChanged;
        public event Action<bool> Boosted;
        public event Action Defended;
        public event Action<float> Rotated;

        private InputBindings _bindings;

        private void Awake()
        {
            _bindings = SaveSystem.LoadInputBindings();
        }

        private void Update()
        {
            if (IsPaused || !IsInitialized)
                return;

            Vector2 dir = Vector2.zero;

            if (Input.GetKey(_bindings.MoveUp)) dir.y += 1;

            if (Input.GetKey(_bindings.MoveDown)) dir.y -= 1;

            if (Input.GetKey(_bindings.MoveLeft)) dir.x -= 1;

            if (Input.GetKey(_bindings.MoveRight)) dir.x += 1;

            DirectionChanged?.Invoke(dir.normalized);

            Boosted?.Invoke(Input.GetKeyDown(_bindings.Boost));

            if (Input.GetKeyDown(_bindings.Defend))
                Defended?.Invoke();

            float rotation = 0;

            if (Input.GetKey(_bindings.RotateLeft)) rotation -= 1;

            if (Input.GetKey(_bindings.RotateRight)) rotation += 1;

            if (_bindings.UseMouseRotation)
                rotation += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f);

            Rotated?.Invoke(rotation);
        }

        public void StartInput() => IsInitialized = true;
    }
}