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
    
    // Separate ListViews for each item type
    public static ListView weaponListView;
    public static ListView armourListView;
    public static ListView npcListView;
    
    public SelectionUI(VisualElement container, ItemContainer itemContainer)
    {
        _itemContainer = itemContainer;
        
        BuildButtons(container);
        
        // Add a search bar above the ListView
        searchField = new TextField("Search"); 
        UIExtensions.ApplyHeaderStyle(searchField);
        searchField.style.marginTop = 10;
        searchField.style.marginLeft = 15;
        searchField.style.marginRight = 15;
        searchField.style.marginBottom = 10;
        container.Add(searchField);
        
        searchField.RegisterValueChangedCallback(evt => FilterList(evt.newValue));
        
        // Create foldouts for Weapons, Armour, and NPC
        var weaponFoldout = new Foldout { text = "Weapons" };
        var armourFoldout = new Foldout { text = "Armour" };
        var npcFoldout = new Foldout { text = "NPC" };

        // Create ListViews for each category
        weaponListView = new ListView(GetItemsByType(ItemType.Weapon), 75, MakeItem, (e, i) => BindItem(e, i, weaponListView));
        armourListView = new ListView(GetItemsByType(ItemType.Armour), 75, MakeItem, (e, i) => BindItem(e, i, armourListView));
        npcListView = new ListView(GetItemsByType(ItemType.NPC), 75, MakeItem, (e, i) => BindItem(e, i, npcListView));
        
        weaponListView.selectionType = SelectionType.Single;
        armourListView.selectionType = SelectionType.Single;
        npcListView.selectionType = SelectionType.Single;

        // work around to ensure it registers clicks properly 
        // needs BOTH itemsChosen and selectionChanged, even if empty
        // https://discussions.unity.com/t/i-cant-select-listview-items-on-uielements-runtime/786109/12
        weaponListView.itemsChosen += (IEnumerable<object> selectedRows) =>
        {
            Debug.Log($"itemsChosen for Weapons: {selectedRows.Count()}");
        };

        armourListView.itemsChosen += (IEnumerable<object> selectedRows) =>
        {
           Debug.Log($"itemsChosen for Armour: {selectedRows.Count()}");
        };

        npcListView.itemsChosen += (IEnumerable<object> selectedRows) =>
        {
            Debug.Log($"itemsChosen for NPC: {selectedRows.Count()}");
        };

        // actual selection changed logic
        weaponListView.selectionChanged += OnListObjectSelected;
        armourListView.selectionChanged += OnListObjectSelected;
        npcListView.selectionChanged += OnListObjectSelected;

        // Add ListViews to their respective foldouts
        weaponFoldout.Add(weaponListView);
        armourFoldout.Add(armourListView);
        npcFoldout.Add(npcListView);

        // Add the foldouts to the container
        container.Add(weaponFoldout);
        container.Add(armourFoldout);
        container.Add(npcFoldout);
        
        // Set ListView styles
        weaponListView.style.marginTop = 10;
        armourListView.style.marginTop = 10;
        npcListView.style.marginTop = 10;
    }
    
    public static void FilterList(string searchTerm)
    {
        // Filter the items based on the search term for each ListView
        weaponListView.itemsSource = FilterItemsByType(ItemType.Weapon, searchTerm);
        armourListView.itemsSource = FilterItemsByType(ItemType.Armour, searchTerm);
        npcListView.itemsSource = FilterItemsByType(ItemType.NPC, searchTerm);
        
        // Rebuild the ListViews to reflect the changes
        weaponListView.Rebuild();
        armourListView.Rebuild();
        npcListView.Rebuild();
    }
    
    private static List<Item> FilterItemsByType(ItemType itemType, string searchTerm)
    {
        // Get the full item list for the specified item type
        var fullList = GetItemsByType(itemType);

        // If search term is empty, return the full list
        if (string.IsNullOrEmpty(searchTerm))
        {
            return fullList;
        }

        // Filter items by name or ID
        return fullList
            .Where(item => item.generalSettings.itemName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                           || item.generalSettings.itemID.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
    
    private static List<Item> GetItemsByType(ItemType itemType)
    {
        return _itemContainer.items
            .Where(item => item.generalSettings.itemType == itemType)
            .ToList();
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
    
    private void BindItem(VisualElement element, int index, ListView listView)
    {
        // Identify the item based on the ListView it belongs to (weapon, armor, or NPC)
        Item item;
        // set defaults
        item.generalSettings.itemName = null;
        item.generalSettings.prefabPath = null;
        
        // Check which ListView the item is being bound from
        if (listView == weaponListView)
        {
            // Directly get the item from the weaponListView using the local index
            item = (Item)weaponListView.itemsSource[index];
        }
        else if (listView == armourListView)
        {
            // Get the item from the armourListView
            item = (Item)armourListView.itemsSource[index];
        }
        else if (listView == npcListView)
        {
            // Get the item from the npcListView
            item = (Item)npcListView.itemsSource[index];
        }

        var image = element.Q<Image>();
        var label = element.Q<Label>();
        
        label.text = item.generalSettings.itemName;

        // Check if prefab exists and assign the preview image
        if (!string.IsNullOrEmpty(item.generalSettings.prefabPath))
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(item.generalSettings.prefabPath);
            Texture2D prefabTexture = AssetPreview.GetAssetPreview(prefab);

            if (prefabTexture != null)
            {
                Rect spriteRect = new Rect(0, 0, prefabTexture.width, prefabTexture.height);
                Sprite prefabSprite = Sprite.Create(prefabTexture, spriteRect, new Vector2(0.5f, 0.5f));

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
            FilterList(searchField.value);  // Refresh ListViews based on loaded items
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
                prefabPath = null,
                itemType = ItemType.Weapon // Default item type; can be modified later
            },
            weaponStats = new WeaponStats(),
            modifiers = new Modifiers(),
            description = new Description(),
            notes = new Notes(),
            requirements = new ItemRequirements()
        };

        _itemContainer.items.Add(newItem);
        FilterList(searchField.value);  // Refresh ListViews based on new item
        weaponListView.selectedIndex = _itemContainer.items.Count - 1; // Adjust to the relevant ListView
        
        
    }
    
    private void RemoveSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < _itemContainer.items.Count)
        {
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
        
            // Rebuild the ListViews
            FilterList(searchField.value); // Refresh ListViews
            selectedIndex = -1;
        
            // Clear the details pane
            DetailsUI.ClearDetailPane();
        }
    }
}
