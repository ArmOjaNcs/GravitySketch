using Assets.Sources.Pause;
using Cinemachine;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class CameraShaker : PauseableObject
    {
        [SerializeField] private Player _player;
        [SerializeField] private float _intensity;
        [SerializeField] private float _time;

        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineBasicMultiChannelPerlin _noise;

        private float _shakeTimer;
        private float _shakeTimerTotal;
        private float _startingIntensity;
        private float _currentAmplitude;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void OnEnable()
        {
            _player.Damaged += ShakeCamera;
        }

        private void OnDisable()
        {
            _player.Damaged -= ShakeCamera;
        }

        private void Update()
        {
            if (IsPaused)
                return;

            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                _noise.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
            }
        }

        public override void Pause()
        {
            base.Pause();
            _currentAmplitude = _noise.m_AmplitudeGain;
            _noise.m_AmplitudeGain = 0;
        }

        public override void Resume()
        {
            base.Resume();
            _noise.m_AmplitudeGain = _currentAmplitude;
        }

        private void ShakeCamera()
        {
            _noise.m_AmplitudeGain = _intensity;
            _startingIntensity = _intensity;
            _shakeTimer = _time;
            _shakeTimerTotal = _time;
        }
    }
}