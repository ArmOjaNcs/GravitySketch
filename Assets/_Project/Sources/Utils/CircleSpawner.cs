using UnityEngine;

namespace Utils
{
    [ExecuteInEditMode]
    public class CircleSpawner : MonoBehaviour
    {
        [Header("Настройки круга")]
        [SerializeField]
        [Min(0.1f)] private float _radius = 5f;
        [SerializeField]
        [Min(0.1f)] private float _spacing = 1f;

        [Header("Настройки объекта")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private bool _clearOldObjects = true;

        [ContextMenu("Spawn In Circle")]
        private void Spawn()
        {
            if (_prefab == null)
            {
                Debug.LogWarning("Prefab не назначен!");
                return;
            }

            if (_parentTransform == null)
            {
                Debug.LogWarning("ParentTransform не назначен!");
                return;
            }

            if (_clearOldObjects)
            {
                ClearChildren();
            }

            float step = _spacing;
            for (float r = 0; r <= _radius; r += step)
            {
                float circumference = 2 * Mathf.PI * r;
                int count = Mathf.Max(1, Mathf.FloorToInt(circumference / _spacing));

                for (int i = 0; i < count; i++)
                {
                    float angle = i * Mathf.PI * 2 / count;
                    Vector3 localPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * r;
                    Vector3 worldPos = _parentTransform.position + localPos;

                    GameObject instance = Instantiate(_prefab, worldPos, Quaternion.identity);
                    instance.transform.SetParent(_parentTransform, true);
                }
            }
        }

        [ContextMenu("Clear Children")]
        private void ClearChildren()
        {
            if (_parentTransform == null)
                return;

            for (int i = _parentTransform.childCount - 1; i >= 0; i--)
            {
                Transform child = _parentTransform.GetChild(i);
#if UNITY_EDITOR
                DestroyImmediate(child.gameObject);
#else
            Destroy(child.gameObject);
#endif
            }
        }
    }
}