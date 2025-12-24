using UnityEngine;

namespace Assets.Sources.Level
{
    public class PrefabPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 _position;

        private void OnEnable()
        {
            transform.position = _position;
        }
    }
}