using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Colorizer : MonoBehaviour
{
    [SerializeField] private TemplateMaterialReference _materialReference;
    [SerializeField] private ColoringPositionHandler _positionHandler;

    private Queue<Material> _availableMaterials = new();

    public event Action<IEnumerable<Color>> QueueChanged;
    public event Action<IReadonlyTemplateCube, Material> PaintApplied;

    private IEnumerable<Color> Colors
    {
        get => _availableMaterials.Select(m => m.color)
                          .Take(_availableMaterials.Count >= UserUtils.ColorizerBarCount
                              ? UserUtils.ColorizerBarCount
                              : _availableMaterials.Count);
    }

    private void Start()
    {
        List<Material> materials = _materialReference.GetAllMaterials();
        SetPaintMaterials(materials);
        QueueChanged?.Invoke(Colors);
        Debug.Log("Total colorized cubes" + _availableMaterials.Count);
    }

    private void OnEnable()
    {
        _positionHandler.PositionApplied += TryPaint;
    }

    private void OnDisable()
    {
        _positionHandler.PositionApplied -= TryPaint;
    }

    private void TryPaint(IReadonlyTemplateCube cube)
    {
        if (cube.Type != CubeType.In || _availableMaterials.Count == 0 || cube.IsMarked)
            return;

        Material paintMaterial = _availableMaterials.Dequeue();
        Debug.Log("Total colorized cubes" + _availableMaterials.Count);
        cube.Mark();
        QueueChanged?.Invoke(Colors);

        PaintApplied?.Invoke(cube, paintMaterial);
    }

    private void SetPaintMaterials(IEnumerable<Material> materials)
    {
        _availableMaterials = new Queue<Material>(materials);
    }
}