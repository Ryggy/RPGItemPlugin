using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class RPGItemCreator : EditorWindow
{
    private VisualElement m_RightPane;
    [SerializeField] private int m_SelectedIndex = -1;
    
    [MenuItem("Tools/RPGItemCreator")]
    public static void ShowWindow()
    {
        RPGItemCreator wnd = GetWindow<RPGItemCreator>();
        wnd.titleContent = new GUIContent("RPGItemCreator");
        
        // Limit size of the window.
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }
    
    public void CreateGUI()
    {
        // Get a list of all sprites in the Resources folder
        var allObjectGuids = AssetDatabase.FindAssets("t:Sprite", new string[]{"Assets/Resources"});
        var allObjects = new List<Sprite>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }
        
        // Create a two-pane view with the left pane being fixed.
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        // Add the view to the visual tree by adding it as a child to the root element.
        rootVisualElement.Add(splitView);
        
        // A TwoPaneSplitView needs exactly two child elements.
        var leftPane = new ListView();
        splitView.Add(leftPane);
        m_RightPane = new VisualElement();
        splitView.Add(m_RightPane);
        // Adding another split view for item info
        var rightPaneSplitView =  new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        m_RightPane.Add(rightPaneSplitView);
        
        // A TwoPaneSplitView needs exactly two child elements.
        var leftInfoPane = new ListView();
        rightPaneSplitView.Add(leftInfoPane);
        var rightInfoPane = new ListView();
        rightPaneSplitView.Add(rightInfoPane);

        // Left info pane content
        Label generalStatsLabel = new Label("General Stats");
        leftInfoPane.makeItem = () => generalStatsLabel;

        // right info pane content
        Label descriptionLabel = new Label("Description");
        rightInfoPane.makeItem = () => descriptionLabel;
        
        // Create a custom Visual Element for each item in the list
        leftPane.makeItem = () =>
        {
            var itemElement = new VisualElement();
            var image = new Image();
            image.style.width = 20;  // Adjust image size
            image.style.height = 20;
            var label = new Label();

            itemElement.Add(image);
            itemElement.Add(label);
            itemElement.style.flexDirection = FlexDirection.Row;
            return itemElement;
        };
        
        // Bind the data to each item
        leftPane.bindItem = (element, index) =>
        {
            var sprite = allObjects[index];
            var image = element.Q<Image>();   // Get the Image component
            var label = element.Q<Label>();   // Get the Label component

            image.sprite = sprite;            // Assign the sprite to the image
            label.text = sprite.name;         // Assign the sprite name to the label
        };

        // leftPane.makeItem = () => new Label();
        // leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        leftPane.itemsSource = allObjects;
        
        // React to the user's selection
        leftPane.selectionChanged += OnSpriteSelectionChange;
        
        // Restore the selection index from before the hot reload.
        leftPane.selectedIndex = m_SelectedIndex;

        // Store the selection index when the selection changes.
        leftPane.selectionChanged += (items) => { m_SelectedIndex = leftPane.selectedIndex; };
    }
    
    private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane.
        m_RightPane.Clear();

        // Get the selected sprite and display it.
        var enumerator = selectedItems.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var selectedSprite = enumerator.Current as Sprite;
            if (selectedSprite != null)
            {
                // Add a new Image control and display the sprite.
                var spriteImage = new Image();
                spriteImage.scaleMode = ScaleMode.ScaleToFit;
                spriteImage.sprite = selectedSprite;

                // Add the Image control to the right-hand pane.
                m_RightPane.Add(spriteImage);
            }
        }
    }
}
