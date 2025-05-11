using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Template : MonoBehaviour
{
    [SerializeField] private TemplateFrameReference _frameReference;

    private List<TemplateCube> _templateCubes = new List<TemplateCube>();

    public IReadOnlyList<IReadonlyCube> TemplateCubes => _templateCubes;

    private void Awake()
    {
        _templateCubes = GetComponentsInChildren<TemplateCube>().ToList();
        _frameReference.InitTemplateCubes(_templateCubes);
        TemplateCube[] templateCubes = _templateCubes.Where(c => c.Type == CubeType.In).ToArray();
        Debug.Log("total IN" + templateCubes.Length);
    }

    public IReadonlyCube GetCube(int index)
    {
        foreach (IReadonlyCube cube in _templateCubes)
            if (cube.Index == index)
                return cube;

        return null;
    }
}