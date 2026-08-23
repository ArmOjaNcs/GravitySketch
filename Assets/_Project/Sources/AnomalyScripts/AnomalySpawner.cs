using System.Collections.Generic;
using Audio;
using Pause;
using UnityEngine;

namespace AnomalyScripts
{
    public class AnomalySpawner : MonoBehaviour
    {
        [SerializeField] private Anomaly _anomalyPrefab;

        private List<AnomalyConfig> _anomalyConfigs = new ();
        private AudioPlayerSpawner _audioPlayerSpawner;
        private PauseHandler _pauseHandler;

        public void Init(
            PauseHandler pauseHandler,
            AudioPlayerSpawner audioPlayerSpawner,
            List<AnomalyConfig> anomalyConfigs)
        {
            _audioPlayerSpawner = audioPlayerSpawner;
            _pauseHandler = pauseHandler;
            _anomalyConfigs = anomalyConfigs;
            CreateAnomaly();
        }

        private void CreateAnomaly()
        {
            foreach (AnomalyConfig anomalyConfig in _anomalyConfigs)
            {
                Anomaly anomaly = Instantiate(_anomalyPrefab);
                anomaly.InitFromConfig(anomalyConfig);
                anomaly.Init(_pauseHandler);
                anomaly.SetAudioPlayerSpawner(_audioPlayerSpawner);
            }
        }
    }
}