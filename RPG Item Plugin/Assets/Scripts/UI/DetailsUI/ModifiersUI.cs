using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ModifiersUI
{
    private IntegerField strengthField;
    private IntegerField intelligenceField;
    private IntegerField agilityField;
    private IntegerField luckField;
    private IntegerField maxHealthField;
    private IntegerField maxManaField;
    private FloatField moveSpeedField;
    private FloatField attackDamageField;
    private FloatField critChanceField;
    private FloatField critMultiplierField;
    private FloatField damageReductionField;
    private FloatField experienceMultiplierField;

    public ModifiersUI(VisualElement container)
    {
        strengthField = new IntegerField();
        intelligenceField = new IntegerField();
        agilityField = new IntegerField();
        luckField = new IntegerField();
        maxHealthField = new IntegerField();
        maxManaField = new IntegerField();
        moveSpeedField = new FloatField();
        attackDamageField = new FloatField();
        critChanceField = new FloatField();
        critMultiplierField = new FloatField();
        damageReductionField = new FloatField();
        experienceMultiplierField = new FloatField();
        
        // Modifiers
        var modifiersFoldout = new Foldout { text = "Modifiers" };
        UIExtensions.AddLabeledField(modifiersFoldout, "Strength", strengthField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Intelligence", intelligenceField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Agility", agilityField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Luck", luckField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Max Health", maxHealthField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Max Mana", maxManaField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Move Speed", moveSpeedField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Attack Damage", attackDamageField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Crit Chance", critChanceField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Crit Multiplier", critMultiplierField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Damage Reduction", damageReductionField);
        UIExtensions.AddLabeledField(modifiersFoldout, "Experience Multiplier", experienceMultiplierField);
        
        container.Add(modifiersFoldout);
        
        AddFieldUpdateCallbacks();
    }

    private void AddFieldUpdateCallbacks()
    {
        strengthField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateStrength(evt.newValue));
        intelligenceField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateIntelligence(evt.newValue));
        agilityField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAgility(evt.newValue));
        luckField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateLuck(evt.newValue));
        maxHealthField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateMaxHealth(evt.newValue));
        maxManaField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateMaxMana(evt.newValue));
        moveSpeedField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateMoveSpeed(evt.newValue));
        attackDamageField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAttackDamage(evt.newValue));
        critChanceField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateCritChance(evt.newValue));
        critMultiplierField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateCritMultiplier(evt.newValue));
        damageReductionField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDamageReduction(evt.newValue));
        experienceMultiplierField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateExperienceMultiplier(evt.newValue));
    }

    public void DisplayItemDetails(Item item)
    {
        strengthField.SetValueWithoutNotify(item.modifiers.strength);
        intelligenceField.SetValueWithoutNotify(item.modifiers.intelligence);
        agilityField.SetValueWithoutNotify(item.modifiers.agility);
        luckField.SetValueWithoutNotify(item.modifiers.luck);
        maxHealthField.SetValueWithoutNotify(item.modifiers.maxHealth);
        maxManaField.SetValueWithoutNotify(item.modifiers.maxMana);
        moveSpeedField.SetValueWithoutNotify(item.modifiers.moveSpeed);
        attackDamageField.SetValueWithoutNotify(item.modifiers.attackDamage);
        critChanceField.SetValueWithoutNotify(item.modifiers.critChance);
        critMultiplierField.SetValueWithoutNotify(item.modifiers.critMultiplier);
        damageReductionField.SetValueWithoutNotify(item.modifiers.damageReduction);
        experienceMultiplierField.SetValueWithoutNotify(item.modifiers.experienceMultiplier);
    }

    public void ClearDetailPane()
    {
        strengthField.SetValueWithoutNotify(0);
        intelligenceField.SetValueWithoutNotify(0);
        agilityField.SetValueWithoutNotify(0);
        luckField.SetValueWithoutNotify(0);
        maxHealthField.SetValueWithoutNotify(0);
        maxManaField.SetValueWithoutNotify(0);
        moveSpeedField.SetValueWithoutNotify(0f);
        attackDamageField.SetValueWithoutNotify(0f);
        critChanceField.SetValueWithoutNotify(0f);
        critMultiplierField.SetValueWithoutNotify(0f);
        damageReductionField.SetValueWithoutNotify(0f);
        experienceMultiplierField.SetValueWithoutNotify(0f);
    }
}
