using System;
using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.Dissolvable
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class DissolvableObject : PauseableRoutine
    {
        [SerializeField]
        [Min(0)] private int _reward;
        [SerializeField]
        [Min(0)] private int _size;
        [SerializeField] private AudioClip _collisionSound = null;

        private protected Tween DissolveAnimation;
        private protected Collider Collider = null;

        private Vector3 _currentVelocity;
        private Vector3 _currentAngularVelocity;
        private Transform _transform;
        private Transform _hole;
        private Rigidbody _rigidbody;
        private AudioPlayerSpawner _audioPlayerSpawner;
        private bool _isDropped;
        private bool _isNeedColisionSound;
        private bool _wasPlayingBeforePause;
        private float _dissolveAnimationTime;
        private int _totalCollisionsCount;
        private int _previousCollisionsCount;

        public event Action Finished;

        public int Size => _size;

        public int Reward => _reward;

        public bool IsDissolving { get; private set; }

        private protected virtual void Awake()
        {
            SetPhysicalIndicators();
        }

        private protected override void OnDisable()
        {
            if (DissolveAnimation.IsActive() && _isDropped)
                DissolveAnimation.Kill();

            base.OnDisable();
        }

        private protected virtual void OnCollisionEnter(Collision collision)
        {
            if (IsPaused || collision.collider.gameObject.layer == UserUtils.PipeLayer || _audioPlayerSpawner == null)
                return;

            if (_rigidbody.velocity.sqrMagnitude < UserUtils.GetMinVelocitySqrt())
                return;

            if (_isNeedColisionSound == false)
            {
                _isNeedColisionSound = true;
                return;
            }

            if (_collisionSound != null && _isDropped)
            {
                _totalCollisionsCount++;

                if (_previousCollisionsCount >= _totalCollisionsCount)
                {
                    _previousCollisionsCount = _totalCollisionsCount;
                    return;
                }
                else
                {
                    _previousCollisionsCount = _totalCollisionsCount;
                    _audioPlayerSpawner.GetAudioPlayer(_transform.position)?
                                       .SetAudioClip(_collisionSound)?.Play();
                }
            }
        }

        private protected virtual void OnCollisionExit(Collision collision)
        {
            if (collision.collider.gameObject.layer == UserUtils.PipeLayer || _audioPlayerSpawner == null)
                return;

            _totalCollisionsCount--;
        }

        public void SetDissolveAnimationTime(float animationTime)
        {
            if (animationTime <= 0)
                return;

            _dissolveAnimationTime = animationTime;
        }

        public void SetAudioPlayerSpawner(AudioPlayerSpawner audioPlayerSpawner)
            => _audioPlayerSpawner = audioPlayerSpawner;

        public override void Pause()
        {
            base.Pause();

            if (DissolveAnimation.IsActive() && DissolveAnimation.IsPlaying())
            {
                DissolveAnimation.Pause();
                _wasPlayingBeforePause = true;
            }

            if (_rigidbody != null && _isDropped && IsDissolving == false)
            {
                _currentVelocity = _rigidbody.velocity;
                _currentAngularVelocity = _rigidbody.angularVelocity;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }
        }

        public override void Resume()
        {
            base.Resume();

            if (DissolveAnimation.IsActive() && _wasPlayingBeforePause)
            {
                DissolveAnimation.Play();
                _wasPlayingBeforePause = false;
            }

            if (_rigidbody != null && _isDropped && IsDissolving == false)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _currentVelocity;
                _rigidbody.angularVelocity = _currentAngularVelocity;
            }
        }

        public void SetSize(int size)
        {
            if (size < 0)
            {
                _size = 0;
                _reward = 0;
                return;
            }

            _size = size;
            _reward = GetReward(size);
        }

        public virtual void DropDown()
        {
            if (_isDropped)
                return;

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            _isDropped = true;
            gameObject.layer = UserUtils.NormalLayer;
        }

        public virtual void Dissolve(Transform hole)
        {
            if (IsDissolving)
                return;

            IsDissolving = true;

            if (Mathf.Approximately(_dissolveAnimationTime, 0))
                _dissolveAnimationTime = UserUtils.Unit + UserUtils.HalfOfUnit;

            DissolveAnimation = AnimationSpawner.GetDissolveAnimation(transform, _dissolveAnimationTime);
            _hole = hole;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _transform.SetParent(hole);

            if (Collider != null)
                Collider.enabled = false;

            DissolveAnimation?.Restart();
            Routine = StartCoroutine(UpdateRoutine(DissolveAnimation.Duration()));
        }

        public virtual void SetSpeculative() => _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        public virtual void SetDynamic() => _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        private protected override void OnRoutineStart() { }

        private protected override void OnRoutineIteration(float cycleDuration)
        {
            float progress = ElapsedTime / cycleDuration;
            _transform.position = Vector3.Lerp(_transform.position, _hole.position, progress);
        }

        private protected override void OnRoutineEnd()
        {
            _transform.position = _hole.position;
            DissolveAnimation.Pause();
            DissolveAnimation.Kill();
            Finished?.Invoke();
            gameObject.SetActive(false);
        }

        private protected void SetPhysicalIndicators()
        {
            if (TryGetComponent(out Collider collider))
                Collider = collider;

            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.mass = 1f;
        }

        private int GetReward(int size)
        {
            if (size < 0)
                return 0;

            return size * UserUtils.RewardBySize;
        }
    }
}