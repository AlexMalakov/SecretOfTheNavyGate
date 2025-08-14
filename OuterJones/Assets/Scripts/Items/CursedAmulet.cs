using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedAmulet : Item
{
    public override PossibleItems getItemType() {
        return PossibleItems.Amulet;
    }

    public override bool startsEquiped() {return false;}
    public override bool canBeToggled() {return true;}
}
