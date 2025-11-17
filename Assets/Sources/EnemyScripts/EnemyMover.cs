using Assets.Sources.Utils;
using UnityEngine;
using UnityEngine.AI;
using Assets.Sources.Pause;

namespace Assets.Sources.EnemyScripts
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
        private EnemyPatrolZone _patrolZone;
        private Transform _target;
        private Vector3 _currentPoint;
        private Transform _transform;
        private NavMeshAgent _agent;
        private Vector3 _currentVelocity;
        private float _minSqrtDistanceToTarget = 20;
        private float _currentUpdateTime;
        private float _rotationSpeed = 5;
        private float _angularSpeed;
        private float _retreatDistance;
        private bool _isPlayerTarget;
        private bool _isInZone;
        private bool _isStopped;
        private bool _isRetreat;
        private bool _isActive;
        private float _retreatTimer;
        private float _retreatUpdateInterval = 0.2f;

        private void Update()
        {
            if (IsPaused || _isActive == false || IsInitialized == false)
                return;

            if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                if (_isPlayerTarget == false)
                    if (_agent.updateRotation == false)
                        EnableAgentRotation();

                if (_isPlayerTarget)
                    RotateTowards(_target.position);

                if (_isPlayerTarget == false && _isInZone)
                {
                    ControlStuck();
                    ControlDistance();
                }

                if (_isPlayerTarget && _isInZone)
                {
                    if (_agent.updateRotation)
                        EnableManualRotation();

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
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _transform = transform;
            _lastPosition = _transform.position;
            _isInZone = true;
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

        public void ReturnToZone()
        {
            _isInZone = false;
            _isPlayerTarget = false;
            GetCurrentPoint();
            DeactivateStopZone();
            ConfirmTarget();
        }

        public void SetInZone()
        {
            _isInZone = true;
            ActivateStopZone();
            _moveZone.Refresh();
        }

        public void Activate()
        {
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
                _angularSpeed = _agent.angularSpeed;
            }

            _stopZone.gameObject.SetActive(true);
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

        public void SetPatrolZone(EnemyPatrolZone patrolZone) => _patrolZone = patrolZone;

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
            _currentPoint = _patrolZone.GetRandomPointInZone();
            _currentPoint.y = _transform.position.y;
        }

        private void Subscribe()
        {
            _stopZone.PlayerIn += OnStopIn;
            _stopZone.PlayerOut += OnStopOut;
            _retreatZone.ShouldRetreat += OnRetreat;
            _moveZone.PlayerIn += OnMoveIn;
            _moveZone.PlayerOut += OnMoveOut;
        }

        private void UnSubscribe()
        {
            _stopZone.PlayerIn -= OnStopIn;
            _stopZone.PlayerOut -= OnStopOut;
            _retreatZone.ShouldRetreat -= OnRetreat;
            _moveZone.PlayerIn -= OnMoveIn;
            _moveZone.PlayerOut -= OnMoveOut;
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
            if (_isInZone == false)
                return;

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

            Vector3 enemyPos = _transform.position;
            Vector3 playerPos = _moveZone.Player.Position;
            enemyPos.y = playerPos.y;

            Vector3 baseDir = (enemyPos - playerPos).normalized;

            float checkDistance = _retreatDistance + _moveZone.Player.Radius;

            if (TryFindDirectionalEscape(enemyPos, playerPos, baseDir, checkDistance, 180, 11, out Vector3 best))
            {
                _agent.destination = best;
                return;
            }

            if (TryFindDirectionalEscape(enemyPos, playerPos, baseDir, checkDistance, 240, 17, out best))
            {
                _agent.destination = best;
                return;
            }

            if (TryFindOmniEscape(enemyPos, playerPos, checkDistance, 24, out best))
            {
                _agent.destination = best;
                return;
            }

            Vector3 breakthrough = playerPos + (playerPos - enemyPos).normalized * checkDistance;

            if (NavMesh.SamplePosition(breakthrough, out NavMeshHit hit, checkDistance, NavMesh.AllAreas))
            {
                _agent.destination = hit.position;
            }
        }

        private bool TryFindDirectionalEscape(Vector3 enemyPos, Vector3 playerPos, Vector3 baseDir,
              float distance, float arcAngle, int checks, out Vector3 bestPoint)
        {
            float halfArc = arcAngle * 0.5f;
            float angleStep = arcAngle / (checks - 1);

            bestPoint = Vector3.zero;
            float bestScore = float.MinValue;

            for (int i = 0; i < checks; i++)
            {
                float angle = -halfArc + i * angleStep;
                Vector3 dir = Quaternion.Euler(0, angle, 0) * baseDir;

                Vector3 candidate = enemyPos + dir * distance;

                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, distance, NavMesh.AllAreas))
                {
                    float score =
                        (hit.position - playerPos).sqrMagnitude
                        - Mathf.Abs(angle) * 0.05f;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestPoint = hit.position;
                    }
                }
            }

            return bestScore > float.MinValue;
        }

        private bool TryFindOmniEscape(Vector3 enemyPos, Vector3 playerPos, float distance,
              int checks, out Vector3 bestPoint)
        {
            bestPoint = Vector3.zero;
            float bestScore = float.MinValue;

            float angleStep = 360f / checks;

            for (int i = 0; i < checks; i++)
            {
                float angle = i * angleStep;
                Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

                Vector3 candidate = enemyPos + dir * distance;

                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, distance, NavMesh.AllAreas))
                {
                    float score =
                        (hit.position - playerPos).sqrMagnitude;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestPoint = hit.position;
                    }
                }
            }

            return bestScore > float.MinValue;
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

        private void ActivateStopZone()
        {
            if (_stopZone.isActiveAndEnabled)
                return;

            _stopZone.enabled = true;
            _stopZone.PlayerIn += OnStopIn;
            _stopZone.PlayerOut += OnStopOut;
            _stopZone.Refresh();
        }

        private void DeactivateStopZone()
        {
            if (_stopZone.isActiveAndEnabled == false)
                return;

            _stopZone.PlayerIn -= OnStopIn;
            _stopZone.PlayerOut -= OnStopOut;
            OnStopOut();
            _stopZone.enabled = false;
        }
    }
}