using Save;
using UnityEngine;
using AudioSettings = Save.AudioSettings;

namespace Audio
{
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioSlider _totalVolume;
        [SerializeField] private AudioSlider _musicVolume;
        [SerializeField] private AudioSlider _soundVolume;
        [SerializeField] private AudioSlider _interfaceVolume;
        [SerializeField] private AudioToggle _totalVolumeStatus;
        [SerializeField] private AudioSource _toggleSource;

        private AudioSettings _settings;
        private bool _isInitialized;

        private void OnEnable()
        {
            _totalVolume.ValueChanged += OnTotalVolumeChanged;
            _musicVolume.ValueChanged += OnMusicVolumeChanged;
            _soundVolume.ValueChanged += OnSoundVolumeChanged;
            _interfaceVolume.ValueChanged += OnInterfaceVolumeChanged;
            _totalVolumeStatus.ValueChanged += OnVolumeStatusValueChanged;
        }

        private void OnDisable()
        {
            SaveSystem.SaveAudioSettings(_settings);
            _totalVolume.ValueChanged -= OnTotalVolumeChanged;
            _musicVolume.ValueChanged -= OnMusicVolumeChanged;
            _soundVolume.ValueChanged -= OnSoundVolumeChanged;
            _interfaceVolume.ValueChanged -= OnInterfaceVolumeChanged;
            _totalVolumeStatus.ValueChanged -= OnVolumeStatusValueChanged;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _settings = SaveSystem.LoadAudioSettings();

            _totalVolume.Init();
            _totalVolume.SetSliderValue(_settings.TotalVolume);
            _musicVolume.Init();
            _musicVolume.SetSliderValue(_settings.MusicVolume);
            _soundVolume.Init();
            _soundVolume.SetSliderValue(_settings.SoundVolume);
            _interfaceVolume.Init();
            _interfaceVolume.SetSliderValue(_settings.InterfaceVolume);
            _totalVolumeStatus.Init();
            _totalVolumeStatus.SetOn(_settings.ToggleStatus);
            _isInitialized = true;
        }

        private void OnTotalVolumeChanged(float volume) => _settings.SetTotalVolume(volume);

        private void OnMusicVolumeChanged(float volume) => _settings.SetMusicVolume(volume);

        private void OnSoundVolumeChanged(float volume) => _settings.SetSoundVolume(volume);

        private void OnInterfaceVolumeChanged(float volume) => _settings.SetInterfaceVolume(volume);

        private void OnVolumeStatusValueChanged(bool isOn)
        {
            _settings.SetToggleStatus(isOn);

            if (_isInitialized)
                _toggleSource.Play();
        }
    }
}