using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class PointMover : PauseableObject
    {
        private const float MinSqrtDistance = 2;

        private Vector3[] _movePoints;
        private float _speed;
        private bool _isRestart;
        private Transform _transform;
        private int _index;
        private int _sign;
        private Vector3 _targetPosition;
        private float _sqrtDistance;
        private Rigidbody _rigidbody;
        private bool _isMove;

        private protected virtual void Update()
        {
            if (IsPaused || IsInitialized == false || _isMove == false)
                return;

            ChangeTarget();
        }

        private protected virtual void FixedUpdate()
        {
            if (IsPaused || IsInitialized == false || _isMove == false)
                return;

            MoveByPoint();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

            if (_movePoints.Length < 1 || _movePoints[_index] == null)
            {
                Debug.LogError("Move points is empty");
                IsInitialized = false;
                return;
            }

            IsInitialized = true;
        }

        public void InitFromConfig(PointMoverConfig pointMoverConfig)
        {
            _movePoints = pointMoverConfig.MovePoints;
            _speed = pointMoverConfig.Speed;
            _isRestart = pointMoverConfig.IsRestart;
            _isMove = true;
        }

        public void Stop() => _isMove = false;

        private void MoveByPoint()
        {
            Vector3 target = _movePoints[_index];
            target.y = _rigidbody.position.y;

            _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, target, _speed * Time.fixedDeltaTime));
        }

        private void ChangeTarget()
        {
            if (_movePoints[_index] != null)
            {
                _targetPosition = _movePoints[_index];
                _targetPosition.y = _transform.position.y;
            }

            _sqrtDistance = (_targetPosition - _transform.position).sqrMagnitude;

            if (_sqrtDistance < MinSqrtDistance)
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