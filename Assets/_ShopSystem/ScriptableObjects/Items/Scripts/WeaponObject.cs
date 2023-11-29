using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory/Items/Weapon")]
public class WeaponObject : ItemObject
{
    [Header("Weapon Info")]
    public int damage;

    private void Awake()
    {
        //make sure correct type is set
        _type = ItemType.Weapon;
    }
}
