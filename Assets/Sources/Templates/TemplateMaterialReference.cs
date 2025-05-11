using System;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMaterialReference : ScriptableObject
{
    [Serializable]
    private class MaterialEntry
    {
        [SerializeField] private List<int> _indexes = new List<int>();
        [SerializeField] private Material _material;
        [SerializeField] private int _count;

        public Material Material => _material;
        public int Count => _count;

        public bool HasIndex(int index)
        {
            return _indexes.Contains(index);
        }

        public void AddIndex(int index) => _indexes.Add(index);
        public void IncrementCount() => _count++;
        public void SetMaterial(Material material) => _material = material;
    }

    [SerializeField] private List<MaterialEntry> _entries = new();

    public void AddMaterialEntry(Material material, int index)
    {
        MaterialEntry existing = _entries.Find(e => e.Material.Equals(material));

        if (existing != null)
        {
            existing.IncrementCount();
            existing.AddIndex(index);
        }
        else
        {
            MaterialEntry materialEntry = new MaterialEntry();
            materialEntry.AddIndex(index);
            materialEntry.SetMaterial(material);
            materialEntry.IncrementCount();
            _entries.Add(materialEntry);
        }
    }

    public void HightLightAllCubes(IReadOnlyList<IReadonlyCube> templateCubes)
    {
        foreach (TemplateCube cube in templateCubes)
        {
            int index = cube.Index;

            foreach(MaterialEntry materialEntry  in _entries)
            {
                if (materialEntry.HasIndex(index))
                {
                    cube.EnableRendering();
                    cube.Highlight(materialEntry.Material);
                    break;
                }
            }
        }
    }

    public List<Material> GetAllMaterials()
    {
        List<Material> materials = new List<Material>();

        for(int i = 0; i < _entries.Count; i++)
        {
            for(int j = 0; j < _entries[i].Count; j++)
            {
                materials.Add(_entries[i].Material);
            }
        }

        return materials;
    }

    public Material GetMaterial(int index)
    {
        foreach (MaterialEntry materialEntry in _entries)
        {
            if (materialEntry.HasIndex(index))
                return materialEntry.Material;
        }

        return null;
    }
}