using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemRequirementsUI
{
    private IntegerField requiredLevelField;
    private TextField requiredClassField;
    private Toggle requiresTwoHandsToggle;
    private IntegerField strengthRequirementField;
    private IntegerField intelligenceRequirementField;
    private IntegerField agilityRequirementField;
    private IntegerField luckRequirementField;

    public ItemRequirementsUI(VisualElement container)
    {
        requiredLevelField = new IntegerField();
        requiredClassField = new TextField();
        requiresTwoHandsToggle = new Toggle();
        strengthRequirementField = new IntegerField();
        intelligenceRequirementField = new IntegerField();
        agilityRequirementField = new IntegerField();
        luckRequirementField = new IntegerField();
        
        // Item Requirements
        var requirementsFoldout = new Foldout { text = "Item Requirements" };
        UIExtensions.AddLabeledField(requirementsFoldout, "Required Level", requiredLevelField);
        UIExtensions.AddLabeledField(requirementsFoldout, "Required Class", requiredClassField);
        UIExtensions.AddLabeledField(requirementsFoldout, "Requires Two Hands", requiresTwoHandsToggle);
        UIExtensions.AddLabeledField(requirementsFoldout, "Required Strength", strengthRequirementField);
        UIExtensions.AddLabeledField(requirementsFoldout, "Required Intelligence", intelligenceRequirementField);
        UIExtensions.AddLabeledField(requirementsFoldout, "Required Agility", agilityRequirementField);
        UIExtensions.AddLabeledField(requirementsFoldout, "Required Luck", luckRequirementField);
        
        container.Add(requirementsFoldout);
        
        AddFieldUpdateCallbacks();
    }
    
    private void AddFieldUpdateCallbacks()
    {
        requiredLevelField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRequiredLevel(evt.newValue));
        requiredClassField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRequiredClass(evt.newValue));
        requiresTwoHandsToggle.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRequiresTwoHands(evt.newValue));
        strengthRequirementField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateStrengthRequirement(evt.newValue));
        intelligenceRequirementField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateIntelligenceRequirement(evt.newValue));
        agilityRequirementField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAgilityRequirement(evt.newValue));
        luckRequirementField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateLuckRequirement(evt.newValue));
    }

    public void DisplayItemDetails(Item item)
    {
        requiredLevelField.SetValueWithoutNotify(item.requirements.requiredLevel);
        requiredClassField.SetValueWithoutNotify(item.requirements.requiredClass);
        requiresTwoHandsToggle.SetValueWithoutNotify(item.requirements.requiresTwoHands);
        strengthRequirementField.SetValueWithoutNotify(item.requirements.strengthRequirement);
        intelligenceRequirementField.SetValueWithoutNotify(item.requirements.intelligenceRequirement);
        agilityRequirementField.SetValueWithoutNotify(item.requirements.agilityRequirement);
        luckRequirementField.SetValueWithoutNotify(item.requirements.luckRequirement);
    }

    public void ClearDetailPane()
    {
        requiredLevelField.SetValueWithoutNotify(0);
        requiredClassField.SetValueWithoutNotify("");
        requiresTwoHandsToggle.SetValueWithoutNotify(false);
        strengthRequirementField.SetValueWithoutNotify(0);
        intelligenceRequirementField.SetValueWithoutNotify(0);
        agilityRequirementField.SetValueWithoutNotify(0);
        luckRequirementField.SetValueWithoutNotify(0);
    }
}
