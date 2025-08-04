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

        foreach(Canal c in this.allCanals) {
            c.getWaterCollider().onFloatiesAquired();
            c.getSkinnyCollider().onFloatiesAquired();
        }

    }

    // public override void unequip() {
    //     base.unequip();
    //     foreach(Canal c in this.allCanals) {
    //         c.getWaterCollider().onFloatiesAquired(false);
    //         if(c.getSkinnySectionWhenFlooded() != null) {
    //             c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[0].isTrigger = false;
    //             c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[1].isTrigger = false;
    //         }
    //     }
    // }

    public override PossibleItems getItemType() {
        return PossibleItems.Floaties;
    }

    public override bool startsEquiped() {return true;}
    public override bool canBeToggled() {return false;}
}
