using DG.Tweening;
using System.Collections;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Pause;
using System;

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
        private float _defaultMass;

        private protected Tween DissolveAnimation;

        public event Action Finished;

        public int Size => _size;
        public int Reward => _reward;
        public bool IsDissolving { get; private set; }

        private protected  override void Start()
        {
            base.Start();

            if (_isInitiated == false)
                Init();
        }

        public override void Pause()
        {
            base.Pause();

            if(_rigidbody != null && _isDropped)
            {
                _currentVelocity = _rigidbody.velocity;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.isKinematic = true;
            }
        }

        public override void Resume()
        {
            base.Resume();

            if (_rigidbody != null && _isDropped)
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

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                if(CurrentTime < elapsedTime)
                    CurrentTime = elapsedTime;

                float progress = elapsedTime / duration;
                _transform.position = Vector3.Lerp(_transform.position, _hole.position, progress);

                yield return null;
            }

            Routine = null;
            CurrentTime = 0;
            _transform.position = _hole.position;
            DissolveAnimation.Pause();
            DissolveAnimation.Kill();
            Finished?.Invoke();
            gameObject.SetActive(false);
        }
    }
}