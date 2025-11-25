using Assets.Sources.Dissolvable;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Level;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class TakeOverLimit : MonoBehaviour
    {
        [SerializeField] private Transform _hole;

        private readonly HashSet<SimpleCube> _absorbed = new HashSet<SimpleCube>();
        private BoxCollider _boxCollider;
        private Vector3 _localSize;
        private Transform _transform;

        private int _mask;

        public event Action<SimpleCube> CubeAbsorbed;
        public event Action<int> Rewarded;
        public event Action EnemyDissolved;
        public event Action<float> MedAidAbsorbed;

        public int EnemiesDissolvedCount { get; private set; }

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _transform = transform;

            _localSize = _boxCollider.size;
            _mask = (1 << UserUtils.NormalLayer) | (1 << UserUtils.FallingLayer);
        }

        private void FixedUpdate()
        {
            //ScanBox();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SimpleCube simpleCube))
            {
                CubeAbsorbed?.Invoke(simpleCube);
            }
            else if (other.TryGetComponent(out Enemy enemy))
            {
                if (enemy.IsDissolving == false)
                {
                    enemy.Dissolve(_hole);
                    Rewarded?.Invoke(enemy.Reward);
                    EnemiesDissolvedCount++;
                    EnemyDissolved?.Invoke();
                }
            }
            else if (other.TryGetComponent(out MedAid medAid))
            {
                if (medAid.IsDissolving == false)
                {
                    MedAidAbsorbed?.Invoke(medAid.HealPower);
                    Rewarded?.Invoke(medAid.Reward);
                    medAid.Dissolve(_hole);
                }
            }
            else if (other.gameObject.CompareTag(UserUtils.Dropped))
            {
                DissolvableObstacle dissolvableObstacle = other.transform.GetComponentInParent<DissolvableObstacle>();

                if (dissolvableObstacle != null && dissolvableObstacle.IsDissolving == false)
                {
                    dissolvableObstacle.Dissolve(_hole);
                    Rewarded?.Invoke(dissolvableObstacle.Reward);
                }
            }
            else if (other.gameObject.TryGetComponent(out LevelExit levelExit))
            {
                if (levelExit.IsDissolving == false)
                    levelExit.Dissolve(_hole);
            }
        }

        private void ScanBox()
        {
            GetWorldBox(out Vector3 center, out Vector3 half, out Quaternion rot);

            Collider[] hits = Physics.OverlapBox(center, half, rot, _mask);

            foreach (var col in hits)
            {
                if (col.TryGetComponent(out SimpleCube cube))
                {
                    if (_absorbed.Contains(cube))
                        continue;

                    _absorbed.Add(cube);
                    Debug.Log("Cube absorbed");
                    CubeAbsorbed?.Invoke(cube);
                }
            }
        }

        private void GetWorldBox(out Vector3 worldCenter, out Vector3 worldHalfExtents, out Quaternion worldRotation)
        {
            worldCenter = _transform.TransformPoint(_boxCollider.center);
            Vector3 scaled = Vector3.Scale(_localSize, _transform.lossyScale);
            worldHalfExtents = scaled * 0.5f;
            worldRotation = _transform.rotation;
        }
    }
}