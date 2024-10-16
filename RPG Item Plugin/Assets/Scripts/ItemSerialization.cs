using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ItemSerialization
{
    public static string GetItemPath(Item item)
    {
        var folder = GetFolderPathByItemType(item.generalSettings.itemType);
        var fileName = $"{item.generalSettings.itemName}_{item.generalSettings.itemID}.json";
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
    List<string> oldFilePaths = new List<string>(); // Collect all old file paths for the confirmation window

    foreach (var item in container.items)
    {
        // Get the folder for the item
        string folderPath = Path.Combine(Application.dataPath, GetFolderPathByItemType(item.generalSettings.itemType));
        string itemPath = GetItemPath(item);

        // Check if the folder exists, if not create it
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Folder created: {folderPath}");
        }

        // Check if the item already exists and read the existing data
        string existingFilePath = null;
        bool itemExists = false;

        foreach (var itemType in Enum.GetValues(typeof(ItemType)))
        {
            string checkFolderPath = Path.Combine(Application.dataPath, "Items", itemType.ToString());

            if (Directory.Exists(checkFolderPath))
            {
                string[] files = Directory.GetFiles(checkFolderPath, "*.json");
                
                // Check each file in the folder
                foreach (var file in files)
                {
                    string json = File.ReadAllText(file);
                    Item existingItem = JsonUtility.FromJson<Item>(json);

                    // If an item with the same ID exists, prepare to overwrite
                    if (existingItem.generalSettings.itemID == item.generalSettings.itemID)
                    {
                        Debug.Log($"Item with ID {item.generalSettings.itemID} already exists in {checkFolderPath}.");
                        existingFilePath = file; // Store the path of the existing item
                        itemExists = true;
                        break; // Exit the inner loop if a match is found
                    }
                }

                // If the item is found, break out of the loop
                if (itemExists) break;
            }
        }

        // Check if the item exists and compare data
        if (itemExists)
        {
            // Read the existing JSON data
            string existingJsonData = File.ReadAllText(existingFilePath);
            string currentJsonData = JsonUtility.ToJson(item, true);

            // Compare the existing data with the current item data
            if (existingJsonData == currentJsonData)
            {
                Debug.Log($"Item {item.generalSettings.itemName} with ID {item.generalSettings.itemID} is unchanged. Skipping save.");
                continue; // Skip saving if the data is unchanged
            }
            else
            {
                // Rename the old file to indicate it's outdated
                string oldFilePath = Path.Combine(folderPath, $"old_{item.generalSettings.itemName}_{item.generalSettings.itemID}.json");
                File.Move(existingFilePath, oldFilePath);
                oldFilePaths.Add(oldFilePath); // Collect old file paths
                Debug.Log($"Old file renamed to: {oldFilePath}");
            }
        }
        else
        {
            Debug.Log($"Saving new item: {item.generalSettings.itemName} with ID {item.generalSettings.itemID}");
        }

        // Save the item as JSON (whether overwriting or saving new)
        string jsonData = JsonUtility.ToJson(item, true);
        File.WriteAllText(itemPath, jsonData);
    }
    
    // Show the confirmation window after processing all items
    if (oldFilePaths.Count > 0)
    {
        OverwriteConfirmationWindow.ShowWindow(oldFilePaths, (selectedFiles) =>
        {
            foreach (var selectedFile in selectedFiles)
            {
                if (File.Exists(selectedFile))
                {
                    File.Delete(selectedFile); // Delete selected files
                    File.Delete(selectedFile+".meta");
                    Debug.Log($"Deleted file: {selectedFile}");
                }
            }
            AssetDatabase.Refresh();
            SelectionUI.FilterList("");
        });
    }
    
    // Refresh the Asset Database to see the files in Unity
    AssetDatabase.Refresh();
    SelectionUI.FilterList(""); // Refresh the list in the UI
    Debug.Log("Items saved successfully.");
}




    public static ItemContainer LoadItems()
    {
        var container = new ItemContainer();
        var basePath = Path.Combine(Application.dataPath, "Items");

        // Get all item type folders (Weapon, Armour, NPC)
        foreach (var itemType in Enum.GetValues(typeof(ItemType)))
        {
            var folderPath = Path.Combine(basePath, itemType.ToString());

            if (Directory.Exists(folderPath))
            {
                // Load each JSON file in the folder
                var files = Directory.GetFiles(folderPath, "*.json");
                foreach (var file in files)
                {
                    var json = File.ReadAllText(file);
                    var item = JsonUtility.FromJson<Item>(json);
                    container.items.Add(item);
                }
            }
        }

        Debug.Log("Items loaded successfully.");
        return container;
    }

    public static ItemContainer LoadItems(List<TextAsset> files)
    {
        var container = new ItemContainer();
        foreach (var file in files)
        {
            var json = file.text;
            var item = JsonUtility.FromJson<Item>(json);
            container.items.Add(item);
        }

        Debug.Log("Items loaded successfully.");
        return container;
    }

    public static Item LoadItem(TextAsset file)
    {
        var json = file.text;
        var item = JsonUtility.FromJson<Item>(json);

        Debug.Log("Item loaded successfully.");
        return item;
    }
}