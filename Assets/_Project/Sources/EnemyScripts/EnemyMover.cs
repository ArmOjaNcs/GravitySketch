using Pause;
using Utils;
using UnityEngine;
using UnityEngine.AI;
using EnemyScripts.EnemyZones;

namespace EnemyScripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMover : PauseableObject
    {
        private const float UpdateTargetTime = 0.5f;

        [SerializeField] private EnemyZone _stopZone;
        [SerializeField] private EnemyZone _moveZone;
        [SerializeField] private GameObject _attackZone;
        [SerializeField] private EnemyRetreatZone _retreatZone;
        [SerializeField] private float _stuckSqrSpeedThreshold = 0.2f;
        [SerializeField] private float _stuckTimeThreshold = 2f;

        private float _stuckTimer;
        private Vector3 _lastPosition;
        private Vector3[] _retreatDirectories = new Vector3[8];
        private Transform _target;
        private Vector3 _currentPoint;
        private Transform _transform;
        private NavMeshAgent _agent;
        private NavMeshPath _path;
        private Vector3 _currentVelocity;
        private float _minSqrtDistanceToTarget = 20;
        private float _currentUpdateTime;
        private float _rotationSpeed = 5;
        private float _angularSpeed;
        private float _retreatDistance;
        private bool _isPlayerTarget;
        private bool _isStopped;
        private bool _isRetreat;
        private bool _isActive;
        private float _retreatTimer;
        private float _retreatUpdateInterval = 0.2f;
        private bool _isPlayerInZone;

        public bool IsBoss { get; private set; }
        public EnemyPatrolZone PatrolZone { get; private set; }

        private Vector3 Forward => _transform.forward;
        private Vector3 Right => _transform.right;

        private void Update()
        {
            if (IsPaused || _isActive == false || IsInitialized == false)
                return;

            if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                if (_isPlayerTarget == false)
                {
                    if (_agent.updateRotation == false)
                        EnableAgentRotation();

                    ControlStuck();
                    ControlDistance();
                }

                if (_isPlayerTarget && _moveZone.PlayerIsDead)
                {
                    _isPlayerTarget = false;
                    ReturnToPatrol();
                }

                if (_isPlayerTarget && _isPlayerInZone)
                {
                    if (_agent.updateRotation)
                        EnableManualRotation();

                    RotateTowards(_target.position);
                    _currentUpdateTime += Time.deltaTime;

                    if (_isRetreat == false && _isStopped == false && _currentUpdateTime > UpdateTargetTime)
                    {
                        if (_agent.isStopped)
                            _agent.isStopped = false;

                        _agent.destination = _target.position;
                        _currentUpdateTime = 0;
                    }

                    if (_isRetreat)
                        RetreatFromPlayer();

                    if (_isStopped && _isRetreat == false)
                    {
                        if (_agent.isStopped == false)
                            _agent.isStopped = true;
                    }
                }

                if (_isPlayerTarget == false && _target != null && _agent.destination == _target.position)
                    ReturnToPatrol();
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _transform = transform;
            _lastPosition = _transform.position;
            _path = new NavMeshPath();
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();
            _currentVelocity = _agent.velocity;
            _agent.SafeStop();
        }

        public override void Resume()
        {
            base.Resume();

            if (_isActive)
            {
                _agent.SafeEnable();
                _agent.velocity = _currentVelocity;
                ConfirmTarget();
            }
        }

        public void SetPatrolDistance(float distance)
        {
            if (distance <= 0)
                return;

            _minSqrtDistanceToTarget = distance;
        }

        public void CalculateRetreatDistance()
        {
            _retreatDistance = _retreatZone.ColliderRadius * transform.lossyScale.x;
        }

        public void SetIsBoss()
        {
            IsBoss = true;
            _isPlayerInZone = true;
        }

        public void Activate()
        {
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
                _angularSpeed = _agent.angularSpeed;
            }

            ActivateZones();
            _retreatZone.gameObject.SetActive(true);
            _attackZone.SetActive(true);
            Subscribe();
            _agent.enabled = true;
            GetCurrentPoint();
            ConfirmTarget();
            _isActive = true;
        }

        public void Deactivate()
        {
            UnSubscribe();
            _stopZone.gameObject.SetActive(false);
            _retreatZone.gameObject.SetActive(false);
            _attackZone.SetActive(false);
            _agent.SafeDisable();
            _isActive = false;
        }

        public void SetPatrolZone(EnemyPatrolZone patrolZone)
        {
            PatrolZone = patrolZone;
        }

        public void OnPlayerInZone()
        {
            if (IsBoss)
                return;

            _isPlayerInZone = true;
            ActivateZones();
        }

        private void OnPlayerOutZone()
        {
            if (IsBoss)
                return;

            _isPlayerInZone = false;

            if (_isPlayerTarget)
            {
                DeactivateZones();
                ReturnToPatrol();
            }
        }

        private void DeactivateZones()
        {
            _stopZone.gameObject.SetActive(false);
            _isStopped = false;
            _moveZone.gameObject.SetActive(false);
            _isPlayerTarget = false;
        }

        private void ActivateZones()
        {
            _stopZone.gameObject.SetActive(true);
            _moveZone.gameObject.SetActive(true);
        }

        private void ReturnToPatrol()
        {
            if (IsPaused)
                return;

            if (_agent.isStopped)
                _agent.isStopped = false;

            GetCurrentPoint();
            ConfirmTarget();
        }

        private void EnableManualRotation()
        {
            if (_agent.updateRotation)
            {
                _agent.updateRotation = false;
                _agent.angularSpeed = 0;
            }
        }

        private void EnableAgentRotation()
        {
            if (_agent.updateRotation == false)
            {
                _agent.updateRotation = true;
                _agent.angularSpeed = _angularSpeed;
            }
        }

        private void GetCurrentPoint()
        {
            _currentPoint = PatrolZone.GetRandomPointInZone();
            _currentPoint.y = _transform.position.y;
        }

        private void Subscribe()
        {
            _stopZone.PlayerIn += OnStopIn;
            _stopZone.PlayerOut += OnStopOut;
            _retreatZone.ShouldRetreat += OnRetreat;
            _moveZone.PlayerIn += OnMoveIn;
            _moveZone.PlayerOut += OnMoveOut;

            if (PatrolZone != null)
            {
                PatrolZone.PlayerInZone += OnPlayerInZone;
                PatrolZone.PlayerOutZone += OnPlayerOutZone;
            }
        }

        private void UnSubscribe()
        {
            _stopZone.PlayerIn -= OnStopIn;
            _stopZone.PlayerOut -= OnStopOut;
            _retreatZone.ShouldRetreat -= OnRetreat;
            _moveZone.PlayerIn -= OnMoveIn;
            _moveZone.PlayerOut -= OnMoveOut;

            if (PatrolZone != null)
            {
                PatrolZone.PlayerInZone -= OnPlayerInZone;
                PatrolZone.PlayerOutZone -= OnPlayerOutZone;
            }
        }

        private void OnStopIn() => _isStopped = true;

        private void OnStopOut() => _isStopped = false;

        private void OnRetreat(bool isRetreat) => _isRetreat = isRetreat;

        private void OnMoveOut()
        {
            _isPlayerTarget = false;
            GetCurrentPoint();
            ConfirmTarget();
        }

        private void OnMoveIn()
        {
            if (_target == null)
                _target = _moveZone.Player.transform;

            _isPlayerTarget = true;
        }

        private void RetreatFromPlayer()
        {
            if (_moveZone.Player == null || _agent == null || _agent.isOnNavMesh == false)
                return;

            if (_agent.isStopped)
                _agent.isStopped = false;

            _retreatTimer += Time.deltaTime;

            if (_retreatTimer < _retreatUpdateInterval)
                return;

            _retreatTimer = 0;
            float checkDistance = _retreatDistance + _moveZone.Player.Radius;

            _retreatDirectories[0] = -Forward;
            _retreatDirectories[1] = (-Forward + Right).normalized;
            _retreatDirectories[2] = (-Forward - Right).normalized;
            _retreatDirectories[3] = Right;
            _retreatDirectories[4] = -Right;
            _retreatDirectories[5] = (Forward + Right).normalized;
            _retreatDirectories[6] = (Forward - Right).normalized;
            _retreatDirectories[7] = Forward;

            foreach (var dir in _retreatDirectories)
            {
                Vector3 target = _transform.position + (dir * checkDistance);

                if (NavMesh.SamplePosition(target, out NavMeshHit hit, checkDistance, NavMesh.AllAreas) == false)
                    continue;

                if (_agent.CalculatePath(hit.position, _path) == false)
                    continue;

                if (_path.status != NavMeshPathStatus.PathComplete)
                    continue;

                _agent.SetDestination(hit.position);
                break;
            }
        }

        private void RotateTowards(Vector3 targetPosition)
        {
            Vector3 lookVector = targetPosition - _transform.position;
            lookVector.y = 0;

            if (lookVector.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(lookVector);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private void ControlDistance()
        {
            float sqrtDistance = (_currentPoint - _transform.position).sqrMagnitude;

            if (sqrtDistance < _minSqrtDistanceToTarget)
            {
                GetCurrentPoint();
                ConfirmTarget();
            }
        }

        private void ControlStuck()
        {
            if (_agent == null || _agent.isActiveAndEnabled == false || _agent.isOnNavMesh == false)
                return;

            Vector3 delta = _transform.position - _lastPosition;
            float sqrSpeed = delta.sqrMagnitude / (Time.deltaTime * Time.deltaTime);
            _lastPosition = _transform.position;

            if (sqrSpeed < _stuckSqrSpeedThreshold)
            {
                _stuckTimer += Time.deltaTime;

                if (_stuckTimer >= _stuckTimeThreshold)
                {
                    GetCurrentPoint();
                    ConfirmTarget();
                    _stuckTimer = 0f;
                }
            }
            else
            {
                _stuckTimer = 0f;
            }
        }

        private void ConfirmTarget()
        {
            if (_agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                if (_isPlayerTarget)
                    _agent.destination = _target.position;
                else
                    _agent.destination = _currentPoint;

                _agent.isStopped = false;
            }
        }
    }
}