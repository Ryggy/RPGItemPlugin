using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneralSettingsFoldOut : ItemVariableFoldout
{
    private ItemVariable itemNameField;
    private ItemVariable itemIDField;
    private ItemVariable prefabField;
    private ItemVariable itemTypeField;
    public GeneralSettingsFoldOut(string foldoutName, FieldType fieldType, VisualElement container) : base(foldoutName, fieldType, container)
    {
         itemNameField = new ItemVariable(FieldType.TextField, foldout);
        itemNameField.UpdateLabelText("Item Name");
        AddToFoldout(itemNameField);
    
        itemIDField = new ItemVariable(FieldType.IntegerField, foldout);
        itemIDField.UpdateLabelText("Item ID");
        AddToFoldout(itemIDField);
    
        prefabField = new ItemVariable(FieldType.ObjectField, foldout);
        prefabField.UpdateLabelText("Prefab");
        AddToFoldout(prefabField);
    
        itemTypeField = new ItemVariable(FieldType.DropdownField, foldout);
        if (itemTypeField.field is DropdownField dropdownField)
        {
            dropdownField.choices = new List<string>(Enum.GetNames(typeof(ItemType)));
        }
        itemTypeField.UpdateLabelText("Item Type"); 
        AddToFoldout(itemTypeField);
        
        AddFieldUpdateCallbacks();
    }
    
    public sealed override void AddFieldUpdateCallbacks()
    {
        // Add change listeners to update the item data when fields are edited
        ((TextField)itemNameField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateItemName(evt.newValue));
        ((IntegerField)itemIDField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateItemID(evt.newValue));
        ((ObjectField)prefabField.field).RegisterValueChangedCallback(evt =>
        {
            var selectedPrefab = evt.newValue as GameObject;
            if (selectedPrefab != null)
            {
                string prefabPath = AssetDatabase.GetAssetPath(selectedPrefab);
                RPGItemCreator.UpdatePrefabPath(prefabPath);
            }
        });
        ((DropdownField)itemTypeField.field).RegisterValueChangedCallback(evt => {
            // Parse the selected string back to ItemType
            if (Enum.TryParse(evt.newValue, out ItemType newItemType))
            {
                RPGItemCreator.UpdateItemType(newItemType);
            }
        });
    }
    
    public override void DisplayItemDetails(Item item)
    {
        // Populate the fields with the item's data
        ((TextField)itemNameField.field).SetValueWithoutNotify(item.generalSettings.itemName);
        ((IntegerField)itemIDField.field).SetValueWithoutNotify(item.generalSettings.itemID);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(item.generalSettings.prefabPath);
        ((ObjectField)prefabField.field).SetValueWithoutNotify(prefab);
        ((DropdownField)itemTypeField.field).SetValueWithoutNotify(item.generalSettings.itemType.ToString());
    }
     
    public override void ClearDetailPane()
    {
        ((TextField)itemNameField.field).SetValueWithoutNotify(null);
        ((IntegerField)itemIDField.field).SetValueWithoutNotify(0);
        ((ObjectField)prefabField.field).SetValueWithoutNotify(null);
        ((DropdownField)itemTypeField.field).SetValueWithoutNotify(ItemType.Weapon.ToString());
    }
}
