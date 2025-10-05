using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaties : Item
{
    public override PossibleItems getItemType() {
        return PossibleItems.Floaties;
    }

    public override void equip() {
        base.equip();
        FindObjectOfType<RoomsLayout>().notifyRoomListeners(null);
    }

    public override bool startsEquiped() {return true;}
    public override bool canBeToggled() {return false;}
    public override string getName() {return "floaties";}
}
