using System.Collections.Generic;
using UnityEngine;

public class LineCubeSpawner : MonoBehaviour
{
    [Header("Cube settings")]
    [SerializeField] private int _count = 12;
    [SerializeField] private List<Material> _materials;
    [SerializeField] private Barrier _barrierPrefab;
    [SerializeField] private Transform _parentTransform;

    [SerializeField] private bool _clearOldCubes = true;

#if UNITY_EDITOR
    [ContextMenu("Spawn Cubes In Line")]
    public void SpawnCubesInLine()
    {
        List<GameObject> childrenToDestroy = new List<GameObject>();

        if (_barrierPrefab == null)
        {
            Debug.LogWarning("Cube Prefab не назначен.");
            return;
        }

        if (_parentTransform == null)
            _parentTransform = transform;

        if (_clearOldCubes)
        {
            foreach (Transform child in _parentTransform)
                childrenToDestroy.Add(child.gameObject);

            foreach (GameObject child in childrenToDestroy)
                if (child != null)
                    DestroyImmediate(child.gameObject);
        }

        for (int i = 0; i < _count; i++)
        {
            Barrier barrier = Instantiate(_barrierPrefab, _parentTransform);
            Vector3 position = transform.position + Vector3.right * barrier.Collider.bounds.size.x * i;
            barrier.transform.localPosition = position;
            barrier.MeshRenderer.material = _materials[Random.Range(0, _materials.Count)];
        }

        Debug.Log($"{_count} cubes spawned in line");
    }
#endif
}
