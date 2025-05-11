using System;
using System.Collections.Generic;
using UnityEngine;

public class TemplateFrameReference : ScriptableObject
{
    [Serializable]
    private class IndexedType
    {
        [SerializeField] private int _index;
        [SerializeField] private CubeType _type;

        public IndexedType(int index, CubeType type)
        {
            _index = index;
            _type = type;
        }

        public int Index => _index;
        public CubeType Type => _type;
    }

    [SerializeField] private List<IndexedType> _indexedTypes = new List<IndexedType>();

    public void AddIndexedType(int index, CubeType type)
    {
        _indexedTypes.Add(new IndexedType(index, type));
    }

    public void InitTemplateCubes(IReadOnlyList<TemplateCube> templateCubes)
    {
        for (int index = 0; index < templateCubes.Count; index++)
        {
            templateCubes[index].Init(_indexedTypes[index].Type, _indexedTypes[index].Index);
        }
    }
}