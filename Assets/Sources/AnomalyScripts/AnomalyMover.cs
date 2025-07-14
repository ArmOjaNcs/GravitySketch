using UnityEngine;
using Assets.Sources.Pause;

namespace Assets.Sources.AnomalyScpipts
{
    [RequireComponent(typeof(Anomaly))]
    public class AnomalyMover : PauseableObject
    {
        [SerializeField] private Transform[] _movePoints;
        [SerializeField] private float _minSqrtDistance;
        [SerializeField] private float _speed;
        [SerializeField] private bool _isRestart;

        private Anomaly _anomaly;
        private Transform _transform;
        private int _index;
        private int _sign;
        private Vector3 _targetPosition;
        private float _sqrtDistance;
        private Rigidbody _rigidbody;
        private bool _isMove;

        private protected override void Awake()
        {
            base.Awake();
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _anomaly = GetComponent<Anomaly>();
            _isMove = true;

            if (_movePoints.Length < 1 || _movePoints[_index] == null)
                Debug.LogError("Move points is empty");
        }

        private void OnEnable()
        {
            _anomaly.IsDowned += OnIsDowned;
        }

        private void OnDisable()
        {
            _anomaly.IsDowned -= OnIsDowned;
        }

        private void Update()
        {
            if (IsPaused)
                return;

            ChangeTarget();
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;

            if (_isMove)
                MoveByPoint();
        }

        private void OnIsDowned() => _isMove = false;

        private void MoveByPoint()
        {
            Vector3 target = _movePoints[_index].position;
            target.y = _rigidbody.position.y;

            _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, target, _speed * Time.fixedDeltaTime));
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
            {
                if (_isRestart == false)
                {
                    if (_index == _movePoints.Length - 1)
                        _sign = -1;

                    if (_index == 0)
                        _sign = 1;

                    _index += 1 * _sign;
                }
                else
                {
                    _index++;
                }

                if (_isRestart)
                    _index = _index % _movePoints.Length;
            }
        }
    }
}