using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Sources.Audio
{
    public class AudioToggle : MonoBehaviour
    {
        private const float MinVolume = -80;

        [SerializeField] private Toggle _toggle;
        [SerializeField] private AudioSlider _slider;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;

        private string _parameterName;

        public event Action<bool> ValueChanged;

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(SetFloat);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(SetFloat);
        }

        public void Init()
        {
            _parameterName = _audioMixerGroup.name;
            _toggle.onValueChanged.AddListener(SetFloat);
        }

        public void SetOn(bool isOn)
        {
            _toggle.isOn = isOn;
            SetFloat(isOn);
        }

        private void SetFloat(bool isEnabled)
        {
            float volume = isEnabled ? _slider.CurrentVolume : MinVolume;
            _audioMixerGroup.audioMixer.SetFloat(_parameterName, volume);
            ValueChanged?.Invoke(isEnabled);
        }
    }
}