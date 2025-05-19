using UnityEngine;

public struct ColorizedCubeData 
{
    public readonly Vector3 StartPosition;
    public readonly IReadonlyTemplateCube TemplateCube;
    public readonly Material Material;
    public readonly float Speed;
    public readonly Vector3 RotateDirection;

    public ColorizedCubeData(Vector3 startPosition, IReadonlyTemplateCube templateCube, 
        Material material, float speed, Vector3 rotateDirection)
    {
        StartPosition = startPosition;
        TemplateCube = templateCube;
        Material = material;
        Speed = speed;
        RotateDirection = rotateDirection;
    }
}