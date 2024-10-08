using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ModifiersFoldout : ItemVariableFoldout
{
    private ItemVariable strengthField;
    private ItemVariable intelligenceField;
    private ItemVariable agilityField;
    private ItemVariable luckField;
    private ItemVariable maxHealthField;
    private ItemVariable maxManaField;
    private ItemVariable moveSpeedField;
    private ItemVariable attackDamageField;
    private ItemVariable critChanceField;
    private ItemVariable critMultiplierField;
    private ItemVariable damageReductionField;
    private ItemVariable experienceMultiplierField;

    public ModifiersFoldout(string foldoutName, FieldType fieldType, VisualElement container) : base(foldoutName,
        fieldType, container)
    {
        strengthField = new ItemVariable(FieldType.IntegerField, foldout);
        strengthField.UpdateLabelText("Strength");
        AddToFoldout(strengthField);

        intelligenceField = new ItemVariable(FieldType.IntegerField, foldout);
        intelligenceField.UpdateLabelText("Intelligence");
        AddToFoldout(intelligenceField);

        agilityField = new ItemVariable(FieldType.IntegerField, foldout);
        agilityField.UpdateLabelText("Agility");
        AddToFoldout(agilityField);

        luckField = new ItemVariable(FieldType.IntegerField, foldout);
        luckField.UpdateLabelText("Luck");
        AddToFoldout(luckField);

        maxHealthField = new ItemVariable(FieldType.IntegerField, foldout);
        maxHealthField.UpdateLabelText("Max Health");
        AddToFoldout(maxHealthField);

        maxManaField = new ItemVariable(FieldType.IntegerField, foldout);
        maxManaField.UpdateLabelText("Max Mana");
        AddToFoldout(maxManaField);

        moveSpeedField = new ItemVariable(FieldType.FloatField, foldout);
        moveSpeedField.UpdateLabelText("Move Speed");
        AddToFoldout(moveSpeedField);

        attackDamageField = new ItemVariable(FieldType.FloatField, foldout);
        attackDamageField.UpdateLabelText("Attack Damage");
        AddToFoldout(attackDamageField);

        critChanceField = new ItemVariable(FieldType.FloatField, foldout);
        critChanceField.UpdateLabelText("Critical Chance");
        AddToFoldout(critChanceField);

        critMultiplierField = new ItemVariable(FieldType.FloatField, foldout);
        critMultiplierField.UpdateLabelText("Critical Multiplier");
        AddToFoldout(critMultiplierField);

        damageReductionField = new ItemVariable(FieldType.FloatField, foldout);
        damageReductionField.UpdateLabelText("Damage Reduction");
        AddToFoldout(damageReductionField);

        experienceMultiplierField = new ItemVariable(FieldType.FloatField, foldout);
        experienceMultiplierField.UpdateLabelText("Experience Multiplier");
        AddToFoldout(experienceMultiplierField);

        AddFieldUpdateCallbacks();
    }
    
    public sealed override void AddFieldUpdateCallbacks()
    {
        ((IntegerField)strengthField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateStrength(evt.newValue));
        ((IntegerField)intelligenceField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateIntelligence(evt.newValue));
        ((IntegerField)agilityField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAgility(evt.newValue));
        ((IntegerField)luckField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateLuck(evt.newValue));
        ((IntegerField)maxHealthField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateMaxHealth(evt.newValue));
        ((IntegerField)maxManaField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateMaxMana(evt.newValue));
        ((FloatField)moveSpeedField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateMoveSpeed(evt.newValue));
        ((FloatField)attackDamageField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAttackDamage(evt.newValue));
        ((FloatField)critChanceField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateCritChance(evt.newValue));
        ((FloatField)critMultiplierField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateCritMultiplier(evt.newValue));
        ((FloatField)damageReductionField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDamageReduction(evt.newValue));
        ((FloatField)experienceMultiplierField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateExperienceMultiplier(evt.newValue));
    }
    
public override void DisplayItemDetails(Item item)
{
    ((IntegerField)strengthField.field).SetValueWithoutNotify((int)item.modifiers.strength);
    ((IntegerField)intelligenceField.field).SetValueWithoutNotify((int)item.modifiers.intelligence);
    ((IntegerField)agilityField.field).SetValueWithoutNotify((int)item.modifiers.agility);
    ((IntegerField)luckField.field).SetValueWithoutNotify((int)item.modifiers.luck);
    ((IntegerField)maxHealthField.field).SetValueWithoutNotify((int)item.modifiers.maxHealth);
    ((IntegerField)maxManaField.field).SetValueWithoutNotify((int)item.modifiers.maxMana);
    ((FloatField)moveSpeedField.field).SetValueWithoutNotify(item.modifiers.moveSpeed);
    ((FloatField)attackDamageField.field).SetValueWithoutNotify(item.modifiers.attackDamage);
    ((FloatField)critChanceField.field).SetValueWithoutNotify(item.modifiers.critChance);
    ((FloatField)critMultiplierField.field).SetValueWithoutNotify(item.modifiers.critMultiplier);
    ((FloatField)damageReductionField.field).SetValueWithoutNotify(item.modifiers.damageReduction);
    ((FloatField)experienceMultiplierField.field).SetValueWithoutNotify(item.modifiers.experienceMultiplier);
}

public override void ClearDetailPane()
{
    ((IntegerField)strengthField.field).SetValueWithoutNotify(0);
    ((IntegerField)intelligenceField.field).SetValueWithoutNotify(0);
    ((IntegerField)agilityField.field).SetValueWithoutNotify(0);
    ((IntegerField)luckField.field).SetValueWithoutNotify(0);
    ((IntegerField)maxHealthField.field).SetValueWithoutNotify(0);
    ((IntegerField)maxManaField.field).SetValueWithoutNotify(0);
    ((FloatField)moveSpeedField.field).SetValueWithoutNotify(0f);
    ((FloatField)attackDamageField.field).SetValueWithoutNotify(0f);
    ((FloatField)critChanceField.field).SetValueWithoutNotify(0f);
    ((FloatField)critMultiplierField.field).SetValueWithoutNotify(0f);
    ((FloatField)damageReductionField.field).SetValueWithoutNotify(0f);
    ((FloatField)experienceMultiplierField.field).SetValueWithoutNotify(0f);
}
}
