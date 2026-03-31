using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class Catcher : PauseableObject
    {
        private const float AdditionalDamageForEnemy = 0.5f;

        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Transform _hole;
        [SerializeField, Min(0)] private float _damageRate;

        private CapsuleCollider _sensor;

        private List<Enemy> _enemiesInGravityCatch;
        private List<GameObject> _objectsInGravityCatch;
        private float _currentDamageTime;
        private float _currentAdditionalDamage = 1;

        public float Damage { get; private set; }

        private void Awake()
        {
            UpdateDamageValue();
        }

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
        }

        private void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            _currentDamageTime += Time.deltaTime;

            if (_currentDamageTime > _damageRate)
            {
                _currentDamageTime = 0;

                foreach (Enemy enemy in _enemiesInGravityCatch)
                {
                    if (enemy != null && enemy.isActiveAndEnabled && enemy.Size <= _growHandler.CurrentSize)
                        enemy.TakeDamage(Damage);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Detect(true);

                if (enemy.Size <= _growHandler.CurrentSize)
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
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == UserUtils.FallingLayer)
            {
                Physics.SyncTransforms();
                other.gameObject.layer = UserUtils.NormalLayer;

                if (_objectsInGravityCatch.Contains(other.gameObject))
                    _objectsInGravityCatch.Remove(other.gameObject);
            }

            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Detect(false);

                if (_enemiesInGravityCatch.Contains(enemy))
                    _enemiesInGravityCatch.Remove(enemy);
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);

            if (IsInitialized)
                return;

            _sensor = GetComponent<CapsuleCollider>();
            _enemiesInGravityCatch = new List<Enemy>();
            _objectsInGravityCatch = new List<GameObject>();
            IsInitialized = true;
        }

        public void SetDie()
        {
            if (IsInitialized == false)
                return;

            _sensor.enabled = false;

            foreach(GameObject gameObject in _objectsInGravityCatch)
                gameObject.layer = UserUtils.NormalLayer;

            _objectsInGravityCatch.Clear();

            foreach(Enemy enemy in _enemiesInGravityCatch)
            {
                if (enemy != null)
                    enemy.Detect(false);
            }

            _enemiesInGravityCatch.Clear();
        }

        public void RefreshSensor()
        {
            if (IsInitialized == false)
                return;

            _sensor.enabled = false;
            _sensor.enabled = true;
        }

        public void UpgradeDamage()
        {
            _currentAdditionalDamage += AdditionalDamageForEnemy;
            UpdateDamageValue();
        }

        private void OnGrowing() => UpdateDamageValue();
       
        private void UpdateDamageValue()
        {
            Damage = _growHandler.CurrentSize * UserUtils.PlayerDamageMultiplier + _currentAdditionalDamage;
        }
    }
}