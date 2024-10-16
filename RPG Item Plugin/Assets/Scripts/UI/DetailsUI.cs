using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DetailsUI
{
    // Define foldout templates as a dictionary
    private static Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>> allFoldoutTemplates = new Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>>
    {
        { "General Settings", (container) => new GeneralSettingsFoldOut("General Settings", FieldType.TextField, container) },
        { "Weapon Stats", (container) => new WeaponStatsFoldout("Weapon Stats", FieldType.IntegerField, container) },
        { "Modifiers", (container) => new ModifiersFoldout("Modifiers", FieldType.Toggle, container) },
        { "Item Requirements", (container) => new ItemRequirementsFoldout("Item Requirements", FieldType.TextField, container) },
        { "Description", (container) => new DescriptionFoldout("Description", FieldType.TextField, container) },
        { "Notes", (container) => new NotesFoldout("Notes", FieldType.TextField, container) },
        {"Custom", (container) => null } // Custom foldout will be handled manually
    };
    
    private static Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>> weaponFoldoutTemplate = new Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>>
    {
        { "General Settings", (container) => new GeneralSettingsFoldOut("General Settings", FieldType.TextField, container) },
        { "Weapon Stats", (container) => new WeaponStatsFoldout("Weapon Stats", FieldType.IntegerField, container) },
        { "Modifiers", (container) => new ModifiersFoldout("Modifiers", FieldType.Toggle, container) },
        { "Item Requirements", (container) => new ItemRequirementsFoldout("Item Requirements", FieldType.TextField, container) },
        { "Description", (container) => new DescriptionFoldout("Description", FieldType.TextField, container) },
        { "Notes", (container) => new NotesFoldout("Notes", FieldType.TextField, container) },
    };
    
    private static Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>> armourFoldoutTemplate = new Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>>
    {
        { "General Settings", (container) => new GeneralSettingsFoldOut("General Settings", FieldType.TextField, container) },
        { "Modifiers", (container) => new ModifiersFoldout("Modifiers", FieldType.Toggle, container) },
        { "Item Requirements", (container) => new ItemRequirementsFoldout("Item Requirements", FieldType.TextField, container) },
        { "Description", (container) => new DescriptionFoldout("Description", FieldType.TextField, container) },
        { "Notes", (container) => new NotesFoldout("Notes", FieldType.TextField, container) },
    };
    
    private static Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>> NPCFoldoutTemplate = new Dictionary<string, System.Func<VisualElement, ItemVariableFoldout>>
    {
        { "General Settings", (container) => new GeneralSettingsFoldOut("General Settings", FieldType.TextField, container) },
        { "Description", (container) => new DescriptionFoldout("Description", FieldType.TextField, container) },
        { "Notes", (container) => new NotesFoldout("Notes", FieldType.TextField, container) },
    };

    public static VisualElement leftDetailsPane;
    public static VisualElement rightDetailsPane;
    private static ItemType currentDisplayedItemType = ItemType.None;
    
    // List to track created foldouts
    private static List<ItemVariableFoldout> foldouts = new List<ItemVariableFoldout>();

    public DetailsUI(VisualElement container)
    {
        VisualElement foldoutSelectorContainer = new VisualElement();
        //foldoutSelectorContainer.style.flexDirection = FlexDirection.Row;
        foldoutSelectorContainer.style.alignItems = Align.FlexEnd; 
        container.Add(foldoutSelectorContainer);
        
        // Create a two-pane view with the left pane being fixed.
        var detailsSplitView = new TwoPaneSplitView(0, 600, TwoPaneSplitViewOrientation.Horizontal);
        container.Add(detailsSplitView);

        // Add two child elements to the TwoPaneSplitView
        leftDetailsPane = new ScrollView();
        rightDetailsPane = new ScrollView();
 
        detailsSplitView.Add(leftDetailsPane);
        detailsSplitView.Add(rightDetailsPane);
        
        leftDetailsPane.style.marginTop = 10;
        leftDetailsPane.style.marginLeft = 10;
        leftDetailsPane.style.marginRight = 10;
        leftDetailsPane.style.marginBottom = 10;
        
        rightDetailsPane.style.marginTop = 10;
        rightDetailsPane.style.marginLeft = 10;
        rightDetailsPane.style.marginRight = 10;
        rightDetailsPane.style.marginBottom = 10;
        
        // Initialize the foldouts
        //InitialiseWeaponFoldouts(leftDetailsPane, rightDetailsPane);
        
        AddFoldoutButtons(foldoutSelectorContainer, leftDetailsPane, rightDetailsPane);
        //var detailsSegment = new DetailsSegment(rightDetailsPane);
    }

    private static void InitialiseWeaponFoldouts(VisualElement leftDetailsPane, VisualElement rightDetailsPane)
    {
        leftDetailsPane.Clear();
        rightDetailsPane.Clear();
        
        int index = 0;
        foreach (var template in weaponFoldoutTemplate)
        {
            // Alternate between left and right panes
            var targetPane = (index % 2 == 0) ? leftDetailsPane : rightDetailsPane;

            // Create the foldout using the target pane
            var foldout = template.Value(targetPane);
            foldouts.Add(foldout);

            // Increment index to switch panes on the next iteration
            index++;
        }
    }
    
    private static void InitialiseArmourFoldouts(VisualElement leftDetailsPane, VisualElement rightDetailsPane)
    {
        leftDetailsPane.Clear();
        rightDetailsPane.Clear();
        
        int index = 0;
        foreach (var template in armourFoldoutTemplate)
        {
            // Alternate between left and right panes
            var targetPane = (index % 2 == 0) ? leftDetailsPane : rightDetailsPane;

            // Create the foldout using the target pane
            var foldout = template.Value(targetPane);
            foldouts.Add(foldout);

            // Increment index to switch panes on the next iteration
            index++;
        }
    }
    
    private static void InitialiseNPCFoldouts(VisualElement leftDetailsPane, VisualElement rightDetailsPane)
    {
        leftDetailsPane.Clear();
        rightDetailsPane.Clear();
        
        int index = 0;
        foreach (var template in NPCFoldoutTemplate)
        {
            // Alternate between left and right panes
            var targetPane = (index % 2 == 0) ? leftDetailsPane : rightDetailsPane;

            // Create the foldout using the target pane
            var foldout = template.Value(targetPane);
            foldouts.Add(foldout);

            // Increment index to switch panes on the next iteration
            index++;
        }
    }
    
    private void AddFoldoutButtons(VisualElement container, VisualElement leftDetailsPane, VisualElement rightDetailsPane)
    {
        VisualElement foldoutSelectionContainer = new VisualElement();
        foldoutSelectionContainer.style.flexDirection = FlexDirection.Row;
        foldoutSelectionContainer.style.alignItems = Align.Center;
        container.Add(foldoutSelectionContainer);
        
        // Filter out the "General Settings" foldout from the dropdown choices.
        var filteredTemplates = allFoldoutTemplates.Keys.ToList().Where(key => key != "General Settings").ToList();
        
        // Dropdown to select foldout template
        var foldoutDropdown = new PopupField<string>("Select Foldout Template", filteredTemplates, 0)
        {
            style =
            {
                marginTop = 10, 
            }
            
        };
        // set the min width for the drop down so it doesnt change with different text
        foldoutDropdown.ElementAt(1).style.minWidth = 200;
        foldoutSelectionContainer.Add(foldoutDropdown);
        
        // Create a spacer 
        var spacer = new VisualElement();
        spacer.style.flexGrow = 1; // Allow spacer to take available space
        spacer.style.flexShrink = 1;
        foldoutSelectionContainer.Add(spacer);
        
        // Button to create the selected foldout
        var createFoldoutButton = new Button(() =>
        {
            var selectedTemplate = foldoutDropdown.value;
            
            // Check if foldout with the same name already exists
            if (foldouts.Any(f => f.foldoutName == selectedTemplate))
            {
                Debug.LogWarning($"Foldout '{selectedTemplate}' already exists.");
                return; // Exit if a foldout with this name already exists
            }
            
            if (selectedTemplate == "Custom")
            {
                // Open the ItemVariableFoldoutWindow for creating a custom foldout
                var window = new ItemVariableFoldoutWindow(leftDetailsPane, rightDetailsPane, foldouts);
                window.Show();
            }
            else if (allFoldoutTemplates.TryGetValue(selectedTemplate, out var templateFunc))
            {
                // Create foldout from the selected template
                var targetPane = (foldouts.Count % 2 == 0) ? leftDetailsPane : rightDetailsPane;
                var foldout = templateFunc(targetPane);
                foldouts.Add(foldout); // Add it to the list
            }
        })
        {
            text = "+"
        };
        createFoldoutButton.style.maxHeight = 30;
        foldoutSelectionContainer.Add(createFoldoutButton);
        
        // Add a button to remove the selected foldout
        var removeFoldoutButton = new Button(() =>
        {
            if (foldouts.Any())
            {
                var selectedFoldoutName = foldoutDropdown.value;

                // Find the corresponding foldout in the foldouts list by name
                var selectedFoldout = foldouts.FirstOrDefault(f => f.foldoutName == selectedFoldoutName);

                if (selectedFoldout != null)
                {
                    // Check which pane the foldout is in by examining the parent element
                    if (selectedFoldout.foldoutElement.parent == leftDetailsPane)
                    {
                        leftDetailsPane.Remove(selectedFoldout.foldoutElement);
                    }
                    else if (selectedFoldout.foldoutElement.parent == rightDetailsPane)
                    {
                        rightDetailsPane.Remove(selectedFoldout.foldoutElement);
                    }

                    // Remove the foldout from the list
                    foldouts.Remove(selectedFoldout);

                    // Update the dropdown choices to reflect the remaining foldouts
                    foldoutDropdown.choices = foldouts.Select(f => f.foldoutName).ToList();
                }
            }
        })
        {
            text = "-"
        };
        removeFoldoutButton.style.maxHeight = 30;
        foldoutSelectionContainer.Add(removeFoldoutButton);
    }

    public static void DisplayItemDetails(Item item)
    {
        // Check if the currently displayed item type matches the new item's type
        if (currentDisplayedItemType != item.generalSettings.itemType)
        {
            // Reinitialize the foldouts based on the new item type
            switch (item.generalSettings.itemType)
            {
                case ItemType.Weapon:
                    InitialiseWeaponFoldouts(leftDetailsPane, rightDetailsPane);
                    break;
                case ItemType.Armour:
                    InitialiseArmourFoldouts(leftDetailsPane, rightDetailsPane);
                    break;
                case ItemType.NPC:
                    InitialiseNPCFoldouts(leftDetailsPane, rightDetailsPane);
                    break;
                default:
                    Debug.LogWarning("Unknown ItemType. Foldouts not initialized.");
                    break;
            }

            // Update the current displayed item type to the new one
            currentDisplayedItemType = item.generalSettings.itemType;
        }
        
        foreach (ItemVariableFoldout foldout in foldouts)
        {
            foldout.DisplayItemDetails(item);
        }
    }
    
    public static void ClearDetailPane()
    {
        foreach (var foldout in foldouts)
        {
            foldout.ClearDetailPane();
        }
    }
}