using System;
using ColorizerScripts;
using Level.StageScripts;
using Pause;
using Utils;
using UnityEngine;

namespace Level
{
    public class Validator : PauseableRoutine
    {
        [SerializeField] private ColorizedCubeSpawner _colorizedCubeSpawner;
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _validSound;
        [SerializeField] private AudioClip _invalidSound;

        private PaintStage _stage;
        private bool _isLastCube;

        public event Action Matched;

        public int MatchScore { get; private set; }

        private void OnEnable()
        {
            _colorizedCubeSpawner.IndexApplied += Validate;
            _colorizer.Reseted += OnReseted;
        }

        private protected override void OnDisable()
        {
            _colorizedCubeSpawner.IndexApplied -= Validate;
            _colorizer.Reseted -= OnReseted;
            base.OnDisable();
        }

        public void SetStage(PaintStage paintStage) => _stage = paintStage;

        private protected override void OnRoutineStart() => _isLastCube = true;

        private protected override void OnRoutineEnd()
        {
            if (_isLastCube)
                base.OnRoutineEnd();
        }

        private void Validate(int index, bool isAutoPaint)
        {
            if (_stage == null)
                return;

            _isLastCube = false;
            Color expectedColor = _stage.GetColor(index);
            Color actualColor = _stage.GetCube(index).Color;

            if (expectedColor == actualColor)
            {
                _audioSource.clip = _validSound;
                _audioSource.Play();

                if (isAutoPaint)
                    MatchScore += UserUtils.MatchScore / 2;
                else
                    MatchScore += UserUtils.MatchScore;

                Matched?.Invoke();
            }
            else
            {
                _audioSource.clip = _invalidSound;
                _audioSource.Play();
            }

            if (_colorizer.ColorsCount == 0)
                OnUpdate();
        }

        private void OnReseted()
        {
            if (_colorizer.ColorsCount == 0)
                OnUpdate();
        }
    }
}