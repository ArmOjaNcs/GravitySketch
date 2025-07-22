using UnityEditor;
using UnityEngine;

namespace Assets.Sources.Utils
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class BaseSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (serializedObject?.targetObject == null)
            {
                EditorGUILayout.HelpBox("Target ScriptableObject is null", MessageType.Error);
                return;
            }

            serializedObject.Update();

            EditorGUI.BeginChangeCheck(); 
            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck()) 
            {
                serializedObject.ApplyModifiedProperties();

                EditorUtility.SetDirty(serializedObject.targetObject);

                if (Application.isPlaying == false)
                    AssetDatabase.SaveAssets();
            }
        }
    }
}