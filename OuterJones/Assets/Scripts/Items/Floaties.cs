using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaties : Item
{
    // private Player player;


    private Canal[] allCanals;

    public void Awake() {
        allCanals = FindObjectsOfType<Canal>(true);
    }

    //ignore collisions with canal Water
    public override void equip() {
        base.equip();
        //should ignore collisions with canals (so this wont work is the funny story... but ill dewal with this later)
        foreach(Canal c in this.allCanals) {
            c.getWaterCollider().GetComponents<Collider2D>()[0].isTrigger = true;
            c.getWaterCollider().GetComponents<Collider2D>()[1].isTrigger = true;
            if(c.getSkinnySectionWhenFlooded() != null) {
                c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[0].isTrigger = true;
                c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[1].isTrigger = true;
            }
        }

    }

    public override void unequip() {
        base.unequip();
        foreach(Canal c in this.allCanals) {
            c.getWaterCollider().GetComponents<Collider2D>()[0].isTrigger = false;
            c.getWaterCollider().GetComponents<Collider2D>()[1].isTrigger = false;

            if(c.getSkinnySectionWhenFlooded() != null) {
                c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[0].isTrigger = false;
                c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[1].isTrigger = false;
            }
        }
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Floaties;
    }

    public override bool startsEquiped() {return true;}
    public override bool canBeToggled() {return false;}
}
