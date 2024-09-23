using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class GeneralSettingsUI
{
    private TextField itemNameField;
    private IntegerField itemIDField;
    private ObjectField prefabField;
    private DropdownField itemTypeField;

    public GeneralSettingsUI(VisualElement container)
    {
        itemNameField = new TextField();
        itemIDField = new IntegerField();
        prefabField = new ObjectField()
        {
            objectType = typeof(GameObject),
            allowSceneObjects = false
        };
        // Create a list of choices from the enum
        var itemTypeChoices = new List<string>(Enum.GetNames(typeof(ItemType)));
        itemTypeField = new DropdownField("", itemTypeChoices, 0);
        
        // General Settings
        var generalSettingsFoldout = new Foldout { text = "General Settings" };
        UIExtensions.AddLabeledField(generalSettingsFoldout, "Item Name", itemNameField);
        UIExtensions.AddLabeledField(generalSettingsFoldout, "Item ID", itemIDField);
        UIExtensions.AddLabeledField(generalSettingsFoldout, "Prefab", prefabField);
        UIExtensions.AddLabeledField(generalSettingsFoldout, "Item Type", itemTypeField);
        
        container.Add(generalSettingsFoldout);

        AddFieldUpdateCallbacks();
    }
    
     private void AddFieldUpdateCallbacks()
    {
        // Add change listeners to update the item data when fields are edited
        itemNameField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateItemName(evt.newValue));
        itemIDField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateItemID(evt.newValue));
        prefabField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdatePrefab(evt.newValue as GameObject));
        itemTypeField.RegisterValueChangedCallback(evt => {
            // Parse the selected string back to ItemType
            if (Enum.TryParse(evt.newValue, out ItemType newItemType))
            {
                RPGItemCreator.UpdateItemType(newItemType);
            }
        });
    }
     
     public void DisplayItemDetails(Item item)
    {
        // Populate the fields with the item's data
        itemNameField.SetValueWithoutNotify(item.generalSettings.itemName);
        itemIDField.SetValueWithoutNotify(item.generalSettings.itemID);
        prefabField.SetValueWithoutNotify(item.generalSettings.prefab);
        itemTypeField.SetValueWithoutNotify(item.generalSettings.itemType.ToString());
    }
     
    public void ClearDetailPane()
    {
        itemNameField.SetValueWithoutNotify("");
        itemIDField.SetValueWithoutNotify(0);
        prefabField.SetValueWithoutNotify(null);
        itemTypeField.SetValueWithoutNotify(null);
    }
    
    
}
