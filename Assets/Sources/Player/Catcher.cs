using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CapsuleCollider))]
public class Catcher : MonoBehaviour
{
    [SerializeField] private int _normalLayer; 
    [SerializeField] private int _fallingLayer;
    [SerializeField] private GrowHandler _growHandler;
    [SerializeField] private Transform _hole;
    [SerializeField] private float _damageRate;

    private CapsuleCollider _sensor;
    private Coroutine _refreshCoroutine;
    private WaitForEndOfFrame _waitForEndOfFrame;

    private List<Enemy> _enemiesInGravityCatch;
    private float _currentDamageTime;
   
    private void Awake()
    {
        _sensor = GetComponent<CapsuleCollider>();
        _waitForEndOfFrame = new WaitForEndOfFrame();
        _enemiesInGravityCatch = new List<Enemy>();
    }

    private void Update()
    {
        _currentDamageTime += Time.deltaTime;

        if( _currentDamageTime > _damageRate )
        {
            _currentDamageTime = 0;

            foreach(Enemy enemy  in _enemiesInGravityCatch)
            {
                if (enemy != null && enemy.isActiveAndEnabled && enemy.Level < _growHandler.CurrentSize)
                    enemy.TakeDamage(_hole, _growHandler.CurrentSize + 1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            if (enemy.Level < _growHandler.CurrentSize)
            {
                if(_enemiesInGravityCatch.Contains(enemy) == false)
                    _enemiesInGravityCatch.Add(enemy);

                enemy.gameObject.layer = _fallingLayer;
            }
        }

        if (other.gameObject.layer == _normalLayer)
            other.gameObject.layer = _fallingLayer;

        if (other.TryGetComponent(out SimpleCube simpleCube))
            simpleCube.DropDown();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _fallingLayer)
            other.gameObject.layer = _normalLayer;

        if(other.TryGetComponent(out Enemy enemy))
        {
            if (enemy.Level < _growHandler.CurrentSize)
            {
                if(_enemiesInGravityCatch.Contains(enemy))
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