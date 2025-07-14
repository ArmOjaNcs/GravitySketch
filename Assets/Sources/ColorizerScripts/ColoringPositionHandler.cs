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
        private IReadonlyTemplateCube _currentHighlighted;
        private bool _isAutoPaint;

        public event Action<IReadonlyTemplateCube> PositionApplied;

        private bool IsColoring => Input.GetMouseButton(0);

        private void Update()
        {
            if (_stage == null || IsPaused)
                return;

            if (_isAutoPaint == false && _stage.IsReferenceShowing == false)
                HandleHoverAndPaint();
        }

        public void Init(PaintStage paintStage) => _stage = paintStage;

        public void SetAutoPaint(bool isAutoPaint) => _isAutoPaint = isAutoPaint;

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
            return IsHitCube(out IReadonlyTemplateCube cube) && _currentHighlighted != null && IsColoring;
        }

        private bool IsCanHighlight()
        {
            return _currentHighlighted != null;
        }
    }
}