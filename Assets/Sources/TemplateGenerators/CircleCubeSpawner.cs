using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CircleCubeSpawner : MonoBehaviour
{
    [Header("Cube settings")]
    [SerializeField] private int _count = 12;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private List<Material> _materials;
    [SerializeField] private Barrier _barrierPrefab;
    [SerializeField] private Transform _parentTransform;

    [SerializeField] private bool _clearOldCubes = true;

#if UNITY_EDITOR
    [ContextMenu("Spawn Cubes In Circle")]
    public void SpawnCubesInCircle()
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

        float angleStep = 360f / _count;

        for (int i = 0; i < _count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _radius;
            Barrier barrier = Instantiate(_barrierPrefab);
            barrier.transform.SetParent(_parentTransform);
            barrier.transform.localPosition = position;
            barrier.transform.localRotation = Quaternion.identity;
            barrier.MeshRenderer.material = _materials[Random.Range(0, _materials.Count)];
        }

        Debug.Log($"{_count} cubes spawned by radius {_radius}.");
    }
#endif
}