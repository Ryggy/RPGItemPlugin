using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DetailsSegment
{
    private List<VisualElement> foldoutFields;
    private Foldout _foldout;
    public DetailsSegment(VisualElement container)
    {
        // Create a row container
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.FlexStart;
        
        _foldout = new Foldout { text = "Foldout"};
        
        _foldout.style.flexGrow = 1;
        _foldout.contentContainer.style.marginTop = 10;
        _foldout.contentContainer.style.marginBottom = 10;
        row.Add(_foldout);
        
        // // Create a spacer to push the buttons to the end
        // var spacer = new VisualElement();
        // spacer.style.flexGrow = 1; // Allow spacer to take available space
        // spacer.style.flexShrink = 1;
        // row.Add(spacer);
        
        BuildButtons(row);
        
        row.style.marginTop = 15;
        row.style.marginLeft = 5;
        row.style.marginRight = 5;
        row.style.marginBottom = 15;
        container.Add(row);
    }
    
    private void BuildButtons(VisualElement container)
    {
        // Add and Remove field buttons
        var buttonContainer = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                marginTop = 10,
                alignItems = Align.FlexStart
            }
        };
        
        var addButton = new Button(() => AddFoldoutField(_foldout))
        {
            text = "+"
        };

        var removeButton = new Button(() => RemoveFoldoutField(_foldout))
        {
            text = "-"
        };
        
        buttonContainer.Add(addButton);
        buttonContainer.Add(removeButton);
        
        buttonContainer.style.marginTop = 10;
        buttonContainer.style.marginLeft = 10;
        buttonContainer.style.marginRight = 10;
        buttonContainer.style.marginBottom = 10;
        
        container.Add(buttonContainer);
    }

    public virtual void DisplayItemDetails(Item item)
    {
        
    }

    public virtual void ClearDetailPane()
    {
        
    }

    protected virtual void AddFieldUpdateCallbacks()
    {
        
    }

    private void AddFoldoutField(VisualElement foldout)
    {
        var dropDownChoices = new List<string>(Enum.GetNames(typeof(FieldType)));
        var dropDown = new DropdownField("");
        dropDown.choices = dropDownChoices;
        UIExtensions.AddLabeledField(foldout, "Select Field Type", dropDown);
        
        dropDown.RegisterValueChangedCallback(evt => {
            // Parse the selected string back to ItemType
            if (Enum.TryParse(evt.newValue, out FieldType newFieldType))
            {
                AddFieldType(newFieldType, dropDown);
                dropDown.parent.Remove(dropDown);
            }
        });
    }

    private void AddFieldType(FieldType fieldType, VisualElement dropDown)
    {
        switch (fieldType)
        {
            case FieldType.TextField:
                var textField = new TextField();
                textField.style.width = Length.Percent(60);
                dropDown.parent.Add(textField);
                break;
            case FieldType.IntegerField:
                var integerField = new IntegerField();
                integerField.style.width = Length.Percent(60);
                dropDown.parent.Add(integerField);
                break;
            case FieldType.FloatField:
                var floatField = new FloatField();
                floatField.style.width = Length.Percent(60);
                dropDown.parent.Add(floatField);
                break;
            case FieldType.Toggle:
                var toggleField = new Toggle();
                toggleField.style.width = Length.Percent(60);
                dropDown.parent.Add(toggleField);
                break;
            case FieldType.DropdownField:
                var dropDownField = new DropdownField();
                dropDownField.style.width = Length.Percent(60);
                dropDown.parent.Add(dropDownField);
                break;
        }
    }
    
    private void RemoveFoldoutField(VisualElement foldout)
    {
        var count =  foldout.contentContainer.childCount;
        foldout.contentContainer.RemoveAt(count-1);
    }
}
// public enum FieldType
// {
//     TextField,
//     IntegerField,
//     FloatField,
//     Toggle,
//     DropdownField
// }
