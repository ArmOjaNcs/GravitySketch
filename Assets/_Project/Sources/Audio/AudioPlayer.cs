using System;
using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : PauseableObject
    {
        public bool IsFinishable { get; private set; }

        private Transform _transform;
        private bool _isFinished;
        private bool _isPlaying;
        private bool _isUI;

        public event Action<AudioPlayer> PlaybackIsFinished;

        public AudioSource AudioSource { get; private set; }

        private void OnEnable()
        {
            _isFinished = false;
            _isUI = false;
        }

        private void OnDisable()
        {
            Stop();
        }

        private void Update()
        {
            if (IsInitialized == false || _isPlaying == false || IsPaused)
                return;

            if (IsFinishable == false)
            {
                if (AudioSource.isPlaying == false && AudioSource.loop == false)
                    Stop();

                return;
            }

            if (AudioSource.isPlaying == false && _isFinished == false)
            {
                Stop();
                _isFinished = true;
                PlaybackIsFinished?.Invoke(this);
            }
        }

        public AudioPlayer SetUI()
        {
            _isUI = true;
            return this;
        }

        public void SetFinishable() => IsFinishable = true;

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
            if (_isUI || _isFinished)
                return;

            base.Pause();

            if (_isPlaying)
                AudioSource.Pause();
        }

        public override void Resume()
        {
            if (_isUI || _isFinished)
                return;

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