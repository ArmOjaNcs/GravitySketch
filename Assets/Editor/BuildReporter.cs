using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildReporter
{
    [MenuItem("Tools/Build with Report")]
    public static void BuildWithReport()
    {
        string path = EditorUtility.SaveFolderPanel("Build", "", "");
        if (string.IsNullOrEmpty(path)) return;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/Collect.unity", 
                "Assets/Scenes/Paint.unity" }, 
            locationPathName = System.IO.Path.Combine(path, "GameBuild"),
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log($"Build completed: {report.summary.result}, size: {report.summary.totalSize / (1024 * 1024)} MB");
    }
}