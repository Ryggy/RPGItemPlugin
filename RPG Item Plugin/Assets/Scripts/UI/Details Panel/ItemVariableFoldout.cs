using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;

public class ItemVariableFoldout : VisualElement
{
    protected Foldout foldout;
    public string foldoutName;
    protected List<ItemVariable> variableList;
    protected FieldType fieldType;
    public VisualElement foldoutElement { get; private set; }

    // Constructor for the base class, initializes the foldout and the variable list
    public ItemVariableFoldout(string foldoutName, FieldType fieldType, VisualElement container)
    {
        foldoutElement = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                justifyContent = Justify.FlexStart
            }
        };

        this.foldoutName = foldoutName;
        this.fieldType = fieldType;
        variableList = new List<ItemVariable>();
        
        // Create the foldout element and set the title
        foldout = new Foldout();
        foldout.text = foldoutName;
        foldoutElement.Add(foldout);
        
        BuildButtons(foldoutElement);
        
        foldoutElement.style.marginTop = 15;
        foldoutElement.style.marginLeft = 5;
        foldoutElement.style.marginRight = 5;
        foldoutElement.style.marginBottom = 15;
        
        container.Add(foldoutElement);
    }

    // Method to add content (any VisualElement) to the foldout
    public void AddToFoldout(ItemVariable variable)
    {
        variableList.Add(variable);
        foldout.Add(variable);
    }

    // Method to clear all the elements in the foldout
    public void ClearFoldout()
    {
        variableList.Clear();
        foldout.Clear();
    }

    // Method to dynamically update the foldout name
    public void SetFoldoutName(string newName)
    {
        foldoutName = newName;
        foldout.text = foldoutName;
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
        
        var addButton = new Button(() => AddFoldoutField(foldout))
        {
            text = "+"
        };

        var removeButton = new Button(() => RemoveFoldoutField(foldout))
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
    
    private void AddFoldoutField(VisualElement foldout)
    {
        var itemVariable = new ItemVariable(fieldType, foldout);
    }
    
    private void RemoveFoldoutField(VisualElement foldout)
    {
        var count =  foldout.contentContainer.childCount;
        if (count < 1)
        {
            return;
        }
        foldout.contentContainer.RemoveAt(count-1);
    }
    
    public virtual void AddFieldUpdateCallbacks() { }
    public virtual void DisplayItemDetails(Item item){ }
    public virtual void ClearDetailPane(){ }
}
public enum FieldType
{
    TextField,
    IntegerField,
    FloatField,
    Toggle,
    DropdownField,
    ObjectField
}