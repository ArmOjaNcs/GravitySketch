using Utils;
using UnityEditor;
using UnityEngine;

namespace Table
{
    [ExecuteInEditMode]
    public class TemplateColorGenerator : MonoBehaviour
    {
        [SerializeField] private Texture2D _sourceTexture;

#if UNITY_EDITOR
        [ContextMenu("Generate Template Color Reference")]
        public void GenerateReference()
        {
            if (_sourceTexture == null)
            {
                Debug.LogError("Source texture not assigned.");
                return;
            }

            var materialReference = ScriptableObject.CreateInstance<TemplateColorReference>();

            for (int y = 0; y < UserUtils.ImageResolution; y++)
            {
                for (int x = 0; x < UserUtils.ImageResolution; x++)
                {
                    int index = (y * UserUtils.ImageResolution) + x;
                    Color color = _sourceTexture.GetPixel(x, y);

                    if (UserUtils.IsTransparent(color) || UserUtils.IsBlack(color))
                        continue;

                    materialReference.AddColorEntry(color, index);
                }
            }

            string assetPath = "Assets/Resources/ScriptableAssets/SmileyReference.asset";
            AssetDatabase.CreateAsset(materialReference, assetPath);
            EditorUtility.SetDirty(materialReference);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = materialReference;

            Debug.Log("TemplateColorReference created.");
        }
#endif
    }
}