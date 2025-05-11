#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Template/Material Holder")]
public class MaterialHolder : ScriptableObject
{
    [SerializeField] private Shader _shader;

    [Serializable]
    private class MaterialEntry
    {
        [SerializeField] private Color _color;
        [SerializeField] private Material _material;

        public MaterialEntry(Color color, Material material)
        {
            _color = color;
            _material = material;
        }

        public Color Color => _color;
        public Material Material => _material;
    }

    [SerializeField] private List<MaterialEntry> _materialEntries = new List<MaterialEntry>();

    public Material GetOrCreateMaterial(Color color)
    {
        if (HasColor(color, out Material material))
            return material;

        string folderPath = "Assets/Resources/GeneratedMaterials";

        if (AssetDatabase.IsValidFolder(folderPath) == false)
            AssetDatabase.CreateFolder("Assets", "GeneratedMaterials");

        string matName = $"r{(int)(color.r * 255)}_g{(int)(color.g * 255)}_b{(int)(color.b * 255)}";
        string matPath = $"{folderPath}/{matName}.mat";

        Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);

        if (mat == null)
        {
            mat = new Material(_shader);
            mat.color = color;
            AssetDatabase.CreateAsset(mat, matPath);
            AssetDatabase.SaveAssets();
        }

        _materialEntries.Add(new MaterialEntry(color, mat));
        return mat;
    }

    private bool HasColor(Color color, out Material material)
    {
        foreach (var entry in _materialEntries)
        {
            if(entry.Color == color)
            {
                material = entry.Material;
                return true;
            } 

        }

        material = null;
        return false;
    }
}
#endif