using Table;
using UnityEngine;

namespace ColorizerScripts
{
    public struct ColorizedCubeData
    {
        public readonly Vector3 StartPosition;
        public readonly IReadonlyTemplateCube TemplateCube;
        public readonly Color Color;
        public readonly float Speed;
        public readonly Vector3 RotateDirection;

        public ColorizedCubeData(
            Vector3 startPosition,
            IReadonlyTemplateCube templateCube,
            Color color,
            float speed,
            Vector3 rotateDirection)
        {
            StartPosition = startPosition;
            TemplateCube = templateCube;
            Color = color;
            Speed = speed;
            RotateDirection = rotateDirection;
        }
    }
}