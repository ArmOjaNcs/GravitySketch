using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColoringPositionHandler : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private TemplateMaterialReference _materialReference;
    [SerializeField] private ColorReferenceViewer _colorReferenceViewer;

    [Header("Input")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private LayerMask _cubeLayer;
    [SerializeField] private float _maxRayDistance = 1000f;

    private Queue<Material> _availableMaterials = new();
    private TemplateCube _currentHighlighted;
    private bool _isReferenceShowing;
   
    public event Action<TemplateCube, Material> PaintApplied;
    public event Action<IEnumerable<Color>> QueueChanged;

    private bool IsColoring => Input.GetMouseButton(0);
    private IEnumerable<Color> Colors => _availableMaterials.Select(m => m.color);

    private void Awake()
    {
        List<Material> materials = _materialReference.GetAllMaterials();
        SetPaintMaterials(materials);
        QueueChanged?.Invoke(Colors);
        Debug.Log("Total cubes" + _availableMaterials.Count);
    }

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
        if (IsHitCube(out TemplateCube cube) == false)
            return;

        if (cube != _currentHighlighted)
        {
            if (_currentHighlighted != null && _isReferenceShowing == false)
            {
                _currentHighlighted.StopHighlight();
            }

            _currentHighlighted = cube;

            if (_currentHighlighted != null && _isReferenceShowing == false)
            {
                _currentHighlighted.EnableRendering();
                _currentHighlighted.Highlight(_highlightMaterial);
            }
        }

        if (IsCanPaint())
        {
            TryPaint(_currentHighlighted);
        }
    }

    private bool IsHitCube(out TemplateCube cube)
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

    private void TryPaint(TemplateCube cube)
    {
        if (cube.Type != CubeType.In || _availableMaterials.Count == 0 || cube.IsMarked)
            return;

        Material paintMaterial = _availableMaterials.Dequeue();

        cube.Mark();
        QueueChanged?.Invoke(Colors);
        PaintApplied?.Invoke(cube, paintMaterial);
    }

    private void SetPaintMaterials(IEnumerable<Material> materials)
    {
        _availableMaterials = new Queue<Material>(materials);
    }

    private bool IsCanPaint()
    {
        return IsHitCube(out TemplateCube cube) && _currentHighlighted != null && IsColoring
            && _isReferenceShowing == false;
    }
}