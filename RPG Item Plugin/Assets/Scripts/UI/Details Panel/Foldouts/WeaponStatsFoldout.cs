using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponStatsFoldout : ItemVariableFoldout
{
    private ItemVariable stackSizeField;
    private ItemVariable rarityField;
    private ItemVariable attackPowerField;
    private ItemVariable attackSpeedField;
    private ItemVariable rangeField;
    private ItemVariable durabilityField;
    
    public WeaponStatsFoldout(string foldoutName, FieldType fieldType, VisualElement container) : base(foldoutName,
        fieldType, container)
    {
        stackSizeField = new ItemVariable(FieldType.IntegerField, foldout);
        stackSizeField.UpdateLabelText("Stack Size");
        AddToFoldout(stackSizeField);

        rarityField = new ItemVariable(FieldType.IntegerField, foldout);
        rarityField.UpdateLabelText("Rarity");
        AddToFoldout(rarityField);

        attackPowerField = new ItemVariable(FieldType.IntegerField, foldout);
        attackPowerField.UpdateLabelText("Attack Power");
        AddToFoldout(attackPowerField);

        attackSpeedField = new ItemVariable(FieldType.FloatField, foldout);
        attackSpeedField.UpdateLabelText("Attack Speed");
        AddToFoldout(attackSpeedField);

        rangeField = new ItemVariable(FieldType.FloatField, foldout);
        rangeField.UpdateLabelText("Range");
        AddToFoldout(rangeField);

        durabilityField = new ItemVariable(FieldType.FloatField, foldout);
        durabilityField.UpdateLabelText("Durability");
        AddToFoldout(durabilityField);

        AddFieldUpdateCallbacks();
    }
    
    public sealed override void AddFieldUpdateCallbacks()
    {
        ((IntegerField)stackSizeField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateStackSize(evt.newValue));
        ((IntegerField)rarityField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRarity(evt.newValue));
        ((IntegerField)attackPowerField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAttackPower(evt.newValue));
        ((FloatField)attackSpeedField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateAttackSpeed(evt.newValue));
        ((FloatField)rangeField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateRange(evt.newValue));
        ((FloatField)durabilityField.field).RegisterValueChangedCallback(evt => RPGItemCreator.UpdateDurability(evt.newValue));
    }

    public override void DisplayItemDetails(Item item)
    {
        ((IntegerField)stackSizeField.field).SetValueWithoutNotify(item.weaponStats.stackSize);
        ((IntegerField)rarityField.field).SetValueWithoutNotify(item.weaponStats.rarity);
        ((IntegerField)attackPowerField.field).SetValueWithoutNotify(item.weaponStats.attackPower);
        ((FloatField)attackSpeedField.field).SetValueWithoutNotify(item.weaponStats.attackSpeed);
        ((FloatField)rangeField.field).SetValueWithoutNotify(item.weaponStats.range);
        ((FloatField)durabilityField.field).SetValueWithoutNotify(item.weaponStats.durability);
    }

    public override void ClearDetailPane()
    {
        ((IntegerField)stackSizeField.field).SetValueWithoutNotify(0);
        ((IntegerField)rarityField.field).SetValueWithoutNotify(0);
        ((IntegerField)attackPowerField.field).SetValueWithoutNotify(0);
        ((FloatField)attackSpeedField.field).SetValueWithoutNotify(0f);
        ((FloatField)rangeField.field).SetValueWithoutNotify(0f);
        ((FloatField)durabilityField.field).SetValueWithoutNotify(0f);
    }
}
