using Assets.Sources.Audio;
using Assets.Sources.ColorizerScripts;
using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class Validator : MonoBehaviour
    {
        [SerializeField] private ColorizedCubeSpawner _colorizedCubeSpawner;
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private AudioClip _validSound;
        [SerializeField] private AudioClip _invalidSound;

        private PaintStage _stage;
        private AudioPlayerSpawner _audioPlayerSpawner;

        public event Action Matched;
        public event Action Finished;

        public int MatchScore { get; private set; }

        private void OnEnable()
        {
            _colorizedCubeSpawner.IndexApplied += Validate;
            _colorizer.Reseted += OnReseted;
        }

        private void OnDisable()
        {
            _colorizedCubeSpawner.IndexApplied -= Validate;
            _colorizer.Reseted -= OnReseted;
        }

        public void Init(PaintStage paintStage, AudioPlayerSpawner audioPlayerSpawner)
        {
            _stage = paintStage;
            _audioPlayerSpawner = audioPlayerSpawner;
        }

        private void Validate(int index, bool isAutoPaint)
        {
            if (_stage == null)
                return;

            Color expectedColor = _stage.GetColor(index);
            Color actualColor = _stage.GetCube(index).Color;

            if (expectedColor == actualColor)
            {
                _audioPlayerSpawner.GetAudioPlayer().SetAudioClip(_validSound).Play();

                if (isAutoPaint)
                    MatchScore += UserUtils.MatchScore / 2;
                else
                    MatchScore += UserUtils.MatchScore;

                Matched?.Invoke();
            }
            else
            {
                _audioPlayerSpawner.GetAudioPlayer().SetAudioClip(_invalidSound).Play();
            }

            if (_colorizer.ColorsCount == 0)
                Finished?.Invoke();
        }

        private void OnReseted()
        {
            if (_colorizer.ColorsCount == 0)
                Finished?.Invoke();
        }
    }
}