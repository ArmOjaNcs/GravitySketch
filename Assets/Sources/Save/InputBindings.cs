using System;
using UnityEngine;

namespace Assets.Sources.Save
{
    [Serializable]
    public class InputBindings
    {
        public KeyCode MoveUp = KeyCode.W;
        public KeyCode MoveDown = KeyCode.S;
        public KeyCode MoveLeft = KeyCode.A;
        public KeyCode MoveRight = KeyCode.D;
        public KeyCode Boost = KeyCode.Mouse1;
        public KeyCode Defend = KeyCode.Mouse0;
        public KeyCode RotateLeft = KeyCode.Q;
        public KeyCode RotateRight = KeyCode.E;
        public KeyCode Paint = KeyCode.Mouse0;
        public KeyCode ResetCube = KeyCode.Mouse1;

        public bool UseMouseRotation = true;
        public bool UseJoystick = false;

        public InputBindings Clone()
        {
            return (InputBindings)MemberwiseClone();
        }

        public static InputBindings GetDefault()
        {
            return new InputBindings();
        }
    }
}