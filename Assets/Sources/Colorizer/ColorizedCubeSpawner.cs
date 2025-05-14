using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorizedCubeSpawner : MonoBehaviour
{
    [SerializeField] private ColorizedCube _cubePrefab;
    [SerializeField] private int _maxCapacity;
    [SerializeField] private Colorizer _colorizer;
    [SerializeField] private SpawnZone[] _spawnZones;

    [Header("CubeSettings")]
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _minRotateSpeed;
    [SerializeField] private float _maxRotateSpeed;

    private ObjectPool<ColorizedCube> _pool;

    public event Action<int> IndexApplied;

    private void Awake()
    {
        if (_spawnZones.Length == 0)
            Debug.Log("Spawnzones are not set");

        _pool = new ObjectPool<ColorizedCube>(_cubePrefab, _maxCapacity, transform);
    }

    private void OnEnable()
    {
        _colorizer.PaintApplied += OnPaintApplied;
    }

    private void OnDisable()
    {
        _colorizer.PaintApplied -= OnPaintApplied;
    }

    private void OnPaintApplied(IReadonlyTemplateCube templateCube, Material material)
    {
        SendCube(templateCube, material);
    }

    private void SendCube(IReadonlyTemplateCube cube, Material material)
    {
        ColorizedCube colorizedCube = _pool.GetElement();
        float speed = Random.Range(_minSpeed, _maxSpeed);
        float rotateSpeed = Random.Range(_minRotateSpeed, _maxRotateSpeed);
        SpawnZone spawnZone = GetRandomSpawnZone();
        Vector3 position = GetRandomPointInZone(spawnZone);
        colorizedCube.Init();
        colorizedCube.SetStartSettings(new ColorizedCubeData(position, cube, material, speed, 
            rotateSpeed, UserUtils.GetRandomFlipType()));
        colorizedCube.Finished += OnCubeFinished;
        colorizedCube.StartMove();
    }

    private void OnCubeFinished(ColorizedCube cube)
    {
        cube.Finished -= OnCubeFinished;
        IndexApplied?.Invoke(cube.GetTargetIndex());
        cube.gameObject.SetActive(false);
    }

    private SpawnZone GetRandomSpawnZone()
    {
        int index = Random.Range(0, _spawnZones.Length);
        return _spawnZones[index];
    }

    public Vector3 GetRandomPointInZone(SpawnZone spawnZone)
    {
        Vector2 size = spawnZone.GetBoxCollider().size;

        Vector3 randomPoint = new Vector3
        (
            Random.Range(-size.x / 2f, size.x / 2f),
            Random.Range(-size.y / 2f, size.y / 2f)
        );

        return spawnZone.GetPoint(randomPoint);
    }
}