using Assets.Sources.Level;
using Assets.Sources.Pause;
using Assets.Sources.Table;
using System;
using UnityEngine;

namespace Assets.Sources.ColorizerScripts
{
    public class ColoringPositionHandler : PauseableObject
    {
        [Header("Gameplay")]
        [SerializeField] private Color _highlightColor;

        [Header("Input")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private LayerMask _cubeLayer;
        [SerializeField] private float _maxRayDistance = 1000f;

        private PaintStage _stage;
        private PaintInput _input;
        private IReadonlyTemplateCube _currentHighlighted;
        private bool _isAutoPaint;
        private bool _isColoring;

        public event Action<IReadonlyTemplateCube> PositionApplied;

        private void OnDisable()
        {
            if (_input != null)
                _input.Coloring -= OnColoring;
        }

        private void Update()
        {
            if (IsPaused || IsInitialized == false)
                return;

            if (_isAutoPaint == false && _stage.IsReferenceShowing == false)
                HandleHoverAndPaint();
        }

        public void SetPaintInput(PaintInput input)
        {
            _input = input;
            _input.Coloring += OnColoring;
        }

        public void SetPaintStage(PaintStage paintStage) => _stage = paintStage;

        public void SetAutoPaint(bool isAutoPaint) => _isAutoPaint = isAutoPaint;

        private void OnColoring(bool isColoring) => _isColoring = isColoring;

        private void HandleHoverAndPaint()
        {
            if (IsHitCube(out IReadonlyTemplateCube cube) == false)
                return;

            if (cube != _currentHighlighted)
            {
                if (IsCanHighlight())
                    _currentHighlighted.StopHighlight();

                _currentHighlighted = cube;

                if (IsCanHighlight())
                {
                    _currentHighlighted.EnableRendering();
                    _currentHighlighted.Highlight(_highlightColor);
                }
            }

            if (IsCanApplyPosition())
                PositionApplied?.Invoke(_currentHighlighted);
        }

        private bool IsHitCube(out IReadonlyTemplateCube cube)
        {
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance, _cubeLayer))
            {
                cube = hit.collider.GetComponent<TemplateCube>();

                if (cube.Type == CubeType.In)
                    return true;

                return false;
            }

            cube = null;
            return false;
        }

        private bool IsCanApplyPosition()
        {
            return IsHitCube(out IReadonlyTemplateCube cube) && _currentHighlighted != null && _isColoring;
        }

        private bool IsCanHighlight()
        {
            return _currentHighlighted != null;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);

            if (_input == null || _stage == null)
                return;

            IsInitialized = true;
        }
    }
}