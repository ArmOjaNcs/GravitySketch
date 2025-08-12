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
    public class PaintStage : LevelScore
    {
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private Validator _validator;
        [SerializeField] private ColoringPositionHandler _positionHandler;
        [SerializeField] private ColorReferenceViewHandler _referenceViewer;
        [SerializeField] private TemplateMaterialReference _materialReference;
        [SerializeField] private Template _template;
        [SerializeField] private Toggle _autoPaint;

        private bool _isFinished;

        public event Action<int> TotalScoreUpdated;

        public bool IsReferenceShowing { get; private set; }
        public IReadOnlyList<IReadonlyTemplateCube> TemplateCubes => _template.TemplateCubes;

        private void OnEnable()
        {
            _validator.Finished += OnFinished;
            _referenceViewer.IsShowing += OnShowing;
            _autoPaint.onValueChanged.AddListener(OnAutoPaint);
        }

        private void OnDisable()
        {
            _validator.Finished -= OnFinished;
            _referenceViewer.IsShowing -= OnShowing;
            _autoPaint.onValueChanged.RemoveListener(OnAutoPaint);
        }

        public void Init(PauseHandler pauseHandler)
        {
            _materialReference.ResetEntriesCurrentIndex();
            _colorizer.SetStage(this, CurrentColors);
            _colorizer.Init(pauseHandler);
            _validator.Init(this);
            _referenceViewer.Init(pauseHandler);
            _positionHandler.SetPaintStage(this);
            _positionHandler.Init(pauseHandler);
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
                StartCoroutine(WaitingBeforeFinish());
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
            _colorizer.SetAutoPaint(isAutoPaint);
            _positionHandler.SetAutoPaint(isAutoPaint);
            _referenceViewer.SetAutoPaint(isAutoPaint);
        }

        private IEnumerator WaitingBeforeFinish()
        {
            yield return new WaitForSeconds(2);

            int finalScore = _validator.MatchScore + CurrentScore + _referenceViewer.ShowCount * UserUtils.ShowScore;
            UpdateProgress(CurrentLevelIndex, finalScore);
            TotalScoreUpdated?.Invoke(finalScore);
            int nextIndex = Index + (int)UserUtils.One;

            if (UserUtils.TryGetSceneName(nextIndex, out string _))
                SetCurrentIndex(nextIndex);
            else
                SaveProgress();
        }

        private void OnNextApplied() => LoadNextScene();
    }
}