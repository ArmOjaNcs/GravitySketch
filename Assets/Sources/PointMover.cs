using Unity.VisualScripting;
using UnityEngine;

public class PointMover : MonoBehaviour
{
    [SerializeField] private Transform[] _movePoints;
    [SerializeField] private float _minSqrtDistance;
    [SerializeField] private float _speed;

    private Transform _transform;
    private int _index;
    private Vector3 _targetPosition;
    private float _sqrtDistance;

    private void Awake()
    {
        _transform = transform;

        if (_movePoints.Length < 1 || _movePoints[_index] == null)
            Debug.LogError("Move points is empty");
    }

    private void Update()
    {
        ChangeTarget();
        MoveByPoint();
    }

    private void MoveByPoint()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, _speed * Time.deltaTime);
    }

    private void ChangeTarget()
    {
        if (_movePoints[_index] != null)
        {
            _targetPosition = _movePoints[_index].position;
            _targetPosition.y = _transform.position.y;
        }

        _sqrtDistance = (_targetPosition - _transform.position).sqrMagnitude;

        if (_sqrtDistance < _minSqrtDistance)
            _index++;

        _index %= _movePoints.Length;
    }
}