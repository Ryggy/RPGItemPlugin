using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NotesUI 
{
    private TextField developerNotesField;

    public NotesUI(VisualElement container)
    {
        developerNotesField = new TextField();
        developerNotesField.multiline = true;
        
        // Notes
        var notesFoldout = new Foldout{ text = "Notes"};
        UIExtensions.AddLabeledField(notesFoldout, "Developer Notes", developerNotesField);
        
        container.Add(notesFoldout);
        
        AddFieldUpdateCallbacks();
    }
    
    private void AddFieldUpdateCallbacks()
    {
        developerNotesField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDeveloperNotes(evt.newValue));
    }

    public void DisplayItemDetails(Item item)
    {
        developerNotesField.SetValueWithoutNotify(item.notes.developerNotes);
    }

    public void ClearDetailPane()
    {
        developerNotesField.SetValueWithoutNotify("");
    }
}
