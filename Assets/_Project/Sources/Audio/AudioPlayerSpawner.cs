using System.Collections.Generic;
using Pause;
using Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioPlayerSpawner : MonoBehaviour
    {
        private readonly List<AudioSource> _active3DSources = new();
        private readonly List<AudioSource> _activeUISources = new();

        [SerializeField] private AudioPlayer _audioPlayerPrefab;
        [SerializeField]
        [Min(5)] private int _capacity;
        [SerializeField]
        [Min(1)] private int _maxSimultaneous3DSounds = 12;
        [SerializeField]
        [Min(1)] private int _maxSimultaneousUISounds = 8;
        [SerializeField] private AudioMixerGroup _soundGroup;
        [SerializeField] private AudioMixerGroup _interfaceGroup;

        private ObjectPool<AudioPlayer> _pool;
        private PauseHandler _pauseHandler;

        private void Awake()
        {
            _pool = new ObjectPool<AudioPlayer>(_audioPlayerPrefab, _capacity, transform);
        }

        public void SetPauseHandler(PauseHandler pauseHandler)
        {
            _pauseHandler = pauseHandler;
        }

        public AudioPlayer GetAudioPlayer(Vector3 position)
        {
            if (_pauseHandler == null)
                return null;

            CleanupInactive(_active3DSources);

            if (_active3DSources.Count >= _maxSimultaneous3DSounds)
                return null;

            AudioPlayer audioPlayer = _pool.GetElement();
            Initialize(audioPlayer);
            audioPlayer.AudioSource.spatialBlend = 1;
            audioPlayer.SetPosition(position);
            audioPlayer.AudioSource.outputAudioMixerGroup = _soundGroup;
            audioPlayer.AudioSource.volume = 0.7f;
            _active3DSources.Add(audioPlayer.AudioSource);

            return audioPlayer;
        }

        public AudioPlayer GetAudioPlayer()
        {
            if (_pauseHandler == null)
                return null;

            CleanupInactive(_activeUISources);

            if (_activeUISources.Count >= _maxSimultaneousUISounds)
                return null;

            AudioPlayer audioPlayer = _pool.GetElement();
            Initialize(audioPlayer);
            audioPlayer.AudioSource.spatialBlend = 0;
            audioPlayer.AudioSource.outputAudioMixerGroup = _interfaceGroup;
            audioPlayer.AudioSource.volume = 0.5f;
            audioPlayer.SetUI();
            _activeUISources.Add(audioPlayer.AudioSource);

            return audioPlayer;
        }

        private void Initialize(AudioPlayer audioPlayer)
        {
            if (audioPlayer.IsInitialized == false)
                audioPlayer.Init(_pauseHandler);

            audioPlayer.SetFinishable();
            audioPlayer.PlaybackIsFinished += OnPlaybackIsFinished;
            audioPlayer.AudioSource.playOnAwake = false;
            audioPlayer.AudioSource.loop = false;
            audioPlayer.gameObject.SetActive(true);
        }

        private void OnPlaybackIsFinished(AudioPlayer audioPlayer)
        {
            audioPlayer.PlaybackIsFinished -= OnPlaybackIsFinished;
            audioPlayer.gameObject.SetActive(false);
        }

        private void CleanupInactive(List<AudioSource> list)
        {
            list.RemoveAll(s => s == null || s.isPlaying == false);
        }
    }
}