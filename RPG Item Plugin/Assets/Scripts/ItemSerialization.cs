using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ItemSerialization
{
    public static string GetItemPath(Item item)
    {
        string folder = GetFolderPathByItemType(item.generalSettings.itemType);
        string fileName = $"{item.generalSettings.itemName}_{item.generalSettings.itemID}.json";
        return Path.Combine(Application.dataPath, folder, fileName);
    }

    private static string GetFolderPathByItemType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                return "Items/Weapon";
            case ItemType.Armour:
                return "Items/Armour";
            case ItemType.NPC:
                return "Items/NPC";
            default:
                return "Items"; // Default folder if item type is unknown
        }
    }

    public static void SaveItems(ItemContainer container)
    {
        foreach (var item in container.items)
        {
            string json = JsonUtility.ToJson(item, true);
            string path = GetItemPath(item);
            string previousPath = String.Empty;
            
            // If there is a previous path (meaning the name or id has changed), get the old path
            if (!string.IsNullOrEmpty(item.generalSettings.previousFilePath))
            {
                previousPath = item.generalSettings.previousFilePath;
            }
            
            // If the previous file exists and its name has changed, rename it
            if (!string.IsNullOrEmpty(previousPath) && File.Exists(previousPath) && previousPath != path)
            {
                File.Move(previousPath, path);
                Debug.Log($"Renamed file from {previousPath} to {path}");
            }
            
            // Save the item as JSON
            File.WriteAllText(path, json);
        }
        AssetDatabase.Refresh();  // Refresh the Asset Database to see the files in Unity
        Debug.Log("Items saved successfully.");
    }
    
    public static ItemContainer LoadItems()
    {
        ItemContainer container = new ItemContainer();
        string basePath = Path.Combine(Application.dataPath, "Items");

        // Get all item type folders (Weapon, Armour, NPC)
        foreach (var itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            string folderPath = Path.Combine(basePath, itemType.ToString());

            if (Directory.Exists(folderPath))
            {
                // Load each JSON file in the folder
                string[] files = Directory.GetFiles(folderPath, "*.json");
                foreach (var file in files)
                {
                    string json = File.ReadAllText(file);
                    Item item = JsonUtility.FromJson<Item>(json);
                    container.items.Add(item); // Assuming you have a List<Item> in ItemContainer
                }
            }
        }
        
        Debug.Log("Items loaded successfully.");
        return container;
    }
}