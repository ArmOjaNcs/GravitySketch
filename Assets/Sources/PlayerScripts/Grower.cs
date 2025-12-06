using Assets.Sources.Audio;
using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class Grower : PauseableRoutine
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Vector3 _sizeDelta;
        [SerializeField] private float _growSize;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private AudioPlayer _audioPlayer;
        [SerializeField] private Transform _player;

        private Vector3 _targetScale;

        public event Action<float> SizeChanged;
        public event Action ScaleChanged;

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private protected override void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
            base.OnDisable();
        }

        public override void Pause()
        {
            base.Pause();
            _effect.Pause();
        }

        public override void Resume()
        {
            base.Resume();
            _effect.Play();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _targetScale = _player.lossyScale;
            _audioPlayer.Init(pauseHandler);
            _audioPlayer.SetUI();
            IsInitialized = true;
        }

        public void PlayGrowSound() => _audioPlayer.Play();

        private void OnGrowing()
        {
            CalculateTargetScale(false);
            OnUpdate();
            PlayGrowSound();
            SizeChanged?.Invoke(_growSize);
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
            ScaleChanged?.Invoke();
        }

        private protected override void OnRoutineEnd()
        {
            _player.localScale = _targetScale;
            ScaleChanged?.Invoke();
        }
    }
}