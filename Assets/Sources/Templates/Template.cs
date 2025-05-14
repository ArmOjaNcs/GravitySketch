using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Template : MonoBehaviour
{
    [SerializeField] private TemplateFrameReference _frameReference;
    [SerializeField] private List<TemplateCube> _templateCubes = new List<TemplateCube>();

    private TemplateCube[] _inCubes;
    private int _inCubesCount;

    public IReadOnlyList<IReadonlyTemplateCube> TemplateCubes => _templateCubes;

    private void Awake()
    {
        _templateCubes = GetComponentsInChildren<TemplateCube>().ToList();
        _frameReference.InitTemplateCubes(_templateCubes);
        _inCubes = _templateCubes.Where(c => c.Type == CubeType.In).ToArray();
        _inCubesCount = _inCubes.Length;
        Debug.Log("Total IN cubes: " + _inCubesCount);
    }

    public IReadonlyTemplateCube GetCube(int index)
    {
        foreach (IReadonlyTemplateCube cube in _templateCubes)
            
            if (cube.Index == index)
            {
                _inCubesCount--;
                Debug.Log("Total IN cubes: " + _inCubesCount); 
                return cube;
            }

        return null;
    }
}