using Assets.Sources.Audio;
using Assets.Sources.ColorizerScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Save;
using Assets.Sources.Table;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Sources.Level
{
    public class PaintStage : Stage
    {
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private Validator _validator;
        [SerializeField] private ColoringPositionHandler _positionHandler;
        [SerializeField] private ColorReferenceViewHandler _referenceViewer;
        [SerializeField] private Toggle _autoPaint;
        [SerializeField] private GameObject _totalScore;
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _aim;
        [SerializeField] private Button _toNextLevel;
        [SerializeField] private AudioClip _toggleSound;
        [SerializeField] private GameObject _hole;
        [SerializeField] private SmoothedFade _interfaceFade;

        private TemplateColorReference _colorReference;
        private Template _template;
        private bool _isFinished;
        private string _nextStageName = string.Empty;

        public event Action<int> TotalScoreUpdated;

        public bool IsReferenceShowing { get; private set; }
        public IReadOnlyList<IReadonlyTemplateCube> TemplateCubes => _template.TemplateCubes;

        private protected override void OnEnable()
        {
            base.OnEnable();
            _validator.Finished += OnFinished;
            _referenceViewer.IsShowing += OnShowing;
            _autoPaint.onValueChanged.AddListener(OnAutoPaint);
            _toNextLevel.onClick.AddListener(OnNextApplied);
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _validator.Finished -= OnFinished;
            _referenceViewer.IsShowing -= OnShowing;
            _autoPaint.onValueChanged.RemoveListener(OnAutoPaint);
            _toNextLevel.onClick.RemoveListener(OnNextApplied);
        }

        public override void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            base.Init(pauseHandler, audioPlayerSpawner);
            _colorizer.SetStage(this, CurrentColors);
            _colorizer.Init(pauseHandler);
            _validator.Init(this, audioPlayerSpawner);
            _referenceViewer.Init(pauseHandler);
            _positionHandler.SetPaintStage(this);
            _positionHandler.Init(pauseHandler);
            _totalScore.SetActive(false);
            _panel.SetActive(false);
            _toNextLevel.gameObject.SetActive(false);
            _takeOverLimit.SetAudioPlayerSpawner(audioPlayerSpawner);
            _interfaceFade.Init(pauseHandler);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            _hole.SetActive(false);
        }

        public void SetTemplate(Template template, TemplateColorReference colorReference)
        {
            _template = template;
            _template.Init();
            _colorReference = colorReference;
            _colorReference.ResetEntriesCurrentIndex();
        }

        public IReadonlyTemplateCube GetCubeByColor(Color color)
        {
            while (_colorReference.HasFreeIndex(color))
            {
                if (_colorReference.TryGetIndexByColor(color, out int index))
                {
                    IReadonlyTemplateCube templateCube = TemplateCubes.First(tc => tc.Index == index);

                    if (templateCube != null && templateCube.IsMarked == false)
                        return templateCube;
                }
                else
                {
                    break;
                }
            }

            return null;
        }

        public IReadonlyTemplateCube GetCube(int index)
        {
            return _template.GetCube(index);
        }

        public Color GetColor(int index)
        {
            return _colorReference.GetColor(index);
        }

        private void OnFinished()
        {
            if (_isFinished == false)
            {
                StartCoroutine(FinishRoutine());
                _isFinished = true;
            }
        }

        private void OnShowing(bool isShowing)
        {
            IsReferenceShowing = isShowing;

            if (isShowing)
            {
                _colorReference.HighlightAllCubes(TemplateCubes);
            }
            else
            {
                foreach (var cube in TemplateCubes)
                    cube.StopHighlight();
            }
        }

        private void OnAutoPaint(bool isAutoPaint)
        {
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(_toggleSound).Play();
            _colorizer.SetAutoPaint(isAutoPaint);
            _positionHandler.SetAutoPaint(isAutoPaint);
            _referenceViewer.SetAutoPaint(isAutoPaint);
        }

        private IEnumerator FinishRoutine()
        {
            Finish();
            _aim.SetActive(false);

            if (UserUtils.TryGetNextStageName(StageName, out string nextStageName))
            {
                _toNextLevel.gameObject.SetActive(true);
                _nextStageName = nextStageName;
            }
            else
            {
                _toNextLevel.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(UserUtils.One);
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(FinalSound).Play();
            _template.DropDown(PauseHandler);

            _interfaceFade.FadeOut();

            yield return new WaitForSeconds(UserUtils.One);
            _totalScore.SetActive(true);
            Window.Show();
            int finalScore = _validator.MatchScore + CurrentScore;
            Progress.UpdateLevelScore(UserUtils.GetCollectStageName(StageName), finalScore);
            TotalScoreUpdated?.Invoke(finalScore);
            _hole.SetActive(true);
        }

        private protected override void OnMainMenuApplied()
        {
            if (_isFinished)
            {
                if (_nextStageName != string.Empty)
                    Progress.SetStageName(_nextStageName);
            }
            else
            {
                Progress.SetStageName(UserUtils.GetCollectStageName(StageName));
            }

            SaveSystem.SavePlayerProgress(Progress);
            base.OnMainMenuApplied();
        }

        private protected override void OnRestartApplied()
        {
            SaveSystem.SavePlayerProgress(Progress);
            base.OnRestartApplied();
        }

        private void OnNextApplied()
        {
            Progress.SetStageName(_nextStageName);
            SaveSystem.SavePlayerProgress(Progress);
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(ButtonSound).Play();
            Window.Closed += LoadNext;
            Window.Hide();
            HidePauseMenu();
        }

        private void LoadNext()
        {
            Window.Closed -= LoadNext;
            SceneManager.LoadScene(UserUtils.Collect);
        }
    }
}