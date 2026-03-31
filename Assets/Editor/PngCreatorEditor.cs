using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PngCreator))]
public class PngCreatorEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();
    //    PngCreator creator = (PngCreator)target;

    //    if (GUILayout.Button("Capture PNG"))
    //        creator.CapturePNG();
    //}
}