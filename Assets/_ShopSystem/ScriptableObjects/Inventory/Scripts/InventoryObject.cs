using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Inventory = new List<InventorySlot>(); //holds items in inventory
    
    //add item to inventory
    public void AddItem(ItemObject addItem, int addAmount)
    {
        //check to see if the item is in inventory
        for(int i = 0; i < Inventory.Count; i++)
        {
            if(Inventory[i]._item == addItem) //if player has item
            {
                Inventory[i].AddAmount(addAmount); //add amount of item to player
                return; //stop loop
            }
        }
        //if no item found then add a new inv slot
        Inventory.Add(new InventorySlot(addItem, addAmount));
    }
}

//add inventory slot so items can be added to inventory
[System.Serializable] //able to see in inspector
public class InventorySlot
{
    //things user see in inspector about item
    public ItemObject _item;
    public int _amount;

    //set given item and amount to inventory slot
    public InventorySlot(ItemObject slotItem, int slotAmount)
    {
        _item = slotItem;
        _amount = slotAmount;
    }

    //add amount of item to inventory
    public void AddAmount(int val)
    {
        _amount += val;
    }
}
