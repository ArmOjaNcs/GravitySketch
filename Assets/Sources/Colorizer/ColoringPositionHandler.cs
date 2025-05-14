using System;
using UnityEngine;

public class ColoringPositionHandler : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private ColorReferenceViewer _colorReferenceViewer;

    [Header("Input")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private LayerMask _cubeLayer;
    [SerializeField] private float _maxRayDistance = 1000f;

    private IReadonlyTemplateCube _currentHighlighted;
    private bool _isReferenceShowing;
   
    public event Action<IReadonlyTemplateCube> PositionApplied;

    private bool IsColoring => Input.GetMouseButton(0);

    private void OnEnable()
    {
        _colorReferenceViewer.ReferenceShowed += OnReferenceShowed;
    }

    private void OnDisable()
    {
        _colorReferenceViewer.ReferenceShowed -= OnReferenceShowed;
    }

    private void Update()
    {
        HandleHoverAndPaint();
    }

    private void OnReferenceShowed(bool isShowed) => _isReferenceShowing = isShowed;

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
                _currentHighlighted.Highlight(_highlightMaterial);
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
            
            if(cube.Type == CubeType.In)
                return true;

            return false;
        }

        cube = null;
        return false;
    }

    private bool IsCanApplyPosition()
    {
        return IsHitCube(out IReadonlyTemplateCube cube) && _currentHighlighted != null && IsColoring
            && _isReferenceShowing == false;
    }

    private bool IsCanHighlight()
    {
        return _currentHighlighted != null && _isReferenceShowing == false;
    }
}