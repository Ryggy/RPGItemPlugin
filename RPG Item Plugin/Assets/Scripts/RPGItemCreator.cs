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
       
        // Create a vertical container for the search bar and ListView
        var leftPaneContainer = new VisualElement();
        leftPaneContainer.style.flexDirection = FlexDirection.Column;  // Ensure vertical stacking
        splitView.Add(leftPaneContainer);
        
        leftPaneContainer.style.marginTop = 10;
        leftPaneContainer.style.marginLeft = 5;
        leftPaneContainer.style.marginRight = 5;
        leftPaneContainer.style.marginBottom = 10;
        
        // Add a search bar above the ListView
        searchField = new TextField("Search");
        ApplyHeaderStyle(searchField);
        searchField.style.marginBottom = 5;  // add some spacing between search bar and list
        searchField.RegisterValueChangedCallback(evt => FilterList(evt.newValue));
        leftPaneContainer.Add(searchField);  // Add the search bar to the container
        
        // Left pane for the list of items
        leftPane = new ListView(itemContainer.items, 75, MakeItem, BindItem);
        leftPane.selectionType = SelectionType.Single;
        leftPane.selectionChanged += OnItemSelected;
        leftPaneContainer.Add(leftPane);  // Add the ListView below the search bar in the container
        
        leftPane.style.marginTop = 10;
        leftPane.style.marginLeft = 5;
        leftPane.style.marginRight = 5;
        leftPane.style.marginBottom = 10;
        
        // Right pane for item details
        m_RightPane = new VisualElement();
        
        splitView.Add(m_RightPane);
        
        // Add Save and Load buttons
        var buttonContainer = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                marginTop = 10,
                alignItems = Align.FlexStart
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
        
        buttonContainer.style.marginTop = 10;
        buttonContainer.style.marginLeft = 10;
        buttonContainer.style.marginRight = 10;
        buttonContainer.style.marginBottom = 10;
        
        m_RightPane.Add(buttonContainer);
        
        // Build the detail UI
        BuildDetailPane();
    }
    
   private void BuildDetailPane()
    {
        // Create a two-pane view with the left pane being fixed.
        var detailsSplitView = new TwoPaneSplitView(0, 700, TwoPaneSplitViewOrientation.Horizontal);
        m_RightPane.Add(detailsSplitView);
        
        // Add two child elements to the TwoPaneSplitView
        VisualElement leftDetailsPane = new ScrollView();
        VisualElement rightDetailsPane = new ScrollView();
        detailsSplitView.Add(leftDetailsPane);
        detailsSplitView.Add(rightDetailsPane);
        
        leftDetailsPane.style.marginTop = 10;
        leftDetailsPane.style.marginLeft = 10;
        leftDetailsPane.style.marginRight = 10;
        leftDetailsPane.style.marginBottom = 10;
        
        rightDetailsPane.style.marginTop = 10;
        rightDetailsPane.style.marginLeft = 10;
        rightDetailsPane.style.marginRight = 10;
        rightDetailsPane.style.marginBottom = 10;
        
        // General Settings
        var generalSettingsFoldout = new Foldout { text = "General Settings" };
        ApplyHeaderStyle(generalSettingsFoldout);
        leftDetailsPane.Add(generalSettingsFoldout);

        itemNameField = new TextField();
        AddLabeledField(generalSettingsFoldout, "Item Name", itemNameField);
        
        itemIDField = new IntegerField();
        AddLabeledField(generalSettingsFoldout, "Item ID", itemIDField);

        prefabField = new ObjectField()
        {
            objectType = typeof(GameObject),
            allowSceneObjects = false
        };
        AddLabeledField(generalSettingsFoldout, "Prefab", prefabField);

        // Weapon Stats
        var weaponStatsFoldout = new Foldout { text = "Weapon Stats" };
        ApplyHeaderStyle(weaponStatsFoldout);
        leftDetailsPane.Add(weaponStatsFoldout);

        stackSizeField = new IntegerField();
        AddLabeledField(weaponStatsFoldout, "Stack Size", stackSizeField);
        
        rarityField = new IntegerField();
        AddLabeledField(weaponStatsFoldout, "Rarity", rarityField);
        
        attackPowerField = new IntegerField();
        AddLabeledField(weaponStatsFoldout, "Attack Power", attackPowerField);

        attackSpeedField = new FloatField();
        AddLabeledField(weaponStatsFoldout, "Attack Speed", attackSpeedField);

        rangeField = new FloatField();
        AddLabeledField(weaponStatsFoldout, "Range", rangeField);

        durabilityField = new FloatField();
        AddLabeledField(weaponStatsFoldout, "Durability", durabilityField);

        // Modifiers
        var modifiersFoldout = new Foldout { text = "Modifiers" };
        ApplyHeaderStyle(modifiersFoldout);
        leftDetailsPane.Add(modifiersFoldout);
        
        strengthField = new IntegerField();
        AddLabeledField(modifiersFoldout, "Strength", strengthField);
        
        intelligenceField = new IntegerField();
        AddLabeledField(modifiersFoldout, "Intelligence", intelligenceField);
        
        agilityField = new IntegerField();
        AddLabeledField(modifiersFoldout, "Agility", agilityField);
        
        luckField = new IntegerField();
        AddLabeledField(modifiersFoldout, "Luck", luckField);
        
        maxHealthField = new IntegerField();
        AddLabeledField(modifiersFoldout, "Max Health", maxHealthField);
        
        maxManaField = new IntegerField();
        AddLabeledField(modifiersFoldout, "Max Mana", maxManaField);
        
        moveSpeedField = new FloatField();
        AddLabeledField(modifiersFoldout, "Move Speed", moveSpeedField);
        
        attackDamageField = new FloatField();
        AddLabeledField(modifiersFoldout, "Attack Damage", attackDamageField);
        
        critChanceField = new FloatField();
        AddLabeledField(modifiersFoldout, "Crit Chance", critChanceField);
        
        critMultiplierField = new FloatField();
        AddLabeledField(modifiersFoldout, "Crit Multiplier", critMultiplierField);
        
        damageReductionField = new FloatField();
        AddLabeledField(modifiersFoldout, "Damage Reduction", damageReductionField);
        
        experienceMultiplierField = new FloatField();
        AddLabeledField(modifiersFoldout, "Experience Multiplier", experienceMultiplierField);
        
        // Item Requirements
        var requirementsFoldout = new Foldout { text = "Item Requirements" };
        ApplyHeaderStyle(requirementsFoldout);
        rightDetailsPane.Add(requirementsFoldout);

        requiredLevelField = new IntegerField();
        AddLabeledField(requirementsFoldout, "Required Level", requiredLevelField);

        requiredClassField = new TextField();
        AddLabeledField(requirementsFoldout, "Required Class", requiredClassField);

        requiresTwoHandsToggle = new Toggle();
        AddLabeledField(requirementsFoldout, "Requires Two Hands", requiresTwoHandsToggle);
        
        strengthRequirementField = new IntegerField();
        AddLabeledField(requirementsFoldout, "Required Strength", strengthRequirementField);
        
        intelligenceRequirementField = new IntegerField();
        AddLabeledField(requirementsFoldout, "Required Intelligence", intelligenceRequirementField);
        
        agilityRequirementField = new IntegerField();
        AddLabeledField(requirementsFoldout, "Required Agility", agilityRequirementField);
        
        luckRequirementField = new IntegerField();
        AddLabeledField(requirementsFoldout, "Required Luck", luckRequirementField);
        
        // Description
        var descriptionFoldout = new Foldout{ text = "Description"};
        ApplyHeaderStyle(descriptionFoldout);
        rightDetailsPane.Add(descriptionFoldout);

        shortDescriptionField = new TextField();
        AddLabeledField(descriptionFoldout, "Short Description", shortDescriptionField);
        
        detailedDescriptionField = new TextField();
        AddLabeledField(descriptionFoldout, "Detailed Description", detailedDescriptionField);
        detailedDescriptionField.multiline = true;
        
        // Notes
        var notesFoldout = new Foldout{ text = "Notes"};
        ApplyHeaderStyle(notesFoldout);
        rightDetailsPane.Add(notesFoldout);

        developerNotesField = new TextField();
        AddLabeledField(notesFoldout, "Developer Notes", developerNotesField);
        developerNotesField.multiline = true;

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

    private void AddLabeledField(VisualElement container, string labelText, VisualElement field)
    {
        // Create a row container
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = Justify.FlexStart; // Aligns items to the start
        row.style.alignItems = Align.Center; // Center vertically if needed

        // Create the label
       
        var label = new Label(labelText)
        {
            style = { width = 150 } // Fixed width for the label
        };
        ApplyDefaultTextStyle(label);
        
        // Add the label to the row
        row.Add(label);
        
        // Create a spacer to push the input field to the end
        var spacer = new VisualElement();
        spacer.style.flexGrow = 1; // Allow spacer to take available space
        row.Add(spacer);
        
        ApplyDefaultTextStyle(field);
        field.style.width = Length.Percent(60);
        // Add the field to the row
        row.Add(field);
        
        // Add the row to the main container
        container.Add(row);
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
        itemElement.style.flexDirection = FlexDirection.Row;
        itemElement.style.alignItems = Align.FlexStart;
        itemElement.style.alignItems = Align.Center; 
        
        // Create and configure the image
        var image = new Image();
        image.style.flexGrow = 0; 
        //image.style.width = Length.Percent(100); 
        //image.style.height = Length.Auto(); 
        itemElement.Add(image);
        
        var label = new Label();
        label.style.flexGrow = 1;
        ApplyHeaderStyle(label);
        itemElement.Add(label);

        return itemElement;
    }
    
    private void BindItem(VisualElement element, int index)
    {
        var item = itemContainer.items[index];
        var image = element.Q<Image>();
        var label = element.Q<Label>();
        // Set item name
        label.text = item.generalSettings.itemName; 
        // Check if prefab exists
        if (item.generalSettings.prefab != null)
        {
            // Get Texture2D preview of the prefab
            Texture2D prefabTexture = AssetPreview.GetAssetPreview(item.generalSettings.prefab);

            if (prefabTexture != null)
            {
                // Create a sprite from the Texture2D
                Rect spriteRect = new Rect(0, 0, prefabTexture.width, prefabTexture.height);
                Sprite prefabSprite = Sprite.Create(prefabTexture, spriteRect, new Vector2(0.5f, 0.5f));

                // Assign the sprite to the Image element
                image.sprite = prefabSprite;
            }
        }
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

    public void ApplyHeaderStyle(VisualElement element)
    {
        element.style.fontSize = 16;
        element.style.unityFontStyleAndWeight = FontStyle.Bold;
        element.style.marginBottom = 10;  
        element.style.marginTop = 5;
    }
    
    public void ApplyDefaultTextStyle(VisualElement element)
    {
        element.style.fontSize = 14;
        element.style.unityFontStyleAndWeight = FontStyle.Normal;
        element.style.marginBottom = 5; 
    }
}


