using Assets.Sources.ColorizerScripts;
using Assets.Sources.UI;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class PaintStageEntryPoint : StageEntryPoint
    {
        [SerializeField] private PaintInput _paintInput;
        [SerializeField] private ColorizerView _colorizerView;
        [SerializeField] private ColoringPositionHandler _coloringPositionHandler;
        [SerializeField] private ColorizedCubeSpawner _colorizedCubeSpawner;
        [SerializeField] private ScoreView _scoreView;

        private protected override void Initialize()
        {
            _paintInput.Init(PauseHandler);
            _coloringPositionHandler.SetPaintInput(_paintInput);
            _colorizerView.Init(PauseHandler);
            _colorizedCubeSpawner.SetAudioPlayerSpawner(AudioPlayerSpawner);
            _colorizedCubeSpawner.Init(PauseHandler);
            _scoreView.Init(PauseHandler);
            Stage.Init(PauseHandler);
        }

        private protected override void OnLoadWindowUpdated()
        {
            base.OnLoadWindowUpdated();
            _paintInput.StartInput();
        }
    }
}