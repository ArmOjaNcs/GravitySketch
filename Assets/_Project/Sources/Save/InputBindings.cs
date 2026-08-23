using System;
using UnityEngine;
using Utils;

namespace Save
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
        public bool UseJoystick = true;

        public static InputBindings GetDefault()
        {
            return new InputBindings();
        }

        public InputBindings Clone()
        {
            return (InputBindings)MemberwiseClone();
        }

        public KeyCode GetKeyCode(ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.MoveUp: return MoveUp;
                case ButtonType.MoveDown: return MoveDown;
                case ButtonType.MoveLeft: return MoveLeft;
                case ButtonType.MoveRight: return MoveRight;
                case ButtonType.Boost: return Boost;
                case ButtonType.Defend: return Defend;
                case ButtonType.RotateLeft: return RotateLeft;
                case ButtonType.RotateRight: return RotateRight;
                case ButtonType.Paint: return Paint;
                case ButtonType.ResetCube: return ResetCube;
                default: return KeyCode.None;
            }
        }

        public void SetKey(ButtonType buttonType, KeyCode key)
        {
            switch (buttonType)
            {
                case ButtonType.MoveUp: MoveUp = key; break;
                case ButtonType.MoveDown: MoveDown = key; break;
                case ButtonType.MoveLeft: MoveLeft = key; break;
                case ButtonType.MoveRight: MoveRight = key; break;
                case ButtonType.Boost: Boost = key; break;
                case ButtonType.Defend: Defend = key; break;
                case ButtonType.RotateLeft: RotateLeft = key; break;
                case ButtonType.RotateRight: RotateRight = key; break;
                case ButtonType.Paint: Paint = key; break;
                case ButtonType.ResetCube: ResetCube = key; break;
            }
        }
    }
}