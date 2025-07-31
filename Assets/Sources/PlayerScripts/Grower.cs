using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Grower : PauseableRoutine
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private ParticleSystem[] _particleSystems;
        [SerializeField] private Vector3 _sizeDelta;
        [SerializeField] private float _growSize;
        [SerializeField] private Catcher _catcher;

        private Transform _player;
        private Vector3 _targetScale;

        public event Action<float> SizeChanged;

        private protected override void Awake()
        {
            base.Awake();
            _player = transform;
            _targetScale = _player.lossyScale;
        }

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private protected override void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
            base.OnDisable();
        }

        private void OnGrowing()
        {
            CalculateTargetScale(false);
            OnUpdate();
            SizeChanged?.Invoke(_growSize);
            _catcher.RefreshSensor();
        }

        private void CalculateTargetScale(bool isNegative)
        {
            int sign = 1;

            if (isNegative)
                sign = -1;

            _targetScale += _sizeDelta * sign;
        }

        private protected override void OnRoutineIteration(float cycleDuration) 
        {
            float progress = ElapsedTime / Duration;
            _player.localScale = Vector3.Lerp(_player.localScale, _targetScale, progress);

            foreach (ParticleSystem particle in _particleSystems)
            {
                particle.transform.localScale = Vector3.Lerp(particle.transform.localScale,
                    _targetScale, progress);
            }
        }

        private protected override void OnRoutineEnd()
        {
            _player.localScale = _targetScale;

            foreach (var particle in _particleSystems)
                particle.transform.localScale = _targetScale;
        }
    }
}