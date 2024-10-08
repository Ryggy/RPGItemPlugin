using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DescriptionFoldout : ItemVariableFoldout
{
    private ItemVariable shortDescriptionField;
    private ItemVariable detailedDescriptionField;

    public DescriptionFoldout(string foldoutName, FieldType fieldType, VisualElement container) : base(foldoutName, fieldType, container)
    {
        shortDescriptionField = new ItemVariable(FieldType.TextField, foldout);
        shortDescriptionField.UpdateLabelText("Short Description");
        AddToFoldout(shortDescriptionField);
        
        detailedDescriptionField = new ItemVariable(FieldType.TextField, foldout);
        detailedDescriptionField.UpdateLabelText("Detailed Description");
        AddToFoldout(detailedDescriptionField);
        
        AddFieldUpdateCallbacks();
    }
    
    public sealed override void AddFieldUpdateCallbacks()
    {
        ((TextField)shortDescriptionField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateShortDescription(evt.newValue));
        ((TextField)detailedDescriptionField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDetailedDescription(evt.newValue));
    }
    
    public override void DisplayItemDetails(Item item)
    {
        ((TextField)shortDescriptionField.field).SetValueWithoutNotify(item.description.shortDescription);
        ((TextField)detailedDescriptionField.field).SetValueWithoutNotify(item.description.detailedDescription);
    }
     
    public override void ClearDetailPane()
    {
        ((TextField)shortDescriptionField.field).SetValueWithoutNotify("");
        ((TextField)detailedDescriptionField.field).SetValueWithoutNotify("");
    }
}
