using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType{
    Weapon,
    Armour,
    NPC
}

[System.Serializable]
public struct Item
{
    public GeneralSettings generalSettings;
    public WeaponStats weaponStats;
    public Modifiers modifiers;
    public Description description;
    public Notes notes;
    public ItemRequirements requirements;
}

[System.Serializable]
public struct GeneralSettings
{
    public string itemName;
    public int itemID;
    public GameObject prefab;
    public ItemType itemType;
}

[System.Serializable]
public struct WeaponStats
{
    public int stackSize;
    public int rarity;
    public int attackPower;
    public float attackSpeed;
    public float range;
    public float durability;
}
[System.Serializable]
public struct Modifiers
{
    public int strength;
    public int intelligence;
    public int agility;
    public int luck;
    public int maxHealth;
    public int maxMana;
    public float moveSpeed;
    public float attackDamage;
    public float critChance;
    public float critMultiplier;
    public float damageReduction;
    public float experienceMultiplier;
}
[System.Serializable]
public struct Description
{
    public string shortDescription;   // For quick, in-game summaries
    public string detailedDescription; // For longer lore or detailed information
}
[System.Serializable]
public struct Notes
{
    public string developerNotes;
}
[System.Serializable]
public struct ItemRequirements
{
    public int requiredLevel;
    public string requiredClass;  // Could be "Warrior", "Mage", etc.
    public bool requiresTwoHands; // For two-handed weapons
    public int strengthRequirement;
    public int intelligenceRequirement;
    public int agilityRequirement;
    public int luckRequirement;
}