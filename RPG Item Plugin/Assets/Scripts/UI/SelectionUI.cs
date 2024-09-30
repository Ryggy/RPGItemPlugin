using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectionUI
{
    public static ItemContainer _itemContainer;
    public static int selectedIndex = -1;
    private TextField searchField;
    public static ListView listViewPane;
    public SelectionUI(VisualElement container, ItemContainer itemContainer)
    {
        _itemContainer = itemContainer;
        
        BuildButtons(container);
        
        // Add a search bar above the ListView
        searchField = new TextField("Search");    
        searchField.style.marginTop = 10;
        searchField.style.marginLeft = 15;
        searchField.style.marginRight = 15;
        searchField.style.marginBottom = 10;
        container.Add(searchField);
        
        searchField.RegisterValueChangedCallback(evt => FilterList(evt.newValue));
        
        // add list of items
        listViewPane = new ListView(_itemContainer.items, 75, MakeItem, BindItem);
        listViewPane.selectionType = SelectionType.Single;
        listViewPane.selectionChanged += OnListObjectSelected;
        container.Add(listViewPane);  // Add the ListView below the search bar in the container
        
        listViewPane.style.marginTop = 10;
        listViewPane.style.marginLeft = 5;
        listViewPane.style.marginRight = 5;
        listViewPane.style.marginBottom = 10;
    }
    
    private void FilterList(string searchTerm)
    {
        List<Item> filteredList;
        
        if (string.IsNullOrEmpty(searchTerm))
        {
            // If search term is empty, show the full item list
            filteredList = _itemContainer.items;
        }
        else
        {
            // Filter items by name or ID
            filteredList = _itemContainer.items
                .Where(item => item.generalSettings.itemName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                               || item.generalSettings.itemID.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    
        // Update the itemsSource of the ListView to the filtered list
        listViewPane.itemsSource = filteredList;
        
        // Rebuild the ListView to reflect the changes
        listViewPane.Rebuild();
    }
    
    private VisualElement MakeItem()
    {
        var itemElement = new VisualElement();
        itemElement.style.flexDirection = FlexDirection.Row;
        itemElement.style.alignItems = Align.FlexStart;
        itemElement.style.alignItems = Align.Center; 
        
        // Create and configure the image
        var image = new Image();
        image.style.flexGrow = 0;
        image.style.flexShrink = 0;
        itemElement.Add(image);
        
        var label = new Label();
        label.style.flexGrow = 1;
        UIExtensions.ApplyHeaderStyle(label);
        itemElement.Add(label);

        return itemElement;
    }
    
    private void BindItem(VisualElement element, int index)
    {
        // Access the filtered list via the ListView's itemsSource
        var item = (Item)listViewPane.itemsSource[index];
        var image = element.Q<Image>();
        var label = element.Q<Label>();
        
        // Set item name
        label.text = item.generalSettings.itemName; 
        
        // Check if prefab exists
        if (item.generalSettings.prefabPath != null)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(item.generalSettings.prefabPath);
            // Get Texture2D preview of the prefab
            Texture2D prefabTexture = AssetPreview.GetAssetPreview(prefab);

            if (prefabTexture != null)
            {
                // Create a sprite from the Texture2D
                Rect spriteRect = new Rect(0, 0, prefabTexture.width, prefabTexture.height);
                Sprite prefabSprite = Sprite.Create(prefabTexture, spriteRect, new Vector2(0.5f, 0.5f));

                // Assign the sprite to the Image element
                image.sprite = prefabSprite;
            }
        }
    }
    
    private void OnListObjectSelected(IEnumerable<object> selectedItems)
    {
        var itemList = selectedItems.ToList();
        if (itemList.Count > 0)
        {
            // Find the index of the selected item
            selectedIndex = _itemContainer.items.IndexOf((Item)itemList[0]);
            DetailsUI.DisplayItemDetails(_itemContainer.items[selectedIndex]);
        }
        else
        {
            selectedIndex = -1;
            DetailsUI.ClearDetailPane();
        }
    }
    
    private void BuildButtons(VisualElement container)
    {
        // Add Save, Load, Add Item and Remove Item buttons
        var buttonContainer = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                marginTop = 10,
                alignItems = Align.FlexStart,
                flexShrink = 0
            }
        };

        var saveButton = new Button(() => ItemSerialization.SaveItems(_itemContainer))
        {
            text = "Save Items"
        };
        var loadButton = new Button(() =>
        {
            _itemContainer = ItemSerialization.LoadItems();
            listViewPane.itemsSource = _itemContainer.items;
            listViewPane.Rebuild();
            DetailsUI.ClearDetailPane();
        })
        {
            text = "Load Items"
        };
        var addButton = new Button(() => AddNewItem())
        {
            text = "Add Item"
        };
        var removeButton = new Button(() => RemoveSelectedItem())
        {
            text = "Remove Item"
        };

        buttonContainer.Add(saveButton);
        buttonContainer.Add(loadButton);
        buttonContainer.Add(addButton);
        buttonContainer.Add(removeButton);
        
        buttonContainer.style.marginTop = 10;
        buttonContainer.style.marginLeft = 10;
        buttonContainer.style.marginRight = 10;
        buttonContainer.style.marginBottom = 10;
        
        container.Add(buttonContainer);
    }
    
    private void AddNewItem()
    {
        // Create a default new item
        Item newItem = new Item
        {
            generalSettings = new GeneralSettings
            {
                itemName = "New Item",
                itemID = _itemContainer.items.Count > 0 ? _itemContainer.items.Max(i => i.generalSettings.itemID) + 1 : 1,
                prefabPath = null
            },
            weaponStats = new WeaponStats(),
            modifiers = new Modifiers(),
            description = new Description(),
            notes = new Notes(),
            requirements = new ItemRequirements()
        };

        _itemContainer.items.Add(newItem);
        listViewPane.Rebuild();
        listViewPane.selectedIndex = _itemContainer.items.Count - 1;
    }
    
    private void RemoveSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < _itemContainer.items.Count)
        {
            // _itemContainer.items.RemoveAt(selectedIndex);
            // listViewPane.Rebuild();
            // selectedIndex = -1;
            // DetailsUI.ClearDetailPane();
            
            // Get the selected item
            Item selectedItem = _itemContainer.items[selectedIndex];
        
            // Get the path to the item's JSON file
            string itemPath = ItemSerialization.GetItemPath(selectedItem);
            string metaFilePath = itemPath + ".meta";  // The corresponding meta file
        
            // Remove the item from the list
            _itemContainer.items.RemoveAt(selectedIndex);
        
            // Delete the JSON file and its meta file
            if (File.Exists(itemPath))
            {
                File.Delete(itemPath);
                Debug.Log($"Deleted {itemPath}");
            }
            if (File.Exists(metaFilePath))
            {
                File.Delete(metaFilePath);
                Debug.Log($"Deleted {metaFilePath}");
            }
        
            // Refresh the AssetDatabase to apply the changes in Unity
            AssetDatabase.Refresh();
        
            // Rebuild the ListView
            listViewPane.Rebuild();
            selectedIndex = -1;
        
            // Clear the details pane
            DetailsUI.ClearDetailPane();
        }
    }
}
