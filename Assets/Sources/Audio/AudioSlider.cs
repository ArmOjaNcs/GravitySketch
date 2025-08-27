using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Sources.Audio
{
    public class AudioSlider : MonoBehaviour
    {
        private const float Multiplier = 20;

        [SerializeField] private Slider _slider;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;

        private string _parameterName;

        public event Action<float> ValueChanged;

        public float CurrentVolume => GetCorrectVolume(_slider.value);

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(SetVolume);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(SetVolume);
        }

        public void Init()
        {
            _parameterName = _audioMixerGroup.name;
            _slider.onValueChanged.AddListener(SetVolume);
        }

        public void SetSliderValue(float value)
        {
            value = Mathf.Clamp01(value);
            _slider.value = value;
            SetVolume(value);
        }

        private void SetVolume(float volume)
        {
            float correctedVolume = GetCorrectVolume(volume);
            _audioMixerGroup.audioMixer.SetFloat(_parameterName, correctedVolume);
            ValueChanged?.Invoke(volume);
        }

        private float GetCorrectVolume(float volume)
        {
            return Mathf.Log10(volume) * Multiplier;
        }
    }
}