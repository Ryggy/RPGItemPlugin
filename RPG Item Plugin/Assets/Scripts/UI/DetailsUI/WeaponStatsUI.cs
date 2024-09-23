using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponStatsUI
{
    // Weapon Stats
    private IntegerField stackSizeField;
    private IntegerField rarityField;
    private IntegerField attackPowerField;
    private FloatField attackSpeedField;
    private FloatField rangeField;
    private FloatField durabilityField;

    public WeaponStatsUI(VisualElement container)
    {
        stackSizeField = new IntegerField();
        rarityField = new IntegerField();
        attackPowerField = new IntegerField();
        attackSpeedField = new FloatField();
        rangeField = new FloatField();
        durabilityField = new FloatField();
        
        // Weapon Stats
        var weaponStatsFoldout = new Foldout { text = "Weapon Stats" };
        UIExtensions.AddLabeledField(weaponStatsFoldout, "Stack Size", stackSizeField);
        UIExtensions.AddLabeledField(weaponStatsFoldout, "Rarity", rarityField);
        UIExtensions.AddLabeledField(weaponStatsFoldout, "Attack Power", attackPowerField);
        UIExtensions.AddLabeledField(weaponStatsFoldout, "Attack Speed", attackSpeedField);
        UIExtensions.AddLabeledField(weaponStatsFoldout, "Range", rangeField);
        UIExtensions.AddLabeledField(weaponStatsFoldout, "Durability", durabilityField);
        
        container.Add(weaponStatsFoldout);
        
        AddFieldUpdateCallbacks();
    }

    private void AddFieldUpdateCallbacks()
    {
        stackSizeField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateStackSize(evt.newValue));
        rarityField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRarity(evt.newValue));
        attackPowerField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAttackPower(evt.newValue));
        attackSpeedField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAttackSpeed(evt.newValue));
        rangeField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRange(evt.newValue));
        durabilityField.RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDurability(evt.newValue));
    }

    public void DisplayItemDetails(Item item)
    {
        stackSizeField.SetValueWithoutNotify(item.weaponStats.stackSize);
        rarityField.SetValueWithoutNotify(item.weaponStats.rarity);
        attackPowerField.SetValueWithoutNotify(item.weaponStats.attackPower);
        attackSpeedField.SetValueWithoutNotify(item.weaponStats.attackSpeed);
        rangeField.SetValueWithoutNotify(item.weaponStats.range);
        durabilityField.SetValueWithoutNotify(item.weaponStats.durability);
    }

    public void ClearDetailPane()
    {
        stackSizeField.SetValueWithoutNotify(0);
        rarityField.SetValueWithoutNotify(0);
        attackPowerField.SetValueWithoutNotify(0);
        attackSpeedField.SetValueWithoutNotify(0f);
        rangeField.SetValueWithoutNotify(0f);
        durabilityField.SetValueWithoutNotify(0f);
    }
}
