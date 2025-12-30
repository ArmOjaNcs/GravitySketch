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

        public void GrowTo(Vector3 targetScale, bool isPlaySound = false)
        {
            _targetScale = targetScale;
            UpdateView(Duration);
            SizeChanged?.Invoke(targetScale.x);

            if (isPlaySound)
                PlayGrowSound();
        }

        private void PlayGrowSound() => _audioPlayer.Play();

        private void OnGrowing()
        {
            Vector3 targetScale = _player.lossyScale + _sizeDelta;
            GrowTo(targetScale, true);
            SizeChanged?.Invoke(targetScale.x);
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
            base.OnRoutineEnd();
        }
    }
}