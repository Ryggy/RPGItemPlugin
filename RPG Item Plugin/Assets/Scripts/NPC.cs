using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public TextAsset mainHand;
    public List<TextAsset> inventory;

    private Item _mainHand;
    private ItemContainer _inventory;

    private void Start()
    {
        _inventory = ItemSerialization.LoadItems(inventory);
        _mainHand = ItemSerialization.LoadItem(mainHand);

        Instantiate(_mainHand.generalSettings.prefab, new Vector3(1f, 0f, 1f), Quaternion.identity, transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log($"Attacking with {_mainHand.generalSettings.itemName}");
            Debug.Log($"{_mainHand.weaponStats.attackPower} Damage");
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("My Inventory");
            Debug.Log("-----------");
            foreach (var item in _inventory.items)
            {
                Debug.Log($"{item.generalSettings.itemName} x {item.weaponStats.stackSize}");
            }
        }
    }
}
