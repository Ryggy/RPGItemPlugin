using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemVariable : VisualElement
{
    private VisualElement container;
    private Label label;
    private string labelText = "Variable Name";
    public VisualElement field;
    private bool fixedLabelWidth = true;
    
    public ItemVariable(FieldType fieldType, VisualElement container)
    {
        // Create a row container
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.FlexStart; // Aligns items to the start
        row.style.alignItems = Align.Center; // Center vertically
        
        // Create the label
        label = new Label(labelText);
        if(fixedLabelWidth) { label.style.width = 150;}
        row.Add(label);
        
        // Add event listener for detecting mouse down and handling double-clicks
        label.RegisterCallback<MouseDownEvent>(evt =>
        {
            if (evt.clickCount == 2) // MouseDownEvent provides clickCount
            {
                OnDoubleClick(label);  // Call OnDoubleClick if double-click detected
            }
        });
        
        // Create a spacer to push the input field to the end
        var spacer = new VisualElement();
        spacer.style.flexGrow = 1; // Allow spacer to take available space
        spacer.style.flexShrink = 1;
        row.Add(spacer);

        field = GetFieldVisualElement(fieldType);
        field.style.width = Length.Percent(60);
        
        row.Add(field);
        container.Add(row);
    }
    
    private static void OnDoubleClick(Label ctx)
    {
        var textField = new TextField();
        textField.value = ctx.text;
        ctx.Display(false);
        ctx.parent.hierarchy.Insert(0, textField);
        textField.style.maxWidth = 150;

        textField.RegisterCallback<FocusOutEvent>(evt =>
        {
            OnDeselected(ctx, textField);
        });
    }

    private static void OnDeselected(Label ctx, TextField ctxName)
    {
        ctx.text = ctxName.text;
        ctxName.parent.Remove(ctxName);
        ctx.Display(true);
    }

    
    // Add a method to update the label text after object creation
    public void UpdateLabelText(string newText)
    {
        label.text = newText; // Update the label's text
    }
    
    private VisualElement GetFieldVisualElement(FieldType fieldType)
    {
        VisualElement inputField = null;
        switch (fieldType)
        {
            case FieldType.TextField:
                var textField = new TextField();
                textField.style.width = Length.Percent(60);
                inputField = textField;
                break;
            case FieldType.IntegerField:
                var integerField = new IntegerField();
                integerField.style.width = Length.Percent(60);
                inputField =  integerField;
                break;
            case FieldType.FloatField:
                var floatField = new FloatField();
                floatField.style.width = Length.Percent(60);
                inputField =  floatField;
                break;
            case FieldType.Toggle:
                var toggleField = new Toggle();
                toggleField.style.width = Length.Percent(60);
                inputField =  toggleField;
                break;
            case FieldType.DropdownField:
                var dropDownField = new DropdownField();
                dropDownField.style.width = Length.Percent(60);
                inputField =  dropDownField;
                break;
            case FieldType.ObjectField:
                var objectField = new ObjectField();
                objectField.style.width = Length.Percent(60);
                inputField = objectField;
                break;
        }
        return inputField;
    }
}
