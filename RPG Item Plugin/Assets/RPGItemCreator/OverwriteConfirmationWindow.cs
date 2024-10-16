using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OverwriteConfirmationWindow : EditorWindow
{
    private List<string> oldFilePaths;
    private List<bool> fileToggles;
    private System.Action<List<string>> onConfirm;

    public static void ShowWindow(List<string> oldFiles, System.Action<List<string>> onConfirmCallback)
    {
        var window = GetWindow<OverwriteConfirmationWindow>("Confirm Overwrite");
        window.oldFilePaths = oldFiles;
        window.fileToggles = new List<bool>(new bool[oldFiles.Count]);
        window.onConfirm = onConfirmCallback;
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Select files to overwrite:", EditorStyles.boldLabel);
        
        for (int i = 0; i < oldFilePaths.Count; i++)
        {
            fileToggles[i] = EditorGUILayout.Toggle(Path.GetFileName(oldFilePaths[i]), fileToggles[i]);
        }

        if (GUILayout.Button("Confirm"))
        {
            List<string> selectedFiles = new List<string>();
            for (int i = 0; i < fileToggles.Count; i++)
            {
                if (fileToggles[i])
                {
                    selectedFiles.Add(oldFilePaths[i]);
                }
            }
            onConfirm?.Invoke(selectedFiles);
            Close();
        }
    }
}