using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RPGItemCreator : EditorWindow
{
    private ItemContainer itemContainer;
    //private VisualElement m_RightPane;

    private SelectionUI selectionUI;
    private DetailsUI detailsUI;
    private void OnEnable()
    {
        // Load items when the window is enabled
        itemContainer = ItemSerialization.LoadItems();
    }

    private void OnDisable()
    {
        // Save items when the window is disabled
        ItemSerialization.SaveItems(itemContainer);
    }

    public void CreateGUI()
    {
        // Load existing items or create new container
        itemContainer ??= new ItemContainer();

        // Create a two-pane view with the left pane being fixed.
        var splitView = new TwoPaneSplitView(0, 350, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);
        
        // Create a vertical container for the search bar and ListView
        var leftPaneContainer = new VisualElement();
        leftPaneContainer.style.flexDirection = FlexDirection.Column; // Ensure vertical stacking
        splitView.Add(leftPaneContainer);

        leftPaneContainer.style.marginTop = 10;
        leftPaneContainer.style.marginLeft = 5;
        leftPaneContainer.style.marginRight = 5;
        leftPaneContainer.style.marginBottom = 10;
        leftPaneContainer.style.minWidth = 350;
        leftPaneContainer.style.flexGrow = 1;
        selectionUI = new SelectionUI(leftPaneContainer, itemContainer);

        // Right pane for item details
        var m_RightPane = new VisualElement();
        splitView.Add(m_RightPane);
        detailsUI = new DetailsUI(m_RightPane);
        
        SelectionUI.listViewPane.Rebuild();
    }

    [MenuItem("Tools/RPGItemCreator")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<RPGItemCreator>();
        wnd.titleContent = new GUIContent("RPGItemCreator");

        // Limit size of the window.
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }

#region Update methods for fields
    public static void UpdateItemName(string newItemName)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            Item item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.generalSettings.itemName = newItemName;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
            SelectionUI.listViewPane.Rebuild();  // Refresh the ListView
        }
    }

    public static void UpdateItemID(int newID)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            // Check for duplicate IDs
            if (SelectionUI._itemContainer.items.Any(item =>
                    item.generalSettings.itemID == newID &&
                    SelectionUI._itemContainer.items.IndexOf(item) != SelectionUI.selectedIndex))
            {
                EditorUtility.DisplayDialog("Duplicate ID", "An item with this ID already exists.", "OK");
                //itemIDField.SetValueWithoutNotify(SelectionUI._itemContainer.items[SelectionUI.selectedIndex].generalSettings.itemID);
                return;
            }

            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.generalSettings.itemID = newID;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdatePrefabPath(string prefabPath)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.generalSettings.prefabPath = prefabPath;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateItemType(ItemType newItemType)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.generalSettings.itemType = newItemType;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateStackSize(int newStackSize)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.weaponStats.stackSize = newStackSize;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateRarity(int newRarity)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.weaponStats.rarity = newRarity;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateAttackPower(int newAttackPower)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.weaponStats.attackPower = newAttackPower;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateAttackSpeed(float newAttackSpeed)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.weaponStats.attackSpeed = newAttackSpeed;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateRange(float newRange)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.weaponStats.range = newRange;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateDurability(float newDurability)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.weaponStats.durability = newDurability;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateStrength(int newStrength)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.strength = newStrength;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateIntelligence(int newIntelligence)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.intelligence = newIntelligence;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateAgility(int newAgility)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.agility = newAgility;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateLuck(int newLuck)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.luck = newLuck;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateMaxHealth(int newMaxHealth)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.maxHealth = newMaxHealth;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateMaxMana(int newMaxMana)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.maxMana = newMaxMana;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateMoveSpeed(float newMoveSpeed)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.moveSpeed = newMoveSpeed;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateAttackDamage(float newAttackDamage)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.attackDamage = newAttackDamage;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateCritChance(float newCritChance)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.critChance = newCritChance;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateCritMultiplier(float newCritMultiplier)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.critMultiplier = newCritMultiplier;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateDamageReduction(float newDamageReduction)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.damageReduction = newDamageReduction;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateExperienceMultiplier(float newExperienceMultiplier)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.modifiers.experienceMultiplier = newExperienceMultiplier;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateShortDescription(string newShortDesc)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.description.shortDescription = newShortDesc;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateDetailedDescription(string newDetailedDesc)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.description.detailedDescription = newDetailedDesc;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateDeveloperNotes(string newDevNotes)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.notes.developerNotes = newDevNotes;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateRequiredLevel(int newRequiredLevel)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.requirements.requiredLevel = newRequiredLevel;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateRequiredClass(string newRequiredClass)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.requirements.requiredClass = newRequiredClass;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateRequiresTwoHands(bool newRequiresTwoHands)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.requirements.requiresTwoHands = newRequiresTwoHands;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateStrengthRequirement(int newStrengthRequirement)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.requirements.strengthRequirement = newStrengthRequirement;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateIntelligenceRequirement(int newIntelligenceRequirement)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.requirements.intelligenceRequirement = newIntelligenceRequirement;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateAgilityRequirement(int newAgilityRequirement)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.requirements.agilityRequirement = newAgilityRequirement;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

    public static void UpdateLuckRequirement(int newLuckRequirement)
    {
        if (SelectionUI.selectedIndex >= 0 && SelectionUI.selectedIndex < SelectionUI._itemContainer.items.Count)
        {
            var item = SelectionUI._itemContainer.items[SelectionUI.selectedIndex];
            item.requirements.luckRequirement = newLuckRequirement;
            SelectionUI._itemContainer.items[SelectionUI.selectedIndex] = item;
        }
    }

#endregion
}