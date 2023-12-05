using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory/Items/Food")]
public class FoodObject : ItemObject
{
    //user can add more stats for food here
    [Header("Food Info")]
    public int healthVal;
    private void Awake()
    {
        //make sure correct type is set
        _type = ItemType.Food;
    }
}
