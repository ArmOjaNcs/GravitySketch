using Assets.Sources.Audio;
using Assets.Sources.ColorizerScripts;
using Assets.Sources.Pause;
using Assets.Sources.Table;
using Assets.Sources.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Level
{
    public class PaintStage : Stage
    {
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private Validator _validator;
        [SerializeField] private ColoringPositionHandler _positionHandler;
        [SerializeField] private ColorReferenceViewHandler _referenceViewer;
        [SerializeField] private TemplateMaterialReference _materialReference;
        [SerializeField] private Template _template;
        [SerializeField] private Toggle _autoPaint;
        [SerializeField] private GameObject _totalScore;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _toNextLevel;
        [SerializeField] private AudioClip _toggleSound;

        private bool _isFinished;

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
            _materialReference.ResetEntriesCurrentIndex();
            _colorizer.SetStage(this, CurrentColors);
            _colorizer.Init(pauseHandler);
            _validator.Init(this, audioPlayerSpawner);
            _referenceViewer.Init(pauseHandler);
            _positionHandler.SetPaintStage(this);
            _positionHandler.Init(pauseHandler);
            _totalScore.SetActive(false);
            _panel.SetActive(false);
            _toNextLevel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public IReadonlyTemplateCube GetCubeByColor(Color color)
        {
            while (_materialReference.HasFreeIndex(color))
            {
                if (_materialReference.TryGetIndexByColor(color, out int index))
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
            return _materialReference.GetColor(index);
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
                _materialReference.HighlightAllCubes(TemplateCubes);
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
            InvokeFinished();
            int nextIndex = Index + (int)UserUtils.One;
            
            if (UserUtils.TryGetSceneName(nextIndex, out string _))
            {
                SetCurrentIndex(nextIndex);
                _toNextLevel.gameObject.SetActive(true);
            }
            else
            {
                SaveProgress();
                _toNextLevel.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(UserUtils.Two);
            _totalScore.SetActive(true);
            Window.Show();
            int finalScore = _validator.MatchScore + CurrentScore + _referenceViewer.ShowCount * UserUtils.ShowScore;
            UpdateProgress(CurrentLevelIndex, finalScore);
            TotalScoreUpdated?.Invoke(finalScore);
        }

        private void OnNextApplied()
        {
            AudioPlayerSpawner.GetAudioPlayer().SetUI().SetAudioClip(ButtonSound).Play();
            Window.Closed += LoadNext;
            Window.Hide();
            HidePauseMenu();
        }

        private void LoadNext()
        {
            Window.Closed -= LoadNext;
            LoadNextScene();
        }
    }
}