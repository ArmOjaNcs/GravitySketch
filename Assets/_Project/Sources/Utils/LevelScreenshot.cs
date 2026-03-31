using UnityEngine;
using System.IO;

namespace Assets.Sources.Utils
{
    public class LevelScreenshot : MonoBehaviour
    {
        public Camera _captureCamera;
        public int _resolution = 1024;

        [ContextMenu("Take Screenshot")]
        public void TakeScreenshot()
        {
            RenderTexture rt = new RenderTexture(_resolution, _resolution, 24);
            _captureCamera.targetTexture = rt;

            Texture2D screenShot = new Texture2D(_resolution, _resolution, TextureFormat.RGB24, false);
            _captureCamera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, _resolution, _resolution), 0, 0);
            screenShot.Apply();

            _captureCamera.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);

            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Path.Combine(Application.dataPath, "MinimapScreenshot.png");
            File.WriteAllBytes(filename, bytes);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif

            Debug.Log($"Saved screenshot to: {filename}");
        }
    }
}