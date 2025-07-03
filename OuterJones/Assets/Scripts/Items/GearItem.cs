using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearItem : Item
{
    [SerializeField] Player p;
    public override void equip() {
        p.setRotateDirection(false);
    }

    public override void unequip() {
        p.setRotateDirection(true);
    }

    public override PossibleItems getItemType() {
        return PossibleItems.GearItem;
    }
}
