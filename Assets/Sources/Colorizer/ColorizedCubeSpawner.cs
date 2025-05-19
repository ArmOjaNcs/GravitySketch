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
        SpawnZone spawnZone = GetRandomSpawnZone();
        Vector3 position = GetRandomPointInZone(spawnZone);
        Vector3 rotateDirection = UserUtils.GetRandomRotateDirection();
        colorizedCube.Init();
        colorizedCube.SetStartSettings(new ColorizedCubeData(position, cube, material, 
            speed, rotateDirection));
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
        Vector3 size = spawnZone.GetBoxCollider().size;

        Vector3 randomPoint = new Vector3
        (
            Random.Range(-size.x / 2f, size.x / 2f),
            Random.Range(-size.y / 2f, size.y / 2f),
            Random.Range(-size.z / 2f, size.z / 2f)
        );

        return spawnZone.GetPoint(randomPoint);
    }
}