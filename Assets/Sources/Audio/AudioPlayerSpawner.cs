using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.Audio
{
    public class AudioPlayerSpawner : MonoBehaviour
    {
        [SerializeField] private AudioPlayer _audioPlayerPrefab;
        [SerializeField, Min(5)] private int _capacity;

        private ObjectPool<AudioPlayer> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<AudioPlayer>(_audioPlayerPrefab, _capacity, transform);
        }

        public AudioPlayer GetAudioPlayer()
        {
            AudioPlayer audioPlayer = _pool.GetElement();
            Initialize(audioPlayer);
            return audioPlayer;
        }

        private void Initialize(AudioPlayer audioPlayer)
        {
            audioPlayer.IsFinishable = true;
            audioPlayer.PlaybackIsFinished += OnPlaybackIsFinished;
            audioPlayer.transform.position = transform.position;
            audioPlayer.AudioSource.playOnAwake = false;
            audioPlayer.AudioSource.loop = false;
            audioPlayer.gameObject.SetActive(true);
            audioPlayer.Init();
            audioPlayer.AudioSource.spatialBlend = 1;
            audioPlayer.IsFinishable = true;
        }

        private void OnPlaybackIsFinished(AudioPlayer audioPlayer)
        {
            Debug.Log("PlaybackIsFinished");
            audioPlayer.gameObject.SetActive(false);
            audioPlayer.PlaybackIsFinished -= OnPlaybackIsFinished;
        }
    }
}