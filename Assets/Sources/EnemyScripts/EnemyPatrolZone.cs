using Assets.Sources.Utils;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class EnemyPatrolZone : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _enemiesCount;
        [SerializeField, Range(3, 25)] private int _maxLevel;
        [SerializeField, Range(3, 25)] private int _minLevel;

        private BoxCollider _collider;
        private Transform _transform;

        public event Action PlayerInZone;
        public event Action PlayerOutZone;

        public int EnemiesCount => _enemiesCount;
        public int MaxLevel => _maxLevel;
        public int MinLevel => _minLevel;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(UserUtils.Player))
                PlayerInZone?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(UserUtils.Player))
                PlayerOutZone?.Invoke();
        }

        public void Initialize()
        {
            _collider = GetComponent<BoxCollider>();
            _transform = transform;

            if (_minLevel > _maxLevel)
                _maxLevel = _minLevel;
        }

        public Vector3 GetRandomPointInZone()
        {
            if (_collider == null)
                return Vector3.zero;

            Vector3 center = _collider.center;
            Vector3 size = _collider.size;

            Vector3 worldCenter = _transform.TransformPoint(center);
            worldCenter.y = 0;

            Vector3 localRandomOffset = new Vector3
            (
                Random.Range(-size.x / 2f, size.x / 2f),
                0f,
                Random.Range(-size.z / 2f, size.z / 2f)
            );

            Vector3 worldOffset = _transform.TransformDirection(Vector3.Scale(localRandomOffset, _transform.lossyScale));

            return worldCenter + worldOffset;
        }
    }
}