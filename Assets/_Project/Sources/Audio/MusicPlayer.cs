using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private List<AudioClip> _music;
        [SerializeField] private Player _player;
        [SerializeField] private AudioClip _gameOverClip;
        [SerializeField] private Button _next;
        [SerializeField] private Button _previous;
        [SerializeField] private AudioSource _buttonSound;

        private int _indexOfClip;
        private bool _isPlaying;

        private void OnEnable()
        {
            _player.IsDead += OnPlayerDead;
            _next.onClick.AddListener(ChangeToNextClip);
            _previous.onClick.AddListener(ChangeToPreviousClip);
        }

        private void OnDisable()
        {
            _player.IsDead -= OnPlayerDead;
            _next.onClick.RemoveListener(ChangeToNextClip);
            _previous.onClick.RemoveListener(ChangeToPreviousClip);
        }

        private void Start()
        {
            PlayRandomMusic();
        }

        private void Update()
        {
            if (_isPlaying)
            {
                if (_musicSource.isPlaying == false)
                    PlayNext();
            }
        }

        public void ChangeToNextClip()
        {
            _buttonSound.Play();
            PlayNext();
        }

        public void ChangeToPreviousClip()
        {
            _buttonSound.Play();
            _indexOfClip = --_indexOfClip;

            if (_indexOfClip < 0)
                _indexOfClip = _music.Count - 1;

            _musicSource.clip = _music[_indexOfClip];
            _musicSource.Play();
        }

        public void PlayRandomMusic()
        {
            _musicSource.loop = true;
            SetRandomMusicClip();
            _isPlaying = true;
            _musicSource.Play();
        }

        public void Stop()
        {
            _isPlaying = false;
            _musicSource.Stop();
        }

        private void PlayNext()
        {
            _indexOfClip = ++_indexOfClip % _music.Count;
            _musicSource.clip = _music[_indexOfClip];
            _musicSource.Play();
        }

        private void OnPlayerDead()
        {
            _musicSource.clip = _gameOverClip;
            _musicSource.loop = false;
            _musicSource.Play();
            _isPlaying = false;
        }

        private void SetRandomMusicClip()
        {
            _indexOfClip = Random.Range(0, _music.Count);
            _musicSource.clip = _music[_indexOfClip];
        }
    }
}