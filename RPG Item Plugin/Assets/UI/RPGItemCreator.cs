using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.Serialization;

public class RPGItemCreator : EditorWindow
{
    private VisualElement m_RightPane;
    private ListView leftPane;
    private ItemContainer itemContainer;
    private int selectedIndex = -1;
    private TextField searchField;
    
    // Fields for editing
    // General Settings
    private TextField itemNameField;
    private IntegerField itemIDField;
    private ObjectField prefabField;
    // Weapon Stats
    private IntegerField stackSizeField;
    private IntegerField rarityField;
    private IntegerField attackPowerField;
    private FloatField attackSpeedField;
    private FloatField rangeField;
    private FloatField durabilityField;
    // Modifiers
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
    // Description
    private TextField shortDescriptionField;
    private TextField detailedDescriptionField;
    // Notes
    private TextField developerNotesField;
    // Item Requirements
    private IntegerField requiredLevelField;
    private TextField requiredClassField;
    private Toggle requiresTwoHandsToggle;
    private IntegerField strengthRequirementField;
    private IntegerField intelligenceRequirementField;
    private IntegerField agilityRequirementField;
    private IntegerField luckRequirementField;
    
    [MenuItem("Tools/RPGItemCreator")]
    public static void ShowWindow()
    {
        RPGItemCreator wnd = GetWindow<RPGItemCreator>();
        wnd.titleContent = new GUIContent("RPGItemCreator");
        
        // Limit size of the window.
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }
    
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
        var splitView = new TwoPaneSplitView(0, 300, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);
        
        // Add a search bar above the ListView
        searchField = new TextField("Search");
        searchField.RegisterValueChangedCallback(evt => FilterList(evt.newValue));
        splitView.Insert(0, searchField);  // Insert at the top
        
        // Left pane for the list of items
        leftPane = new ListView(itemContainer.items, 20, MakeItem, BindItem);
        leftPane.selectionType = SelectionType.Single;
        leftPane.onSelectionChange += OnItemSelected;
        splitView.Add(leftPane);

        // Right pane for item details
        m_RightPane = new ScrollView();
        splitView.Add(m_RightPane);
        
        // Build the detail UI
        BuildDetailPane();

        // Add Save and Load buttons
        var buttonContainer = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                marginTop = 10
            }
        };

        var saveButton = new Button(() => ItemSerialization.SaveItems(itemContainer))
        {
            text = "Save Items"
        };
        var loadButton = new Button(() =>
        {
            itemContainer = ItemSerialization.LoadItems();
            leftPane.itemsSource = itemContainer.items;
            leftPane.Rebuild();
            ClearDetailPane();
        })
        {
            text = "Load Items"
        };
        var addButton = new Button(() => AddNewItem())
        {
            text = "Add Item"
        };
        var removeButton = new Button(() => RemoveSelectedItem())
        {
            text = "Remove Item"
        };

        buttonContainer.Add(saveButton);
        buttonContainer.Add(loadButton);
        buttonContainer.Add(addButton);
        buttonContainer.Add(removeButton);
        m_RightPane.Add(buttonContainer);
    }
    
    private void FilterList(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            leftPane.itemsSource = itemContainer.items;
        }
        else
        {
            leftPane.itemsSource = itemContainer.items.Where(item => item.generalSettings.itemName.ToLower().Contains(searchTerm.ToLower())).ToList();
        }
        leftPane.Rebuild();
    }
    
    private VisualElement MakeItem()
    {
        var itemElement = new VisualElement();
        var label = new Label();
        itemElement.Add(label);
        return itemElement;
    }
    
    private void BindItem(VisualElement element, int index)
    {
        var item = itemContainer.items[index];
        var label = element.Q<Label>();
        label.text = item.generalSettings.itemName;
    }
    
    private void OnItemSelected(IEnumerable<object> selectedItems)
    {
        var itemList = selectedItems.ToList();
        if (itemList.Count > 0)
        {
            // Find the index of the selected item
            selectedIndex = itemContainer.items.IndexOf((Item)itemList[0]);
            DisplayItemDetails(itemContainer.items[selectedIndex]);
        }
        else
        {
            selectedIndex = -1;
            ClearDetailPane();
        }
    }
    
    private void BuildDetailPane()
    {
        // General Settings
        var generalSettingsFoldout = new Foldout{ text = "General Settings"};
        generalSettingsFoldout.style.fontSize = 16;
        generalSettingsFoldout.style.unityFontStyleAndWeight = FontStyle.Bold;
        m_RightPane.Add(generalSettingsFoldout);

        itemNameField = new TextField("Item Name");
        generalSettingsFoldout.Add(itemNameField);

        itemIDField = new IntegerField("Item ID");
        generalSettingsFoldout.Add(itemIDField);

        prefabField = new ObjectField("Prefab")
        {
            objectType = typeof(GameObject),
            allowSceneObjects = false
        };
        generalSettingsFoldout.Add(prefabField);

        // Weapon Stats
        var weaponStatsFoldout = new Foldout{ text = "Weapon Stats"};
        weaponStatsFoldout.style.fontSize = 16;
        weaponStatsFoldout.style.unityFontStyleAndWeight = FontStyle.Bold;
        m_RightPane.Add(weaponStatsFoldout);

        stackSizeField = new IntegerField("Stack Size");
        weaponStatsFoldout.Add(stackSizeField);
        
        rarityField = new IntegerField("Rarity");
        weaponStatsFoldout.Add(rarityField);
        
        attackPowerField = new IntegerField("Attack Power");
        m_RightPane.Add(attackPowerField);

        attackSpeedField = new FloatField("Attack Speed");
        weaponStatsFoldout.Add(attackSpeedField);

        rangeField = new FloatField("Range");
        weaponStatsFoldout.Add(rangeField);

        durabilityField = new FloatField("Durability");
        weaponStatsFoldout.Add(durabilityField);

        // Modifiers
        var modifiersFoldout = new Foldout{ text = "Modifiers"};
        modifiersFoldout.style.fontSize = 16;
        modifiersFoldout.style.unityFontStyleAndWeight = FontStyle.Bold;
        m_RightPane.Add(modifiersFoldout);
        
        strengthField = new IntegerField("Strength");
        modifiersFoldout.Add(strengthField);
        
        intelligenceField= new IntegerField("Intelligence");
        modifiersFoldout.Add(intelligenceField);
        
        agilityField= new IntegerField("Agility");
        modifiersFoldout.Add(agilityField);
        
        luckField= new IntegerField("Luck");
        modifiersFoldout.Add(luckField);
        
        maxHealthField= new IntegerField("Max Health");
        modifiersFoldout.Add(maxHealthField);
        
        maxManaField= new IntegerField("Max Mana");
        modifiersFoldout.Add(maxManaField);
        
        moveSpeedField = new FloatField("Move Speed");
        modifiersFoldout.Add(moveSpeedField);
        
        attackDamageField= new FloatField("Attack Damage");
        modifiersFoldout.Add(attackDamageField);
        
        critChanceField= new FloatField("Crit Chance");
        modifiersFoldout.Add(critChanceField);
        
        critMultiplierField= new FloatField("Crit Multiplier");
        modifiersFoldout.Add(critMultiplierField);
        
        damageReductionField= new FloatField("Damage Reduction");
        modifiersFoldout.Add(damageReductionField);
        
        experienceMultiplierField= new FloatField("Experience Multiplier");
        modifiersFoldout.Add(experienceMultiplierField);

        // Description
        var descriptionLabel = new Label("Description");
        descriptionLabel.style.fontSize = 16;
        descriptionLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        m_RightPane.Add(descriptionLabel);

        shortDescriptionField = new TextField("Short Description");
        m_RightPane.Add(shortDescriptionField);

        detailedDescriptionField = new TextField("Detailed Description");
        detailedDescriptionField.style.height = 60;
        m_RightPane.Add(detailedDescriptionField);

        // Notes
        var notesLabel = new Label("Notes");
        notesLabel.style.fontSize = 16;
        notesLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        m_RightPane.Add(notesLabel);

        developerNotesField = new TextField("Developer Notes");
        developerNotesField.style.height = 40;
        m_RightPane.Add(developerNotesField);

        // Item Requirements
        var requirementsFoldout = new Foldout{ text = "Item Requirements"};
        requirementsFoldout.style.fontSize = 16;
        requirementsFoldout.style.unityFontStyleAndWeight = FontStyle.Bold;
        m_RightPane.Add(requirementsFoldout);

        requiredLevelField = new IntegerField("Required Level");
        requirementsFoldout.Add(requiredLevelField);

        requiredClassField = new TextField("Required Class");
        requirementsFoldout.Add(requiredClassField);

        requiresTwoHandsToggle = new Toggle("Requires Two Hands");
        requirementsFoldout.Add(requiresTwoHandsToggle);
        
        strengthRequirementField = new IntegerField("Required Strength");
        requirementsFoldout.Add(strengthRequirementField);
        
        intelligenceRequirementField = new IntegerField("Required Intelligence");
        requirementsFoldout.Add(intelligenceRequirementField);
        
        agilityRequirementField = new IntegerField("Required Agility");
        requirementsFoldout.Add(agilityRequirementField);
        
        luckRequirementField= new IntegerField("Required Luck");
        requirementsFoldout.Add(luckRequirementField);

        // Add change listeners to update the item data when fields are edited
        itemNameField.RegisterValueChangedCallback(evt => UpdateItemName(evt.newValue));
        itemIDField.RegisterValueChangedCallback(evt => UpdateItemID(evt.newValue));
        prefabField.RegisterValueChangedCallback(evt => UpdatePrefab(evt.newValue as GameObject));

        stackSizeField.RegisterValueChangedCallback(evt => UpdateStackSize(evt.newValue));
        rarityField.RegisterValueChangedCallback(evt => UpdateRarity(evt.newValue));
        attackPowerField.RegisterValueChangedCallback(evt => UpdateAttackPower(evt.newValue));
        attackSpeedField.RegisterValueChangedCallback(evt => UpdateAttackSpeed(evt.newValue));
        rangeField.RegisterValueChangedCallback(evt => UpdateRange(evt.newValue));
        durabilityField.RegisterValueChangedCallback(evt => UpdateDurability(evt.newValue));
        
        strengthField.RegisterValueChangedCallback(evt => UpdateStrength(evt.newValue));
        intelligenceField.RegisterValueChangedCallback(evt => UpdateIntelligence(evt.newValue));
        agilityField.RegisterValueChangedCallback(evt => UpdateAgility(evt.newValue));
        luckField.RegisterValueChangedCallback(evt => UpdateLuck(evt.newValue));
        maxHealthField.RegisterValueChangedCallback(evt => UpdateMaxHealth(evt.newValue));
        maxManaField.RegisterValueChangedCallback(evt => UpdateMaxMana(evt.newValue));
        moveSpeedField.RegisterValueChangedCallback(evt => UpdateMoveSpeed(evt.newValue));
        attackDamageField.RegisterValueChangedCallback(evt => UpdateAttackDamage(evt.newValue));
        critChanceField.RegisterValueChangedCallback(evt => UpdateCritChance(evt.newValue));
        critMultiplierField.RegisterValueChangedCallback(evt => UpdateCritMultiplier(evt.newValue));
        damageReductionField.RegisterValueChangedCallback(evt => UpdateDamageReduction(evt.newValue));
        experienceMultiplierField.RegisterValueChangedCallback(evt => UpdateExperienceMultiplier(evt.newValue));
        
        shortDescriptionField.RegisterValueChangedCallback(evt => UpdateShortDescription(evt.newValue));
        detailedDescriptionField.RegisterValueChangedCallback(evt => UpdateDetailedDescription(evt.newValue));

        developerNotesField.RegisterValueChangedCallback(evt => UpdateDeveloperNotes(evt.newValue));

        requiredLevelField.RegisterValueChangedCallback(evt => UpdateRequiredLevel(evt.newValue));
        requiredClassField.RegisterValueChangedCallback(evt => UpdateRequiredClass(evt.newValue));
        requiresTwoHandsToggle.RegisterValueChangedCallback(evt => UpdateRequiresTwoHands(evt.newValue));
        strengthRequirementField.RegisterValueChangedCallback(evt => UpdateStrengthRequirement(evt.newValue));
        intelligenceRequirementField.RegisterValueChangedCallback(evt => UpdateIntelligenceRequirement(evt.newValue));
        agilityRequirementField.RegisterValueChangedCallback(evt => UpdateAgilityRequirement(evt.newValue));
        luckRequirementField.RegisterValueChangedCallback(evt => UpdateLuckRequirement(evt.newValue));
    }

    private void DisplayItemDetails(Item item)
    {
        // Populate the fields with the item's data
        itemNameField.SetValueWithoutNotify(item.generalSettings.itemName);
        itemIDField.SetValueWithoutNotify(item.generalSettings.itemID);
        prefabField.SetValueWithoutNotify(item.generalSettings.prefab);

        stackSizeField.SetValueWithoutNotify(item.weaponStats.stackSize);
        rarityField.SetValueWithoutNotify(item.weaponStats.rarity);
        attackPowerField.SetValueWithoutNotify(item.weaponStats.attackPower);
        attackSpeedField.SetValueWithoutNotify(item.weaponStats.attackSpeed);
        rangeField.SetValueWithoutNotify(item.weaponStats.range);
        durabilityField.SetValueWithoutNotify(item.weaponStats.durability);

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

        shortDescriptionField.SetValueWithoutNotify(item.description.shortDescription);
        detailedDescriptionField.SetValueWithoutNotify(item.description.detailedDescription);

        developerNotesField.SetValueWithoutNotify(item.notes.developerNotes);

        requiredLevelField.SetValueWithoutNotify(item.requirements.requiredLevel);
        requiredClassField.SetValueWithoutNotify(item.requirements.requiredClass);
        requiresTwoHandsToggle.SetValueWithoutNotify(item.requirements.requiresTwoHands);
        strengthRequirementField.SetValueWithoutNotify(item.requirements.strengthRequirement);
        intelligenceRequirementField.SetValueWithoutNotify(item.requirements.intelligenceRequirement);
        agilityRequirementField.SetValueWithoutNotify(item.requirements.agilityRequirement);
        luckRequirementField.SetValueWithoutNotify(item.requirements.luckRequirement);
    }
    
    private void ClearDetailPane()
    {
        itemNameField.SetValueWithoutNotify("");
        itemIDField.SetValueWithoutNotify(0);
        prefabField.SetValueWithoutNotify(null);

        stackSizeField.SetValueWithoutNotify(0);
        rarityField.SetValueWithoutNotify(0);
        attackPowerField.SetValueWithoutNotify(0);
        attackSpeedField.SetValueWithoutNotify(0f);
        rangeField.SetValueWithoutNotify(0f);
        durabilityField.SetValueWithoutNotify(0f);

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

        shortDescriptionField.SetValueWithoutNotify("");
        detailedDescriptionField.SetValueWithoutNotify("");

        developerNotesField.SetValueWithoutNotify("");

        requiredLevelField.SetValueWithoutNotify(0);
        requiredClassField.SetValueWithoutNotify("");
        requiresTwoHandsToggle.SetValueWithoutNotify(false);
        strengthRequirementField.SetValueWithoutNotify(0);
        intelligenceRequirementField.SetValueWithoutNotify(0);
        agilityRequirementField.SetValueWithoutNotify(0);
        luckRequirementField.SetValueWithoutNotify(0);
    }
    
    private void AddNewItem()
    {
        // Create a default new item
        Item newItem = new Item
        {
            generalSettings = new GeneralSettings
            {
                itemName = "New Item",
                itemID = itemContainer.items.Count > 0 ? itemContainer.items.Max(i => i.generalSettings.itemID) + 1 : 1,
                prefab = null
            },
            weaponStats = new WeaponStats(),
            modifiers = new Modifiers(),
            description = new Description(),
            notes = new Notes(),
            requirements = new ItemRequirements()
        };

        itemContainer.items.Add(newItem);
        leftPane.Rebuild();
        leftPane.selectedIndex = itemContainer.items.Count - 1;
    }
    
    private void RemoveSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            itemContainer.items.RemoveAt(selectedIndex);
            leftPane.Rebuild();
            selectedIndex = -1;
            ClearDetailPane();
        }
    }

    #region Update methods for each field
    private void UpdateItemName(string newName)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.generalSettings.itemName = newName;
            itemContainer.items[selectedIndex] = item;
            leftPane.Rebuild();  // Refresh the ListView
        }
    }

    private void UpdateItemID(int newID)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            // Check for duplicate IDs
            if (itemContainer.items.Any(item => item.generalSettings.itemID == newID && itemContainer.items.IndexOf(item) != selectedIndex))
            {
                EditorUtility.DisplayDialog("Duplicate ID", "An item with this ID already exists.", "OK");
                itemIDField.SetValueWithoutNotify(itemContainer.items[selectedIndex].generalSettings.itemID);
                return;
            }

            Item item = itemContainer.items[selectedIndex];
            item.generalSettings.itemID = newID;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdatePrefab(GameObject newPrefab)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.generalSettings.prefab = newPrefab;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateStackSize(int newStackSize)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.weaponStats.stackSize = newStackSize;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateRarity(int newRarity)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.weaponStats.rarity = newRarity;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateAttackPower(int newAttackPower)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.weaponStats.attackPower = newAttackPower;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateAttackSpeed(float newAttackSpeed)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.weaponStats.attackSpeed = newAttackSpeed;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateRange(float newRange)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.weaponStats.range = newRange;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateDurability(float newDurability)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.weaponStats.durability = newDurability;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateStrength(int newStrength)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.strength = newStrength;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateIntelligence(int newIntelligence)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.intelligence = newIntelligence;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateAgility(int newAgility)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.agility = newAgility;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateLuck(int newLuck)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.luck = newLuck;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateMaxHealth(int newMaxHealth)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.maxHealth = newMaxHealth;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateMaxMana(int newMaxMana)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.maxMana = newMaxMana;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateMoveSpeed(float newMoveSpeed)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.moveSpeed = newMoveSpeed;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateAttackDamage(float newAttackDamage)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.attackDamage = newAttackDamage;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateCritChance(float newCritChance)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.critChance = newCritChance;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateCritMultiplier(float newCritMultiplier)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.critMultiplier = newCritMultiplier;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateDamageReduction(float newDamageReduction)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.damageReduction = newDamageReduction;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateExperienceMultiplier(float newExperienceMultiplier)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.modifiers.experienceMultiplier = newExperienceMultiplier;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateShortDescription(string newShortDesc)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.description.shortDescription = newShortDesc;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateDetailedDescription(string newDetailedDesc)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.description.detailedDescription = newDetailedDesc;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateDeveloperNotes(string newDevNotes)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.notes.developerNotes = newDevNotes;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateRequiredLevel(int newRequiredLevel)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.requirements.requiredLevel = newRequiredLevel;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateRequiredClass(string newRequiredClass)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.requirements.requiredClass = newRequiredClass;
            itemContainer.items[selectedIndex] = item;
        }
    }

    private void UpdateRequiresTwoHands(bool newRequiresTwoHands)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.requirements.requiresTwoHands = newRequiresTwoHands;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateStrengthRequirement(int newStrengthRequirement)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.requirements.strengthRequirement = newStrengthRequirement;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateIntelligenceRequirement(int newIntelligenceRequirement)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.requirements.intelligenceRequirement = newIntelligenceRequirement;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateAgilityRequirement(int newAgilityRequirement)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.requirements.agilityRequirement = newAgilityRequirement;
            itemContainer.items[selectedIndex] = item;
        }
    }
    
    private void UpdateLuckRequirement(int newLuckRequirement)
    {
        if (selectedIndex >= 0 && selectedIndex < itemContainer.items.Count)
        {
            Item item = itemContainer.items[selectedIndex];
            item.requirements.luckRequirement = newLuckRequirement;
            itemContainer.items[selectedIndex] = item;
        }
    }
    #endregion 

}

  // public void CreateGUI()
  //   {
  //       // Get a list of all items in the items folder
  //       var allObjectSOs = AssetDatabase.FindAssets("t:ScriptableObject", new string[]{"Assets/Scripts/Items"});
  //       var allObjects = new List<ItemScriptableObject>();
  //       foreach (var SO in allObjectSOs)
  //       {
  //           allObjects.Add(AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(AssetDatabase.GUIDToAssetPath(SO)));
  //       }
  //       
  //       // Create a two-pane view with the left pane being fixed.
  //       var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
  //       // Add the view to the visual tree by adding it as a child to the root element.
  //       rootVisualElement.Add(splitView);
  //       
  //       // A TwoPaneSplitView needs exactly two child elements.
  //       var leftPane = new ListView();
  //       splitView.Add(leftPane);
  //       m_RightPane = new VisualElement();
  //       splitView.Add(m_RightPane);
  //       // Adding another split view for item info
  //       var rightPaneSplitView =  new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
  //       m_RightPane.Add(rightPaneSplitView);
  //       
  //       // A TwoPaneSplitView needs exactly two child elements.
  //       var leftInfoPane = new ListView();
  //       rightPaneSplitView.Add(leftInfoPane);
  //       var rightInfoPane = new ListView();
  //       rightPaneSplitView.Add(rightInfoPane);
  //       
  //       // Create a custom Visual Element for each item in the list
  //       leftPane.makeItem = () =>
  //       {
  //           var itemElement = new VisualElement();
  //           var image = new Image();
  //           image.style.width = 20;  // Adjust image size
  //           image.style.height = 20;
  //           var label = new Label();
  //
  //           itemElement.Add(image);
  //           itemElement.Add(label);
  //           itemElement.style.flexDirection = FlexDirection.Row;
  //           return itemElement;
  //       };
  //       
  //       // Bind the data to each item
  //       leftPane.bindItem = (element, index) =>
  //       {
  //           var item = allObjects[index];
  //           var image = element.Q<Image>();   // Get the Image component
  //           var label = element.Q<Label>();   // Get the Label component
  //
  //           // if (item.pre!= null)
  //           // {
  //           //     Texture2D prefabVisualTexture = AssetPreview.GetAssetPreview(prefabObjectField.value);
  //           //     prefabVisual.sprite = Sprite.Create(prefabVisualTexture, new Rect(0,0, prefabVisualTexture.width, prefabVisualTexture.height), new Vector2(0.5f, 0.5f));
  //           // }
  //           //
  //           // image.sprite = sprite;
  //           // label.text = sprite.name;
  //       };
  //
  //       // leftPane.makeItem = () => new Label();
  //       // leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
  //       leftPane.itemsSource = allObjects;
  //
  //       // Get a list of all Scriptable Objects in the Item Info folder
  //       var allItemInfoSO = AssetDatabase.FindAssets("t:ScriptableObject", new string[]{"Assets/Scripts/Items/Weapons"});
  //       var allItemInfo = new List<GeneralSettingsScriptableObject>();
  //       foreach (var sO in allItemInfoSO)
  //       {
  //           allItemInfo.Add(AssetDatabase.LoadAssetAtPath<GeneralSettingsScriptableObject>(AssetDatabase.GUIDToAssetPath(sO)));
  //       }
  //       
  //       // Left info pane content
  //       leftInfoPane.makeItem = () =>
  //       {
  //           var itemElement = new VisualElement();
  //           Label label = new Label();
  //           var container = new VisualElement();
  //           itemElement.Add(label);
  //           itemElement.Add(container);
  //           
  //           var prefabVisual = new Image();
  //           var container2 = new VisualElement();
  //           container.Add(prefabVisual);
  //           container.Add(container2);
  //           container.style.flexDirection = FlexDirection.Row;
  //           
  //           // container2 content
  //           var nameInput = new TextField();
  //           var typeDropdown = new DropdownField();
  //           var prefabInput = new ObjectField();
  //           container2.Add(nameInput);
  //           container2.Add(typeDropdown);
  //           container2.Add(prefabInput);
  //
  //           return itemElement;
  //       };
  //       
  //       // Bind the data to each item
  //       leftInfoPane.bindItem = (element, index) =>
  //       {
  //           var prefabVisual = element.Q<Image>(); 
  //           var label = element.Q<Label>(); 
  //           var nameInput = element.Q<TextField>();
  //           var typeDropdown = element.Q<DropdownField>();
  //           var prefabObjectField = element.Q<ObjectField>();
  //
  //           if (prefabObjectField.value != null)
  //           {
  //               Texture2D prefabVisualTexture = AssetPreview.GetAssetPreview(prefabObjectField.value);
  //               prefabVisual.sprite = Sprite.Create(prefabVisualTexture, new Rect(0,0, prefabVisualTexture.width, prefabVisualTexture.height), new Vector2(0.5f, 0.5f));
  //           }
  //           
  //           label.text = "General Settings";
  //       };
  //
  //       leftInfoPane.itemsSource = allItemInfoSO;
  //       
  //       // right info pane content
  //       rightInfoPane.makeItem = () =>
  //       {
  //           var itemElement = new VisualElement();
  //           var descriptionElement = new VisualElement();
  //
  //           return itemElement;
  //       };
  //       
  //       
  //       // React to the user's selection
  //       leftPane.selectionChanged += OnSpriteSelectionChange;
  //       
  //       // Restore the selection index from before the hot reload.
  //       leftPane.selectedIndex = m_SelectedIndex;
  //
  //       // Store the selection index when the selection changes.
  //       leftPane.selectionChanged += (items) => { m_SelectedIndex = leftPane.selectedIndex; };
  //   }
  // private void OnItemSelectionChanged(IEnumerable<object> selectedItems)
  // {
  //     // Clear all previous content from the pane.
  //     m_RightPane.Clear();
  //
  //     // Get the selected item and display it.
  //     var enumerator = selectedItems.GetEnumerator();
  //     if (enumerator.MoveNext())
  //     {
  //         var selectedSprite = enumerator.Current as Sprite;
  //         if (selectedSprite != null)
  //         {
  //             // Add a new Image control and display the sprite.
  //             var spriteImage = new Image();
  //             spriteImage.scaleMode = ScaleMode.ScaleToFit;
  //             spriteImage.sprite = selectedSprite;
  //
  //             // Add the Image control to the right-hand pane.
  //             m_RightPane.Add(spriteImage);
  //         }
  //     }
  // }

