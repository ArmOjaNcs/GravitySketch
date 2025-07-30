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

        private protected override void Awake()
        {
            base.Awake();

            if (AudioSource == null)
            {
                AudioSource = GetComponent<AudioSource>();
                _transform = transform;
            }
        }

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
            AudioSource.clip = clip;
            return this;
        }

        public void Init()
        {
            if (AudioSource != null)
                return;

            AudioSource = GetComponent<AudioSource>();
            _transform = transform;
        }

        public override void Pause()
        {
            base.Pause();

            if (IsActive() == false)
                return;

            if (_isPlaying)
                AudioSource.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (IsActive() == false)
                return;

            if (_isPlaying)
                AudioSource.Play();
        }

        public void Play()
        {
            AudioSource.Play();
            _isPlaying = true;
        }

        public void Stop()
        {
            AudioSource.Stop();
            _isPlaying = false;
        }
    }
}