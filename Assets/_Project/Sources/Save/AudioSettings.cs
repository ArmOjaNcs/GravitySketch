using System;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.Save
{
    [Serializable]
    public class AudioSettings
    {
        [SerializeField] private float _totalVolume;
        [SerializeField] private float _musicVolume;
        [SerializeField] private float _soundVolume;
        [SerializeField] private float _interfaceVolume;
        [SerializeField] private bool _toggleStatus;

        public AudioSettings()
        {
            _totalVolume = UserUtils.Unit;
            _musicVolume = UserUtils.Unit;
            _soundVolume = UserUtils.Unit;
            _interfaceVolume = UserUtils.Unit;
            _toggleStatus = true;
        }

        public float TotalVolume => _totalVolume;

        public float MusicVolume => _musicVolume;

        public float SoundVolume => _soundVolume;

        public float InterfaceVolume => _interfaceVolume;

        public bool ToggleStatus => _toggleStatus;

        public void SetTotalVolume(float value)
        {
            value = Mathf.Clamp01(value);
            _totalVolume = value;
        }

        public void SetMusicVolume(float value)
        {
            value = Mathf.Clamp01(value);
            _musicVolume = value;
        }

        public void SetSoundVolume(float value)
        {
            value = Mathf.Clamp01(value);
            _soundVolume = value;
        }

        public void SetInterfaceVolume(float value)
        {
            value = Mathf.Clamp01(value);
            _interfaceVolume = value;
        }

        public void SetToggleStatus(bool status) => _toggleStatus = status;
    }
}