using Assets.Sources.Pause;
using Assets.Sources.Save;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.PlayerScripts
{
    public class PlayerInput : PauseableObject, IDisposable
    {
        public event Action<Vector2> DirectionChanged;
        public event Action Boosted;
        public event Action Defended;
        public event Action<float> Rotated;

        private InputBindings _bindings;
        private FixedJoystick _moveJoystick;
        private FixedJoystick _rotateJoystick;
        private Button _shieldAbility;
        private Button _boosterAbility;
        private bool _subscribed;

        private bool IsJoystickMode => _bindings == null ? false : _bindings.UseJoystick;

        private void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            if (IsJoystickMode)
                HandleJoystickInput();
            else
                HandleKeyboardMouseInput();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void InitBindings(InputBindings inputBindings, FixedJoystick moveJoystick,
            FixedJoystick rotateJoystick, Button shieldAbility, Button boosterAbility)
        {
            _bindings = inputBindings;
            _moveJoystick = moveJoystick;
            _rotateJoystick = rotateJoystick;
            _shieldAbility = shieldAbility;
            _boosterAbility = boosterAbility;

            if (_subscribed)
                return;

            _shieldAbility.onClick.AddListener(OnShieldClick);
            _boosterAbility.onClick.AddListener(OnBoostClick);
            _subscribed = true;
        }

        public void Dispose()
        {
            if (_subscribed == false)
                return;

            _shieldAbility.onClick.RemoveListener(OnShieldClick);
            _boosterAbility.onClick.RemoveListener(OnBoostClick);
        }

        public void StartInput() => IsInitialized = true;

        public void StopInput()
        {
            IsInitialized = false;
            DirectionChanged?.Invoke(Vector2.zero);
            Rotated?.Invoke(0);
        }

        private void HandleKeyboardMouseInput()
        {
            Vector2 dir = Vector2.zero;

            if (Input.GetKey(_bindings.MoveUp)) dir.y += 1;
            if (Input.GetKey(_bindings.MoveDown)) dir.y -= 1;
            if (Input.GetKey(_bindings.MoveLeft)) dir.x -= 1;
            if (Input.GetKey(_bindings.MoveRight)) dir.x += 1;

            DirectionChanged?.Invoke(dir.normalized);

            if (Input.GetKeyDown(_bindings.Boost))
                Boosted?.Invoke();

            if (Input.GetKeyDown(_bindings.Defend))
                Defended?.Invoke();

            float rotation = 0;

            if (Input.GetKey(_bindings.RotateLeft)) rotation -= 1;
            if (Input.GetKey(_bindings.RotateRight)) rotation += 1;

            if (_bindings.UseMouseRotation)
                rotation += Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f);

            Rotated?.Invoke(rotation);
        }

        private void HandleJoystickInput()
        {
            Vector2 dir = new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical);
            DirectionChanged?.Invoke(dir);
            float rotation = _rotateJoystick.Horizontal;
            Rotated?.Invoke(rotation);
        }

        private void OnShieldClick()
        {
            if (IsPaused || IsInitialized == false)
                return;

            Defended?.Invoke();
        }

        private void OnBoostClick()
        {
            if (IsPaused || IsInitialized == false)
                return;

            Boosted?.Invoke();
        }
    }
}