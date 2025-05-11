using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class Colorizer : MonoBehaviour
{
    [SerializeField] private ColorizedCube _cubePrefab;
    [SerializeField] private int _maxCapacity;
    [SerializeField] private ColoringPositionHandler _coloringPositionHandler;

    [Header("CubeSettings")]
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private BoxCollider _boxCollider;
    private ObjectPool<ColorizedCube> _pool;

    public event Action<int> IndexApplied;

    private void Awake()
    {
        _pool = new ObjectPool<ColorizedCube>(_cubePrefab, _maxCapacity, transform);
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _coloringPositionHandler.PaintApplied += OnPaintApplied;
    }

    private void OnDisable()
    {
        _coloringPositionHandler.PaintApplied -= OnPaintApplied;
    }

    private void OnPaintApplied(TemplateCube templateCube, Material material)
    {
        SendCube(templateCube, material);
    }

    private void SendCube(TemplateCube templateCube, Material material)
    {
        ColorizedCube cube = _pool.GetElement();
        float speed = Random.Range(_minSpeed, _maxSpeed);
        Vector3 position = GetRandomPointInsideBox();
        position.z = transform.position.z;
        cube.Init();
        cube.SetStartSettings(position, templateCube, material, speed);
        cube.Finished += OnCubeFinished;
        cube.StartMove();
    }

    private void OnCubeFinished(ColorizedCube cube)
    {
        cube.Finished -= OnCubeFinished;
        IndexApplied?.Invoke(cube.GetTargetIndex());
        cube.gameObject.SetActive(false);
    }

    private Vector3 GetRandomPointInsideBox()
    {
        Vector2 size = _boxCollider.size;

        Vector3 randomPoint = new Vector3
        (
            Random.Range(-size.x / 2f, size.x / 2f),
            Random.Range(-size.y / 2f, size.y / 2f)
        );

        return _boxCollider.transform.TransformPoint(randomPoint);
    }
}