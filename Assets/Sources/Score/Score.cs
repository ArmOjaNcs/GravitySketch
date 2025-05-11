using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TemplateMaterialReference _materialReference;
    [SerializeField] private Template _template;
    [SerializeField] private Colorizer _colorizer;

    public event Action ValueChanged;

    public int Value { get; private set; }

    private void OnEnable()
    {
        _colorizer.IndexApplied += Validate;
    }

    private void OnDisable()
    {
        _colorizer.IndexApplied -= Validate;
    }

    private void Validate(int index)
    {
        Material expected = _materialReference.GetMaterial(index);
        Material actual = _template.GetCube(index).Material;

        if (MaterialsMatch(expected, actual) == false)
            return;

        Value++;
        ValueChanged?.Invoke();
    }

    private bool MaterialsMatch(Material a, Material b)
    {
        return a != null && b != null && a.color == b.color;
    }
}