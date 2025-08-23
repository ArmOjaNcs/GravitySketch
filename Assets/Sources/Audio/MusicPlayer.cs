using Assets.Sources.PlayerScripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private List<AudioClip> _music;
        [SerializeField] private Player _player;
        [SerializeField] private AudioClip _gameOverClip;
        [SerializeField] private Button _next;
        [SerializeField] private Button _previous;
        [SerializeField] private AudioSource _buttonSource;

        private int _indexOfClip;
        private bool _isPlaying;

        private void Awake()
        {
            PlayRandomMusic();
        }

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
            _musicSource.Play();
        }

        private void Update()
        {
            if (_isPlaying)
            {
                if (_musicSource.isPlaying == false)
                    ChangeToNextClip();
            }
        }

        public void ChangeToNextClip()
        {
            _buttonSource.Play();
            _indexOfClip = ++_indexOfClip % _music.Count;
            _musicSource.clip = _music[_indexOfClip];
            _musicSource.Play();
        }

        public void ChangeToPreviousClip()
        {
            _buttonSource.Play();
            _indexOfClip = --_indexOfClip;

            if (_indexOfClip < 0)
                _indexOfClip = _music.Count - 1;

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

        private void PlayRandomMusic()
        {
            _indexOfClip = Random.Range(0, _music.Count);
            _musicSource.clip = _music[_indexOfClip];
            _isPlaying = true;
        }
    }
}