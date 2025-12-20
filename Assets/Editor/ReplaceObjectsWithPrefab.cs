using UnityEngine;
using UnityEditor;

public class ReplaceObjectsWithPrefab : EditorWindow
{
    [SerializeField] private GameObject[] _targets;
    [SerializeField] private GameObject _prefab;

    [MenuItem("Tools/Replace Objects With Prefab")]
    private static void Open()
    {
        GetWindow<ReplaceObjectsWithPrefab>("Replace With Prefab");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Targets", EditorStyles.boldLabel);
        SerializedObject so = new SerializedObject(this);
        SerializedProperty targetsProp = so.FindProperty("_targets");
        EditorGUILayout.PropertyField(targetsProp, true);
        so.ApplyModifiedProperties();

        _prefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab",
            _prefab,
            typeof(GameObject),
            false
        );

        GUILayout.Space(10);

        GUI.enabled = _targets != null && _targets.Length > 0 && _prefab != null;

        if (GUILayout.Button("Replace"))
            Replace();

        GUI.enabled = true;
    }

    private void Replace()
    {
        foreach (GameObject target in _targets)
        {
            if (target == null)
                continue;

            ClearChildrenImmediate(target);
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(_prefab, target.scene);

            if (instance == null)
            {
                Debug.LogError("Failed to instantiate prefab");
                continue;
            }

            instance.transform.SetParent(target.transform);
            Undo.RegisterCreatedObjectUndo(instance, "Create Prefab Instance");
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
        }
    }

    private void ClearChildrenImmediate(GameObject parent)
    {
        while (parent.transform.childCount > 0)
        {
            DestroyImmediate(parent.transform.GetChild(0).gameObject);
        }
    }
}