using System.IO;
using UnityEditor;
using UnityEngine;

public class PngCreator : MonoBehaviour
{
    public Camera TargetCamera;
    public ParticleSystem ParticleSystem;
    public int Resolution = 1024;

    public void CapturePNG()
    {
        if (TargetCamera == null)
        {
            Debug.LogError("Camera is missing!");
            return;
        }

        if (ParticleSystem == null)
        {
            Debug.LogError("ParticleSystem is missing!");
            return;
        }

        ParticleSpriteBaker.PrepareParticle(ParticleSystem);

        TargetCamera.clearFlags = CameraClearFlags.SolidColor;
        TargetCamera.backgroundColor = new Color(0, 0, 0, 0);
        TargetCamera.allowHDR = false;
        TargetCamera.allowMSAA = false;
        TargetCamera.depthTextureMode = DepthTextureMode.None;

        RenderTexture renderTexture = new RenderTexture(Resolution, Resolution, 0, RenderTextureFormat.ARGB32);
        renderTexture.antiAliasing = 1;

        TargetCamera.targetTexture = renderTexture;

        TargetCamera.Render();

        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(Resolution, Resolution, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, Resolution, Resolution), 0, 0);
        texture.Apply();

        RenderTexture.active = null;
        TargetCamera.targetTexture = null;

        string directory = "Assets/Sprites";
        Directory.CreateDirectory(directory);

        string path = $"{directory}/Anomaly.png";

        File.WriteAllBytes(path, texture.EncodeToPNG());

        AssetDatabase.Refresh();

        DestroyImmediate(renderTexture);

        Debug.Log("Saved PNG: " + path);
    }
}