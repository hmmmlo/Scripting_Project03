using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    //user can add more stats for equipment here
    [Header("Equpment Info")]
    public int defense;

    private void Awake()
    {
        //make sure correct type is set
        _type = ItemType.Equipment;
    }
}
