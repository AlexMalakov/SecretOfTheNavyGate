using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearItem : Item
{
    [SerializeField] Player p;
    public void equip() {
        p.setRotateDirection(false);
    }

    public void unequip() {
        p.setRotateDirection(true);
    }
}
