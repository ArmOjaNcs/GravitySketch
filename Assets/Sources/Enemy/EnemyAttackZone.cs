using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyAttackZone : MonoBehaviour
{
    [SerializeField] private float _attackRate;
    [SerializeField] private EnemyShooter _shooter;
    [SerializeField] private Transform _firePoint;

    private CubesCollector _cubesCollector;
    private float _currentTime;
    private bool _isAttacking;

    private void Update()
    {
        if (_cubesCollector == null || _isAttacking == false)
            return;

        _currentTime += Time.deltaTime;
        
        if( _currentTime > _attackRate)
        {
            _currentTime = 0;
            _shooter.Shoot(_firePoint.position, _cubesCollector.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(_cubesCollector == null)
                _cubesCollector = other.GetComponentInChildren<CubesCollector>();

            _isAttacking = true;
            Debug.Log("Player finded");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isAttacking = false;
            _currentTime = 0;
            Debug.Log("Player losed");
        }
    }
}