using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemVariableFoldoutWindow : EditorWindow
{
    private string foldoutName = "New Foldout";
    private FieldType selectedFieldType = FieldType.TextField;
    private DetailsPaneSide selectedSide = DetailsPaneSide.Left;
    private VisualElement container;

    private VisualElement leftDetailsPane;
    private VisualElement rightDetailsPane;
    private List<ItemVariableFoldout> foldouts;
    public ItemVariableFoldoutWindow(VisualElement leftDetailsPane, VisualElement rightDetailsPane, List<ItemVariableFoldout> foldouts)
    {
        this.leftDetailsPane = leftDetailsPane;
        this.rightDetailsPane = rightDetailsPane;
        this.foldouts = foldouts;
    }

    public void CreateGUI()
    {
        // Create a TextField for the foldout name input
        var foldoutNameField = new TextField("Foldout Name");
        foldoutNameField.value = foldoutName;
        foldoutNameField.RegisterValueChangedCallback(evt =>
        {
            foldoutName = evt.newValue;
        });
        rootVisualElement.Add(foldoutNameField);

        // Create a DropdownField for the FieldType selection
        var fieldTypeDropdown = new EnumField("Field Type", selectedFieldType);
        fieldTypeDropdown.Init(FieldType.TextField);
        fieldTypeDropdown.RegisterValueChangedCallback(evt =>
        {
            selectedFieldType = (FieldType)evt.newValue;
        });
        rootVisualElement.Add(fieldTypeDropdown);
        
        // Create a DropdownField for the pane side selection
        var selectedSideDropdown = new EnumField("Panel Side", selectedSide);
        selectedSideDropdown.Init(DetailsPaneSide.Left);
        selectedSideDropdown.RegisterValueChangedCallback(evt =>
        {
            selectedSide = (DetailsPaneSide)evt.newValue;
        });
        rootVisualElement.Add(selectedSideDropdown);
        
        // Add a button to confirm and create the ItemVariableFoldout
        var confirmButton = new Button(OnConfirmButtonClick) { text = "Confirm" };
        rootVisualElement.Add(confirmButton);
    }

    public new void Show()
    {
        var wnd = GetWindow<ItemVariableFoldoutWindow>();
        wnd.titleContent = new GUIContent("Create Foldout");
        wnd.minSize = new Vector2(300, 200);
        wnd.maxSize = new Vector2(300, 200);
        wnd.ShowUtility();
    }
    
    private void OnConfirmButtonClick()
    {
        ItemVariableFoldout foldout;
        if (selectedSide == DetailsPaneSide.Left)
        {
            // Create a new ItemVariableFoldout and add it to the container
           foldout =  new ItemVariableFoldout(foldoutName, selectedFieldType, leftDetailsPane);
        }
        else
        {
            foldout = new ItemVariableFoldout(foldoutName, selectedFieldType, rightDetailsPane);
        }

        foldouts.Add(foldout);
        // Close the editor window
        Close();
    }
}

public enum DetailsPaneSide
{
    Left,
    Right
}