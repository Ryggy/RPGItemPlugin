using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIExtensions 
{
    /// <summary>
    /// Displays or hides a VisualElement based on the 'enabled' parameter.
    /// </summary>
    /// <param name="element">The VisualElement to display or hide.</param>
    /// <param name="enabled">A boolean value indicating whether to display the element.</param>
    public static void Display(this VisualElement element, bool enabled)
    {
        if(element == null) return;
        element.style.display = enabled ? DisplayStyle.Flex : DisplayStyle.None;
    }

    /// <summary>
    /// Checks if a VisualElement is currently displayed as Flex.
    /// </summary>
    /// <param name="element">The VisualElement to check.</param>
    /// <returns>True if the element is displayed as Flex, false otherwise.</returns>
    public static bool IsDisplayFlex(this VisualElement element) =>
        element != null && element.style.display == DisplayStyle.Flex;
    
    /// <summary>
    /// Adds a labeled field to a container with specified label text and field.
    /// </summary>
    /// <param name="container">The container VisualElement to add the labeled field to.</param>
    /// <param name="labelText">The text to display as the label for the field.</param>
    /// <param name="field">The VisualElement field to add.</param>
    public static void AddLabeledField(VisualElement container, string labelText, VisualElement field, bool fixedLabelWidth = true)
    {
        // Create a row container
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.FlexStart; // Aligns items to the start
        row.style.alignItems = Align.Center; // Center vertically


        // Create the label
        var label = new Label(labelText);
        if(fixedLabelWidth) { label.style.width = 150;}
        
        row.Add(label);
        
        // Create a spacer to push the input field to the end
        var spacer = new VisualElement();
        spacer.style.flexGrow = 1; // Allow spacer to take available space
        spacer.style.flexShrink = 1;
        row.Add(spacer);
        
        field.style.width = Length.Percent(60);
        
        row.Add(field);
        container.Add(row);
    }
    
    public static void ApplyHeaderStyle(VisualElement element)
    {
        element.style.fontSize = 16;
        element.style.unityFontStyleAndWeight = FontStyle.Bold;
        element.style.marginBottom = 10;  
        element.style.marginTop = 5;
    }
    
    public static void ApplyDefaultTextStyle(VisualElement element)
    {
        element.style.fontSize = 14;
        element.style.unityFontStyleAndWeight = FontStyle.Normal;
        element.style.marginBottom = 5; 
    }
}