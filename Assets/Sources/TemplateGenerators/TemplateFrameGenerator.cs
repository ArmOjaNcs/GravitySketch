#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TemplateFrameGenerator : MonoBehaviour
{
    [SerializeField] private TemplateCube _cubePrefab;
    [SerializeField] private Texture2D _sourceTexture;
    [SerializeField] private Material _defaultMaterial;

    private Vector2Int _pixelPositions = new Vector2Int(UserUtils.ImageResolution, UserUtils.ImageResolution);
    private CubeType[] _cubeTypes = new CubeType[UserUtils.ImageResolution * UserUtils.ImageResolution];

    [ContextMenu("Generate Template")]
    public void Generate()
    {
        ClearChildren();

        GameObject gameObject = new GameObject("Template_Container");
        gameObject.transform.parent = transform;

        float offsetX = _pixelPositions.x / UserUtils.Half - UserUtils.HalfUnit;
        float offsetY = _pixelPositions.y / UserUtils.Half - UserUtils.HalfUnit;

        gameObject.transform.localPosition = new Vector3(offsetX, offsetY, 0);

        TemplateFrameReference frameReference = ScriptableObject.CreateInstance<TemplateFrameReference>();

        for (int y = 0; y < _pixelPositions.y; y++)
        {
            for (int x = 0; x < _pixelPositions.x; x++)
            {
                int index = y * _pixelPositions.x + x;
                CubeType type = _cubeTypes[index];

                switch (type)
                {
                    case CubeType.Border:
                        InitiateCube(new Vector2(x, y), gameObject.transform);
                        frameReference.AddIndexedType(index, type);
                        break;
                    case CubeType.In:
                        TemplateCube cube = InitiateCube(new Vector2(x, y), gameObject.transform);
                        cube.DisableRendering();
                        cube.ApplyMaterial(_defaultMaterial);
                        frameReference.AddIndexedType(index, type);
                        break;
                    case CubeType.Out:
                        continue;
                }
            }
        }

        string assetPath = "Assets/Resources/ScriptableAssets/TemplateFrameReference.asset";
        AssetDatabase.CreateAsset(frameReference, assetPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = frameReference;

        Debug.Log("Template generated!");
    }

    [ContextMenu("Read PNG into CubeTypeArray")]
    public void LoadFromPNG()
    {
        if (_sourceTexture == null)
        {
            Debug.LogError("No PNG texture assigned!");
            return;
        }

        for (int y = 0; y < _pixelPositions.y; y++)
        {
            for (int x = 0; x < _pixelPositions.x; x++)
            {
                int index = y * _pixelPositions.x + x;
                Color pixel = _sourceTexture.GetPixel(x, y);

                if (pixel.a == 0)
                {
                    _cubeTypes[index] = CubeType.Out;
                }
                else if (UserUtils.IsBlack(pixel))
                {
                    _cubeTypes[index] = CubeType.Border;
                }
                else
                {
                    _cubeTypes[index] = CubeType.In;
                }
            }
        }

        Debug.Log("Template loaded from PNG!");
    }

    private void ClearChildren()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private TemplateCube InitiateCube(Vector2 position, Transform container)
    {
        TemplateCube cube = PrefabUtility.InstantiatePrefab(_cubePrefab, transform) as TemplateCube;
        cube.transform.localPosition = position;
        cube.transform.parent = container;
        return cube;
    }
}
#endif