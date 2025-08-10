using Assets.Sources.Pause;
using System;
using UnityEngine;

namespace Assets.Sources.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : PauseableObject
    {
        public bool IsFinishable;

        private Transform _transform;
        private bool _isFinished;
        private bool _isPlaying;

        public event Action<AudioPlayer> PlaybackIsFinished;

        public AudioSource AudioSource { get; private set; }

        private void OnEnable()
        {
            _isFinished = false;
        }

        private void OnDisable()
        {
            Stop();
        }

        private void Update()
        {
            if (IsInitialized == false)
                return;

            if (_isPlaying == false || IsPaused || IsFinishable == false)
                return;

            if (AudioSource.isPlaying == false && _isFinished == false)
            {
                _isPlaying = false;
                _isFinished = true;
                PlaybackIsFinished?.Invoke(this);
            }
        }

        public void SetPosition(Vector3 position) => _transform.position = position;

        public AudioPlayer SetAudioClip(AudioClip clip)
        {
            if (IsInitialized == false)
                return null;

            AudioSource.clip = clip;
            return this;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            AudioSource = GetComponent<AudioSource>();
            _transform = transform;
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();

            if (_isPlaying)
                AudioSource.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (_isPlaying)
                AudioSource.Play();
        }

        public void Play()
        {
            if (AudioSource == null)
                return;

            AudioSource.Play();
            _isPlaying = true;
        }

        public void Stop()
        {
            if (AudioSource == null)
                return;

            AudioSource.Stop();
            _isPlaying = false;
        }
    }
}