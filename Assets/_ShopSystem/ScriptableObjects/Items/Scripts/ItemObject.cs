using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//setup diff item types
public enum ItemType
{
    Food,
    Weapon,
    Generic,
    Treasure
}

public abstract class ItemObject : ScriptableObject
{
    //info about the object for inspector
    [Header("Object Info")]
    public ItemType _type;

    [TextArea(15,20)]
    public string _description;

    //info for the shop system to use
    [Header("Shop Info")]
    public string _itemName;
    public Sprite _itemSprite;
    public int _itemPrice;
}
