using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.EnemyScripts;
using Assets.Sources.Dissolvable;
using Assets.Sources.SimpleCubeScripts;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Catcher : MonoBehaviour
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Transform _hole;
        [SerializeField, Min(0)] private float _damageRate;

        private CapsuleCollider _sensor;
        private Coroutine _refreshCoroutine;
        private WaitForEndOfFrame _waitForEndOfFrame;

        private List<Enemy> _enemiesInGravityCatch;
        private float _currentDamageTime;

        public float Damage => _growHandler.CurrentSize * UserUtils.PlayerDamageMultiplier;

        private void Awake()
        {
            _sensor = GetComponent<CapsuleCollider>();
            _waitForEndOfFrame = new WaitForEndOfFrame();
            _enemiesInGravityCatch = new List<Enemy>();
        }

        private void Update()
        {
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

                DissolvableObject dissolvableObject = other.GetComponentInParent<DissolvableObject>();

                if (dissolvableObject != null)
                {
                    dissolvableObject.ResetMass();
                    Debug.Log("mass reseted");
                }
            }

            if (other.TryGetComponent(out SimpleCube simpleCube))
            {
                simpleCube.DropDown();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == UserUtils.FallingLayer)
            {
                Physics.SyncTransforms();
                other.gameObject.layer = UserUtils.NormalLayer;

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

        public void RefreshSensor()
        {
            if (_refreshCoroutine != null)
                return;

            _refreshCoroutine = StartCoroutine(RefreshRoutine());
        }

        private IEnumerator RefreshRoutine()
        {
            _sensor.enabled = false;

            yield return _waitForEndOfFrame;

            _sensor.enabled = true;
            _refreshCoroutine = null;
        }
    }
}