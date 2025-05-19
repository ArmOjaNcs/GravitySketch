#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleCubeSpawner))]
public class SimpleCubeSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SimpleCubeSpawner spawner = (SimpleCubeSpawner)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Prepare Queue")) spawner.PrepareQueue();
        if (GUILayout.Button("Spawn Cubes")) spawner.SpawnCubes();
    }
}
#endif