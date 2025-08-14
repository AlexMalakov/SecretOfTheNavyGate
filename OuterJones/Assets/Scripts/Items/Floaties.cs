using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaties : Item
{
    public override PossibleItems getItemType() {
        return PossibleItems.Floaties;
    }

    public override bool startsEquiped() {return true;}
    public override bool canBeToggled() {return false;}
}
