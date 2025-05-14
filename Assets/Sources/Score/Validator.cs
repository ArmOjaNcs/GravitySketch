using System;
using UnityEngine;

public class Validator : MonoBehaviour
{
    [SerializeField] private TemplateMaterialReference _materialReference;
    [SerializeField] private Template _template;
    [SerializeField] private ColorizedCubeSpawner _colorizedCubeSpawner;

    public event Action ValidateConfirmed;

    private void OnEnable()
    {
        _colorizedCubeSpawner.IndexApplied += Validate;
    }

    private void OnDisable()
    {
        _colorizedCubeSpawner.IndexApplied -= Validate;
    }

    private void Validate(int index)
    {
        Material expected = _materialReference.GetMaterial(index);
        Material actual = _template.GetCube(index).Material;

        if (MaterialsMatch(expected, actual) == false)
            return;

        ValidateConfirmed?.Invoke();
    }

    private bool MaterialsMatch(Material a, Material b)
    {
        return a != null && b != null && a.color == b.color;
    }
}