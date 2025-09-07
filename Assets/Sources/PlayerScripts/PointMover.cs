using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class PointMover : PauseableObject
    {
        [SerializeField] private Transform[] _movePoints;
        [SerializeField] private float _minSqrtDistance;
        [SerializeField] private float _speed;
        [SerializeField] private bool _isRestart;

        private protected Transform Transform;
        private int _index;
        private int _sign;
        private Vector3 _targetPosition;
        private float _sqrtDistance;
        private Rigidbody _rigidbody;

        private protected virtual void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            ChangeTarget();
        }

        private protected virtual void FixedUpdate()
        {
            if (IsPaused || IsInitialized == false)
                return;

            MoveByPoint();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            Transform = transform;
            _rigidbody = GetComponent<Rigidbody>();

            if (_movePoints.Length < 1 || _movePoints[_index] == null)
            {
                Debug.LogError("Move points is empty");
                IsInitialized = false;
                return;
            }

            IsInitialized = true;
        }

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
                _targetPosition.y = Transform.position.y;
            }

            _sqrtDistance = (_targetPosition - Transform.position).sqrMagnitude;

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