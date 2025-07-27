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

        public float CurrentVolume => GetCorrectVolume(_slider.value);

        private void Awake()
        {
            _parameterName = _audioMixerGroup.name;
        }

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(SetVolume);
        }

        private void Start()
        {
            _slider.value = 1f;
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(SetVolume);
        }

        private void SetVolume(float volume)
        {
            float correctedVolume = GetCorrectVolume(volume);
            _audioMixerGroup.audioMixer.SetFloat(_parameterName, correctedVolume);
        }

        private float GetCorrectVolume(float volume)
        {
            return Mathf.Log10(volume) * Multiplier;
        }
    }
}