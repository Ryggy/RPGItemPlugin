using System.IO;
using UnityEditor;
using UnityEngine;

public static class ItemSerialization
{
    private static string GetDefaultPath()
    {
        return Path.Combine(Application.dataPath, "Items.json");
    }

    public static void SaveItems(ItemContainer container)
    {
        string json = JsonUtility.ToJson(container, true);
        string path = GetDefaultPath();
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();  // Refresh the Asset Database to see the file in Unity
        Debug.Log($"Items saved to {path}");
    }

    public static ItemContainer LoadItems()
    {
        string path = GetDefaultPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ItemContainer container = JsonUtility.FromJson<ItemContainer>(json);
            Debug.Log($"Items loaded from {path}");
            return container;
        }
        else
        {
            Debug.LogWarning($"Save file not found at {path}. Creating a new container.");
            return new ItemContainer();
        }
    }
}