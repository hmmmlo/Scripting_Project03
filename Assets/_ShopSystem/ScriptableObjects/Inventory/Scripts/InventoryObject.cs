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
        bool hasItem = false; //start by assuming player does not have item

        //check to see if the item is in inventory
        for(int i = 0; i < Inventory.Count; i++)
        {
            if(Inventory[i]._item == addItem) //if player has item
            {
                Inventory[i].AddAmount(addAmount); //add amount of item to player
                hasItem = true; //player has item
                break;
            }
        }

        if(!hasItem) //if player doesn't have item
        {
            //add new slot to inventory
            Inventory.Add(new InventorySlot(addItem, addAmount));
        }
    }
}

//add inventory slot so items can be added to inventory
[System.Serializable] //able to see in inspector
public class InventorySlot
{
    public ItemObject _item;
    public int _amt;

    //set given item and amount to inventory slot
    public InventorySlot(ItemObject slotItem, int slotAmount)
    {
        _item = slotItem;
        _amt = slotAmount;
    }

    //add amount of item to inventory
    public void AddAmount(int val)
    {
        _amt += val;
    }


}
