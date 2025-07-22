using Assets.Sources.Pause;
using Assets.Sources.Utils;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Sources.Dissolvable
{
    [RequireComponent(typeof(Rigidbody))]
    public class DissolvableObject : PauseableRoutine
    {
        [SerializeField, Min(0)] private int _reward;
        [SerializeField, Min(0)] private int _size;

        private Vector3 _currentVelocity;
        private Transform _transform;
        private Transform _hole;
        private Rigidbody _rigidbody;
        private bool _isInitiated;
        private bool _isDropped;
        private bool _wasPlayingBeforePause;
        private float _defaultMass;

        private protected Tween DissolveAnimation;
        private protected Collider Collider = null;

        public event Action Finished;

        public int Size => _size;
        public int Reward => _reward;
        public bool IsDissolving { get; private set; }

        private protected override void Awake()
        {
            base.Awake();

            if (TryGetComponent(out Collider collider))
                Collider = collider;
        }

        private protected override void OnDisable()
        {
            if (DissolveAnimation != null)
                DissolveAnimation.Kill();

            base.OnDisable();
        }

        private protected  override void Start()
        {
            base.Start();

            if (_isInitiated == false)
                Init();
        }

        public override void Pause()
        {
            base.Pause();

            if (DissolveAnimation != null && DissolveAnimation.IsPlaying())
            {
                DissolveAnimation.Pause();
                _wasPlayingBeforePause = true;
            }

            if (_rigidbody != null && _isDropped && IsDissolving == false)
            {
                _currentVelocity = _rigidbody.velocity;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }
        }

        public override void Resume()
        {
            base.Resume();

            if (DissolveAnimation != null && _wasPlayingBeforePause)
            {
                DissolveAnimation.Play();
                _wasPlayingBeforePause = false;
            }

            if (_rigidbody != null && _isDropped && IsDissolving == false)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = _currentVelocity;
            }
        }

        public void Init(int size)
        {
            if (_isInitiated)
                return;

            Init();

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
            gameObject.layer = UserUtils.FallingLayer;
        }

        public virtual void Dissolve(Transform hole)
        {
            if (IsDissolving)
                return;

            if (Collider != null)
                Collider.enabled = false;

            IsDissolving = true;
            _hole = hole;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _transform.SetParent(hole);
            DissolveAnimation.Restart();
            Routine = StartCoroutine(UpdateRoutine(DissolveAnimation.Duration()));
        }

        public void ResetMass()
        {
            _rigidbody.mass = 0.0001f;
        }

        public void RecoverMass()
        {
            _rigidbody.mass = _defaultMass;
        }

        private protected virtual void Init()
        {
            if (_isInitiated)
                return;

            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.mass = 1f;
            _defaultMass = _rigidbody.mass;
            DissolveAnimation = AnimationSpawner.GetDissolveAnimation(transform, 3);
            _isInitiated = true;
        }

        private int GetReward(int size)
        {
            if (size < 0)
                return 0;

            return size * UserUtils.RewardBySize;
        }

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
    }
}