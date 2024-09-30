using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DetailsUI
{
    // Fields for editing
    private static GeneralSettingsUI generalSettingsUI;
    private static WeaponStatsUI weaponStatsUI;
    private static ItemRequirementsUI itemRequirementsUI;
    private static ModifiersUI modifiersUI;
    private static DescriptionUI descriptionUI;
    private static NotesUI notesUI;
    public DetailsUI(VisualElement container)
    {
        // Create a two-pane view with the left pane being fixed.
        var detailsSplitView = new TwoPaneSplitView(0, 700, TwoPaneSplitViewOrientation.Horizontal);
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
        
        generalSettingsUI = new GeneralSettingsUI(leftDetailsPane);
        weaponStatsUI = new WeaponStatsUI(leftDetailsPane);
        modifiersUI = new ModifiersUI(leftDetailsPane);
        itemRequirementsUI = new ItemRequirementsUI(rightDetailsPane);
        descriptionUI = new DescriptionUI(rightDetailsPane);
        notesUI = new NotesUI(rightDetailsPane);

        //var detailsSegment = new DetailsSegment(rightDetailsPane);
    }
    
    public static void DisplayItemDetails(Item item)
    {
        generalSettingsUI.DisplayItemDetails(item);
        weaponStatsUI.DisplayItemDetails(item);
        modifiersUI.DisplayItemDetails(item);
        itemRequirementsUI.DisplayItemDetails(item);
        descriptionUI.DisplayItemDetails(item);
        notesUI.DisplayItemDetails(item);
    }
    
    public static void ClearDetailPane()
    {
        generalSettingsUI.ClearDetailPane();
        weaponStatsUI.ClearDetailPane();
        modifiersUI.ClearDetailPane();
        itemRequirementsUI.ClearDetailPane();
        descriptionUI.ClearDetailPane();
        notesUI.ClearDetailPane();
    }
}
