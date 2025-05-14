using UnityEngine;

public struct ColorizedCubeData 
{
    public readonly Vector3 StartPosition;
    public readonly IReadonlyTemplateCube TemplateCube;
    public readonly Material Material;
    public readonly float Speed;
    public readonly float RotateSpeed;
    public readonly FlipType FlipType;

    public ColorizedCubeData(Vector3 startPosition, IReadonlyTemplateCube templateCube, 
        Material material, float speed, float rotateSpeed, FlipType flipType)
    {
        StartPosition = startPosition;
        TemplateCube = templateCube;
        Material = material;
        Speed = speed;
        RotateSpeed = rotateSpeed;
        FlipType = flipType;
    }
}