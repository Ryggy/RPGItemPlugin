using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DetailsUI
{
    // Define foldout templates as a dictionary for easier management
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
    
    
    // List to track created foldouts
    private static List<ItemVariableFoldout> foldouts = new List<ItemVariableFoldout>();

    public DetailsUI(VisualElement container)
    {
        // Create a two-pane view with the left pane being fixed.
        var detailsSplitView = new TwoPaneSplitView(0, 600, TwoPaneSplitViewOrientation.Horizontal);
        container.Add(detailsSplitView);
        
        // Add two child elements to the TwoPaneSplitView
        VisualElement leftDetailsPane = new ScrollView();
        VisualElement rightDetailsPane = new ScrollView();
 
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
        InitializeFoldouts(leftDetailsPane, rightDetailsPane);
        
        AddFoldoutButtons(container, leftDetailsPane, rightDetailsPane);
        //var detailsSegment = new DetailsSegment(rightDetailsPane);
    }

    private void InitializeFoldouts(VisualElement leftDetailsPane, VisualElement rightDetailsPane)
    {
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
    
    private void AddFoldoutButtons(VisualElement container, VisualElement leftDetailsPane, VisualElement rightDetailsPane)
    {
        // Filter out the "General Settings" foldout from the dropdown choices.
        // Items must have a general settings
        var filteredTemplates = allFoldoutTemplates.Keys.ToList().Where(key => key != "General Settings").ToList();
        
        // Dropdown to select foldout template
        var foldoutDropdown = new PopupField<string>("Select Foldout Template", filteredTemplates, 0)
        {
            style = { marginTop = 10 }
        };
        container.Add(foldoutDropdown);
        
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
            text = "Create Foldout"
        };
        container.Add(createFoldoutButton);
        
        // Filter out the "General Settings" foldout from the dropdown choices.
        // Items must have a general settings
        var filteredFoldouts = foldouts.Where(foldout => foldout.foldoutName != "General Settings").ToList();

        // Add a dropdown to select a foldout to remove, excluding "General Settings"
        var removeFoldoutDropdown = new PopupField<ItemVariableFoldout>("Select Foldout to Remove", filteredFoldouts, 0,
            foldout => foldout?.foldoutName, foldout => foldout?.foldoutName)
        {
            style = { marginTop = 10 }
        };
        container.Add(removeFoldoutDropdown);
        
        // Add a button to remove the selected foldout
        var removeFoldoutButton = new Button(() =>
        {
            if (foldouts.Any())
            {
                var selectedFoldout = removeFoldoutDropdown.value;
                if (selectedFoldout != null)
                {
                    // Check which pane the foldout is in by examining the parent element
                    if (selectedFoldout.foldoutElement.parent == leftDetailsPane)
                    {
                        // If the foldout is in the left pane, remove it from the left pane
                        leftDetailsPane.Remove(selectedFoldout.foldoutElement);
                    }
                    else if (selectedFoldout.foldoutElement.parent == rightDetailsPane)
                    {
                        // If the foldout is in the right pane, remove it from the right pane
                        rightDetailsPane.Remove(selectedFoldout.foldoutElement);
                    }

                    // Remove the foldout from the list
                    foldouts.Remove(selectedFoldout);

                    // Update the dropdown choices to reflect the remaining foldouts
                    removeFoldoutDropdown.choices = foldouts;
                }
            }
        })
        {
            text = "Remove Selected Foldout"
        };
        container.Add(removeFoldoutButton);
    }

    public static void DisplayItemDetails(Item item)
    {
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