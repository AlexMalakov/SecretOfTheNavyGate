using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearItem : Item
{
    [SerializeField] Player p;
    public override void equip() {
        base.equip();
        p.setRotateDirection(false);
    }

    public override void unequip() {
        base.unequip();
        p.setRotateDirection(true);
    }

    public override PossibleItems getItemType() {
        return PossibleItems.GearItem;
    }

    public override bool startsEquiped() {return false;}
    public override bool canBeToggled() {return true;}
}
