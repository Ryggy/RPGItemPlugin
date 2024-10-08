using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NotesFoldout : ItemVariableFoldout
{
    private ItemVariable developerNotesField;

    
    public NotesFoldout(string foldoutName, FieldType fieldType, VisualElement container) : base(foldoutName,
        fieldType, container)
    {
        developerNotesField = new ItemVariable(FieldType.TextField, foldout);
        developerNotesField.UpdateLabelText("Developer Notes");
        AddToFoldout(developerNotesField);
        
        AddFieldUpdateCallbacks();
    }
    
    public sealed override void AddFieldUpdateCallbacks()
    {
        ((TextField)developerNotesField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDeveloperNotes(evt.newValue));
    }

    public override void DisplayItemDetails(Item item)
    {
        ((TextField)developerNotesField.field).SetValueWithoutNotify(item.notes.developerNotes);
    }

    public override void ClearDetailPane()
    {
        ((TextField)developerNotesField.field).SetValueWithoutNotify("");
    }
}   
