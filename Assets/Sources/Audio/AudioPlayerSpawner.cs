using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Sources.Audio
{
    public class AudioPlayerSpawner : MonoBehaviour
    {
        [SerializeField] private AudioPlayer _audioPlayerPrefab;
        [SerializeField, Min(5)] private int _capacity;
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

        public AudioPlayer GetAudioPlayer(Vector3 position, string mixerGroupName)
        {
            if(_pauseHandler == null)
                return null;

            AudioPlayer audioPlayer = _pool.GetElement();
            Initialize(audioPlayer);
            audioPlayer.SetPosition(position);

            if (mixerGroupName.Equals(UserUtils.MixerGroupSound))
                audioPlayer.AudioSource.outputAudioMixerGroup = _soundGroup;
            else if (mixerGroupName.Equals(UserUtils.MixerGroupInterface))
                audioPlayer.AudioSource.outputAudioMixerGroup = _interfaceGroup;

            return audioPlayer;
        }

        private void Initialize(AudioPlayer audioPlayer)
        {
            audioPlayer.Init(_pauseHandler);
            audioPlayer.IsFinishable = true;
            audioPlayer.PlaybackIsFinished += OnPlaybackIsFinished;
            audioPlayer.AudioSource.playOnAwake = false;
            audioPlayer.AudioSource.loop = false;
            audioPlayer.gameObject.SetActive(true);
            audioPlayer.AudioSource.spatialBlend = 1;
        }

        private void OnPlaybackIsFinished(AudioPlayer audioPlayer)
        {
            audioPlayer.PlaybackIsFinished -= OnPlaybackIsFinished;
            audioPlayer.gameObject.SetActive(false);
        }
    }
}