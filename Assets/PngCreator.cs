using System.IO;
using UnityEditor;
using UnityEngine;

public class PngCreator : MonoBehaviour
{
    public Camera TargetCamera;
    public ParticleSystem ParticleSystem;
    public int Resolution = 1024;
    public string OutputPath = "Assets/Sprites/BakedSprite.png";

    public void CapturePNG()
    {
        if (TargetCamera == null)
        {
            Debug.LogError("Missing camera");
            return;
        }

        TargetCamera.allowHDR = false;
        TargetCamera.allowMSAA = false;

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(
            Resolution,
            Resolution,
            RenderTextureFormat.ARGB32,
            24
        );
        descriptor.sRGB = true;

        RenderTexture renderTexture = new RenderTexture(descriptor);
        TargetCamera.targetTexture = renderTexture;
        TargetCamera.clearFlags = CameraClearFlags.SolidColor;
        TargetCamera.backgroundColor = new Color(0, 0, 0, 0);

        TargetCamera.Render();

        Texture2D result = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        result.ReadPixels(new Rect(0, 0, Resolution, Resolution), 0, 0);
        result.Apply();

        RenderTexture.active = null;
        TargetCamera.targetTexture = null;
        renderTexture.Release();

        Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
        File.WriteAllBytes(OutputPath, result.EncodeToPNG());
        AssetDatabase.Refresh();

        Debug.Log("Saved: " + OutputPath);
    }
}