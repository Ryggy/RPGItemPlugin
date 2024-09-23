using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DescriptionUI
{
    private TextField shortDescriptionField;
    private TextField detailedDescriptionField;

    public DescriptionUI(VisualElement container)
    {
        shortDescriptionField = new TextField();
        detailedDescriptionField = new TextField();
        detailedDescriptionField.multiline = true;
        
        // Description
        var descriptionFoldout = new Foldout{ text = "Description"};
        UIExtensions.AddLabeledField(descriptionFoldout, "Short Description", shortDescriptionField);
        UIExtensions.AddLabeledField(descriptionFoldout, "Detailed Description", detailedDescriptionField);
        
        container.Add(descriptionFoldout);
        
        AddFieldUpdateCallbacks();
    }
    
    private void AddFieldUpdateCallbacks()
    {
        shortDescriptionField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateShortDescription(evt.newValue));
        detailedDescriptionField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDetailedDescription(evt.newValue));
    }

    public void DisplayItemDetails(Item item)
    {
        shortDescriptionField.SetValueWithoutNotify(item.description.shortDescription);
        detailedDescriptionField.SetValueWithoutNotify(item.description.detailedDescription);
    }

    public void ClearDetailPane()
    {
        shortDescriptionField.SetValueWithoutNotify("");
        detailedDescriptionField.SetValueWithoutNotify("");
    }
}
