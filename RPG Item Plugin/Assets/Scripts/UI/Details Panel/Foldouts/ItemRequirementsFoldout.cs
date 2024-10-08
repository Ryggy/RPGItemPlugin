using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemRequirementsFoldout : ItemVariableFoldout
{
    private ItemVariable requiredLevelField;
    private ItemVariable requiredClassField;
    private ItemVariable requiresTwoHandsToggle;
    private ItemVariable strengthRequirementField;
    private ItemVariable intelligenceRequirementField;
    private ItemVariable agilityRequirementField;
    private ItemVariable luckRequirementField;

    public ItemRequirementsFoldout(string foldoutName, FieldType fieldType, VisualElement container) : base(foldoutName, fieldType, container)
    {
        requiredLevelField = new ItemVariable(FieldType.IntegerField, foldout);
        requiredLevelField.UpdateLabelText("Required Level");
        AddToFoldout(requiredLevelField);
        
        requiredClassField = new ItemVariable(FieldType.TextField, foldout);
        requiredClassField.UpdateLabelText("Required Class");
        AddToFoldout(requiredClassField);
        
        requiresTwoHandsToggle = new ItemVariable(FieldType.Toggle, foldout);
        requiresTwoHandsToggle.UpdateLabelText("Requires Two Hands");
        AddToFoldout(requiresTwoHandsToggle);
        
        strengthRequirementField = new ItemVariable(FieldType.IntegerField, foldout);
        strengthRequirementField.UpdateLabelText("Strength Requirement");
        AddToFoldout(strengthRequirementField);
        
        intelligenceRequirementField = new ItemVariable(FieldType.IntegerField, foldout);
        intelligenceRequirementField.UpdateLabelText("Intelligence Requirement");
        AddToFoldout(intelligenceRequirementField);
        
        agilityRequirementField = new ItemVariable(FieldType.IntegerField, foldout);
        agilityRequirementField.UpdateLabelText("Agility Requirement");
        AddToFoldout(agilityRequirementField);
        
        luckRequirementField = new ItemVariable(FieldType.IntegerField, foldout);
        luckRequirementField.UpdateLabelText("Luck Requirement");
        AddToFoldout(luckRequirementField);
        
        AddFieldUpdateCallbacks();
    }
    
    public sealed override void AddFieldUpdateCallbacks()
    {
        ((IntegerField)requiredLevelField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRequiredLevel(evt.newValue));
        ((TextField)requiredClassField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRequiredClass(evt.newValue));
        ((Toggle)requiresTwoHandsToggle.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRequiresTwoHands(evt.newValue));
        ((IntegerField)strengthRequirementField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateStrengthRequirement(evt.newValue));
        ((IntegerField)intelligenceRequirementField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateIntelligenceRequirement(evt.newValue));
        ((IntegerField)agilityRequirementField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAgilityRequirement(evt.newValue));
        ((IntegerField)luckRequirementField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateLuckRequirement(evt.newValue));
    }

    public override void DisplayItemDetails(Item item)
    {
        ((IntegerField)requiredLevelField.field).SetValueWithoutNotify(item.requirements.requiredLevel);
        ((TextField)requiredClassField.field).SetValueWithoutNotify(item.requirements.requiredClass);
        ((Toggle)requiresTwoHandsToggle.field).SetValueWithoutNotify(item.requirements.requiresTwoHands);
        ((IntegerField)strengthRequirementField.field).SetValueWithoutNotify(item.requirements.strengthRequirement);
        ((IntegerField)intelligenceRequirementField.field).SetValueWithoutNotify(item.requirements.intelligenceRequirement);
        ((IntegerField)agilityRequirementField.field).SetValueWithoutNotify(item.requirements.agilityRequirement);
        ((IntegerField)luckRequirementField.field).SetValueWithoutNotify(item.requirements.luckRequirement);
    }

    public override void ClearDetailPane()
    {
        ((IntegerField)requiredLevelField.field).SetValueWithoutNotify(0);
        ((TextField)requiredClassField.field).SetValueWithoutNotify("");
        ((Toggle)requiresTwoHandsToggle.field).SetValueWithoutNotify(false);
        ((IntegerField)strengthRequirementField.field).SetValueWithoutNotify(0);
        ((IntegerField)intelligenceRequirementField.field).SetValueWithoutNotify(0);
        ((IntegerField)agilityRequirementField.field).SetValueWithoutNotify(0);
        ((IntegerField)luckRequirementField.field).SetValueWithoutNotify(0);
    }
}
