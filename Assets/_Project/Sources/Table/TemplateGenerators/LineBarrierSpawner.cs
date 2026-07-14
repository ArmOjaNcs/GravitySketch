using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Table
{
    public class LineBarrierSpawner : MonoBehaviour
    {
        [SerializeField] private int _count = 12;
        [SerializeField] private GameObject _barrierPrefab;
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private bool _clearOldCubes = true;

#if UNITY_EDITOR
        [ContextMenu("Spawn Cubes In Line")]
        public void SpawnCubesInLine()
        {
            List<GameObject> childrenToDestroy = new List<GameObject>();
            float colliderBoundsX = 0;

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
                {
                    if (child != null)
                        DestroyImmediate(child.gameObject);
                }
            }

            for (int i = 0; i < _count; i++)
            {
                GameObject barrier = Instantiate(_barrierPrefab, _parentTransform);

                if (Mathf.Approximately(colliderBoundsX, 0))
                {
                    BoxCollider barrierCollider = barrier.GetComponent<BoxCollider>();
                    colliderBoundsX = barrierCollider.bounds.size.x;
                    Debug.Log($"collider bounds X = {barrierCollider.bounds.size.x}");
                }

                Vector3 position = Vector3.right * colliderBoundsX * i;
                barrier.transform.localPosition = position;
            }

            Debug.Log($"{_count} cubes spawned in line");
        }
#endif
    }
}