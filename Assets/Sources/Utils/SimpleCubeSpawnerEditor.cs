#if UNITY_EDITOR
using Assets.Sources.SimpleCubeScripts;
using UnityEditor;
using UnityEngine;

namespace Assets.Sources.Utils
{
    [CustomEditor(typeof(SimpleCubeSpawner))]
    public class SimpleCubeSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SimpleCubeSpawner spawner = (SimpleCubeSpawner)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Prepare Queue"))
                spawner.PrepareQueue();
        }
    }

#endif
}
