using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _minSqrtDistanceToTarget = 20;
    [SerializeField] private Health _health;
    [SerializeField] private int _level;
    [SerializeField] private EnemyStopZone _stopZone;
    [SerializeField] private EnemyMoveZone _moveZone;
    [SerializeField] private EnemyAttackZone _attackZone;
    [SerializeField] private MovePointsHolder _movePointsHolder;

    private Transform _target;
    private Transform _transform;
    private NavMeshAgent _agent;
    private Tween _dissolveAnimation;
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;

    public int Level => _level;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _rigidbody.isKinematic = true;
        _transform = transform;
        _target = _movePointsHolder.GetMovePoint();
        _dissolveAnimation = AnimationSpawner.GetDissolveAnimation(transform, 4);
    }

    private void OnEnable()
    {
        _stopZone.gameObject.SetActive(true);
        _attackZone.gameObject.SetActive(true);
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void Update()
    {
        ControlDistance();

        if (_agent.isActiveAndEnabled && _agent.isStopped == false)
        {
            _agent.destination = _target.position;
            _agent.Move(transform.forward * Time.deltaTime);
        }
    }

    private void ControlDistance()
    {
        if (_target.CompareTag("MovePoint") == false)
            return;

        float sqrtDistance = (_target.position - _transform.position).sqrMagnitude;

        if (sqrtDistance < _minSqrtDistanceToTarget)
            _target = _movePointsHolder.GetMovePoint();
    }

    private void DropDown(Transform hole)
    {
        UnSubscribe();
        _stopZone.gameObject.SetActive(false);
        _attackZone.gameObject.SetActive(false);
        _agent.SafeDisable();
        _rigidbody.isKinematic = false;
        _boxCollider.isTrigger = false;
       // Dissolve(hole);
    }

    public void TakeDamage(Transform hole, float damage = 1)
    {
        _health.TakeDamage(damage);
            
        if(_health.CurrentValue <= 0)
            DropDown(hole);
    }

    public void Dissolve(Transform hole)
    {
        _dissolveAnimation.Restart();
        StartCoroutine(FallingRoutine(hole, _dissolveAnimation.Duration()));
    }

    private IEnumerator FallingRoutine(Transform hole, float duration)
    {
        yield return AnimationSpawner.GetCatchedAnimation(_transform, hole).Play();

        float elapsedTime = 0;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / duration;
            _transform.position = Vector3.Lerp(_transform.position, hole.position, normalizedPosition);

            yield return null;
        }
        
        _transform.position = hole.position;
        _transform.gameObject.SetActive(false);
    }

    private void OnIsStopped(bool isStopped) => _agent.isStopped = isStopped;

    private void OnPlayerLosed()
    {
        _target = _movePointsHolder.GetMovePoint();
        Debug.Log("Player Losed. Target tag:" + _target.tag);
    }

    private void OnPlayerDetected()
    {
        _target = _moveZone.Player;
    }

    private void Subscribe()
    {
        _stopZone.IsStopped += OnIsStopped;
        _moveZone.PlayerDetected += OnPlayerDetected;
        _moveZone.PlayerLosed += OnPlayerLosed;
    }

    private void UnSubscribe()
    {
        _stopZone.IsStopped -= OnIsStopped;
        _moveZone.PlayerDetected -= OnPlayerDetected;
        _moveZone.PlayerLosed -= OnPlayerLosed;
    }
}