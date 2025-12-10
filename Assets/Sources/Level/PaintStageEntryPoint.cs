using Assets.Sources.ColorizerScripts;
using Assets.Sources.PlayerScripts;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class PaintStageEntryPoint : StageEntryPoint
    {
        [SerializeField] private ColorizerView _colorizerView;
        [SerializeField] private ColoringPositionHandler _coloringPositionHandler;
        [SerializeField] private ColorizedCubeSpawner _colorizedCubeSpawner;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private HoleMover _holeMover;
        [SerializeField] private Renderer _tableRenderer;

        private PaintStagePrefab _paintStagePrefab;

        private protected override void Initialize()
        {
            if(Stage.IsTutorial)
                Prefab = Resources.Load<GameObject>(UserUtils.TutorialPaintName);
            else
                Prefab = Resources.Load<GameObject>(Stage.StageName);

            Debug.Log($"Stage name {Stage.StageName}");
            Prefab = Instantiate(Prefab);

            if(Stage.IsTutorial )
                Stage.SetTutorialObject(Prefab);

            _paintStagePrefab = Prefab.GetComponent<PaintStagePrefab>();
            _holeMover.Init(_paintStagePrefab.TableMaterial);
            _tableRenderer.material = _paintStagePrefab.TableMaterial;
            _colorizerView.Init(PauseHandler);
            _colorizedCubeSpawner.Init(PauseHandler);
            _scoreView.Init(PauseHandler);
            PaintStage paintStage = Stage.GetComponent<PaintStage>();
            paintStage.SetTemplate(_paintStagePrefab.Template, _paintStagePrefab.ColorReference);
            paintStage.Init(PauseHandler, AudioPlayerSpawner);
        }

        private protected override void Begin()
        {
            base.Begin();
            _coloringPositionHandler.StartStage();
        }
    }
}