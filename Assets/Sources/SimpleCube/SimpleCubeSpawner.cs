using System.Collections.Generic;
using UnityEngine;

public class SimpleCubeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleCube _cubePrefab;
    [SerializeField] private TemplateMaterialReference _materialReference;
    [SerializeField] private List<SpawnArea> _spawnAreas = new List<SpawnArea>();

    [Header("Settings")]
    [SerializeField] private float _spacing = 1.5f;
    [SerializeField] private float _yPosition = 0f;

    private Queue<Material> _materialsQueue;
    private int _totalMaterials;

#if UNITY_EDITOR
    public void PrepareQueue()
    {
        ClearAllCubes();

        if (ValidateReferences() == false)
            return;

        _materialsQueue = new Queue<Material>(ShuffleMaterials(_materialReference.GetAllMaterials()));
        _totalMaterials = _materialsQueue.Count;

        Debug.Log($"Prepared queue with {_totalMaterials} materials");
    }

    public void SpawnCubes()
    {
        if (_materialsQueue == null || _materialsQueue.Count == 0)
        {
            Debug.LogError("No materials in queue! Prepare queue first.");
            return;
        }

        if (ValidateReferences() == false)
            return;

        DistributeCubes();
        LogSpawnResults();
    }

    private void DistributeCubes()
    {
        foreach (SpawnArea area in _spawnAreas)
        {
            if (_materialsQueue.Count == 0)
                break;

            if (area.transform.childCount >= area.Count)
                continue;

            SpawnInArea(area);
        }
    }

    private void SpawnInArea(SpawnArea area)
    {
        int cubesToSpawn = Mathf.Min(area.Count - area.transform.childCount, _materialsQueue.Count);
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(cubesToSpawn));

        for (int i = 0; i < cubesToSpawn; i++)
        {
            if (_materialsQueue.Count == 0)
                break;

            Vector3 newPosition = CalculatePosition(gridSize, i / gridSize, i % gridSize);
            SpawnCube(area.transform, newPosition);
        }
    }

    private void SpawnCube(Transform parent, Vector3 newPosition)
    {
        SimpleCube cube = Instantiate(_cubePrefab, parent);
        cube.transform.localPosition = newPosition;
        cube.SetMaterial(_materialsQueue.Dequeue());
    }

    private void LogSpawnResults()
    {
        if (_materialsQueue.Count == 0)
        {
            Debug.Log("All materials used!");
            return;
        }

        int remainingAreas = 0;

        foreach (SpawnArea area in _spawnAreas)
        {
            if (area.transform.childCount < area.Count)
                remainingAreas++;
        }

        if (remainingAreas > 0)
        {
            Debug.Log($"Materials remaining: {_materialsQueue.Count}\n" +
                     $"Add {remainingAreas} more SpawnAreas to use all materials");
        }
        else
        {
            Debug.Log($"Materials remaining: {_materialsQueue.Count}\n" +
                     "All SpawnAreas are full!");
        }
    }

    private Vector3 CalculatePosition(int gridSize, int row, int col)
    {
        float x = (col - (gridSize - 1) * 0.5f) * _spacing;
        float z = (row - (gridSize - 1) * 0.5f) * _spacing;

        return new Vector3(x, 0, z);
    }

    private List<Material> ShuffleMaterials(List<Material> materials)
    {
        for (int i = materials.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (materials[i], materials[j]) = (materials[j], materials[i]);
        }

        return materials;
    }

    private bool ValidateReferences()
    {
        if (_cubePrefab == null)
            Debug.LogError("Cube prefab missing!");

        if (_materialReference == null)
            Debug.LogError("Material reference missing!");

        if (_spawnAreas.Count == 0)
            Debug.LogError("No spawn areas!");

        return _cubePrefab && _materialReference && _spawnAreas.Count > 0;
    }

    private void ClearAllCubes()
    {
        List<GameObject> childrenToDestroy = new List<GameObject>();

        foreach (SpawnArea area in _spawnAreas)
        {
            if (area == null || area.transform == null)
                continue;

            foreach (Transform child in area.transform)
            {
                if (child != null && child.gameObject != null)
                    childrenToDestroy.Add(child.gameObject);
            }
        }

        foreach (GameObject child in childrenToDestroy)
        {
            if (child != null)
                DestroyImmediate(child.gameObject);
        }
    }
}
#endif