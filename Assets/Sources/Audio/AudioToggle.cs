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

        private void Awake()
        {
            _parameterName = _audioMixerGroup.name;
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(SetFloat);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(SetFloat);
        }

        private void SetFloat(bool isEnabled)
        {
            float volume = isEnabled ? _slider.CurrentVolume : MinVolume;
            _audioMixerGroup.audioMixer.SetFloat(_parameterName, volume);
        }
    }
}