using Assets.Sources.Dissolvable;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Catcher : PauseableObject
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Transform _hole;
        [SerializeField, Min(0)] private float _damageRate;

        private CapsuleCollider _sensor;

        private List<Enemy> _enemiesInGravityCatch;
        private List<GameObject> _objectsInGravityCatch;
        private float _currentDamageTime;

        public float Damage => _growHandler.CurrentSize * UserUtils.PlayerDamageMultiplier;

        private protected override void Awake()
        {
            base.Awake();
            _sensor = GetComponent<CapsuleCollider>();
            _enemiesInGravityCatch = new List<Enemy>();
            _objectsInGravityCatch = new List<GameObject>();
        }

        private void Update()
        {
            if (IsPaused)
                return;

            _currentDamageTime += Time.deltaTime;

            if (_currentDamageTime > _damageRate)
            {
                _currentDamageTime = 0;

                foreach (Enemy enemy in _enemiesInGravityCatch)
                {
                    if (enemy != null && enemy.isActiveAndEnabled && enemy.Size < _growHandler.CurrentSize)
                        enemy.TakeDamage(Damage);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Detect(true);

                if (enemy.Size < _growHandler.CurrentSize)
                {
                    if (_enemiesInGravityCatch.Contains(enemy) == false)
                        _enemiesInGravityCatch.Add(enemy);
                }
            }

            if (other.gameObject.layer == UserUtils.NormalLayer)
            {
                Physics.SyncTransforms();
                other.gameObject.layer = UserUtils.FallingLayer;

                if (_objectsInGravityCatch.Contains(other.gameObject) == false)
                    _objectsInGravityCatch.Add(other.gameObject);

                DissolvableObject dissolvableObject = other.GetComponentInParent<DissolvableObject>();

                if (dissolvableObject != null)
                    dissolvableObject.ResetMass();
            }

            if (other.TryGetComponent(out SimpleCube simpleCube))
                simpleCube.DropDown();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == UserUtils.FallingLayer)
            {
                Physics.SyncTransforms();
                other.gameObject.layer = UserUtils.NormalLayer;

                if (_objectsInGravityCatch.Contains(other.gameObject))
                    _objectsInGravityCatch.Remove(other.gameObject);

                DissolvableObject dissolvableObject = other.GetComponentInParent<DissolvableObject>();

                if (dissolvableObject != null)
                    dissolvableObject.RecoverMass();
            }

            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Detect(false);

                if (enemy.Size < _growHandler.CurrentSize)
                {
                    if (_enemiesInGravityCatch.Contains(enemy))
                        _enemiesInGravityCatch.Remove(enemy);
                }
            }
        }

        public void SetDie()
        {
            _sensor.enabled = false;

            foreach(GameObject gameObject in _objectsInGravityCatch)
                gameObject.layer = UserUtils.NormalLayer;
        }

        public void RefreshSensor()
        {
            _sensor.enabled = false;
            _sensor.enabled = true;
        }
    }
}