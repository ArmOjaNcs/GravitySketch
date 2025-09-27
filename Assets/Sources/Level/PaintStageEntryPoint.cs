using Assets.Sources.ColorizerScripts;
using Assets.Sources.PlayerScripts;
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
        [SerializeField] private HoleMover _holeMover;
        [SerializeField] private Renderer _tableRenderer;

        private PaintStagePrefab _paintStagePrefab;

        private protected override void Initialize()
        {
            GameObject prefab = Resources.Load<GameObject>(Stage.StageName);
            _paintStagePrefab = prefab.GetComponent<PaintStagePrefab>();
            Debug.Log($"Stage name {Stage.StageName}");
            _holeMover.Init(_paintStagePrefab.TableMaterial);
            _tableRenderer.material = _paintStagePrefab.TableMaterial;
            prefab = Instantiate(prefab);
            PaintStage paintStage = Stage.GetComponent<PaintStage>();
            paintStage.SetTemplate(_paintStagePrefab.Template, _paintStagePrefab.ColorReference);
            _paintInput.Init(PauseHandler);
            _coloringPositionHandler.SetPaintInput(_paintInput);
            _colorizerView.Init(PauseHandler);
            _colorizedCubeSpawner.Init(PauseHandler);
            _scoreView.Init(PauseHandler);
            paintStage.Init(PauseHandler, AudioPlayerSpawner);
        }

        private protected override void OnLoadWindowUpdated()
        {
            base.OnLoadWindowUpdated();
            _paintInput.StartInput();
        }
    }
}