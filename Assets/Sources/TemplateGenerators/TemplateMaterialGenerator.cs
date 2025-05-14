using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TemplateMaterialGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D _sourceTexture;
    [SerializeField] private MaterialHolder _materialHolder;

#if UNITY_EDITOR
    [ContextMenu("Generate Template Material Reference")]
    public void GenerateReference()
    {
        if (_sourceTexture == null || _materialHolder == null)
        {
            Debug.LogError("Source texture or MaterialHolder not assigned.");
            return;
        }

        TemplateMaterialReference materialReference = ScriptableObject.CreateInstance<TemplateMaterialReference>();

        for (int y = 0; y < UserUtils.ImageResolution; y++)
        {
            for (int x = 0; x < UserUtils.ImageResolution; x++)
            {
                int index = y * UserUtils.ImageResolution + x;

                Color color = _sourceTexture.GetPixel(x, y);

                if (UserUtils.IsTransparent(color) || UserUtils.IsBlack(color)) 
                    continue;

                Material material = _materialHolder.GetOrCreateMaterial(color);
                materialReference.AddMaterialEntry(material, index);
            }
        }

        string assetPath = "Assets/Resources/ScriptableAssets/TemplateMaterialReference.asset";
        AssetDatabase.CreateAsset(materialReference, assetPath);
        EditorUtility.SetDirty(materialReference);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = materialReference;

        Debug.Log("TemplateMaterialReference created.");
    }
}
#endif