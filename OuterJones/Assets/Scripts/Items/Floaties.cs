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
    //TODO: player with floaties should be able to cross through

    //ignore collisions with canal Water
    public override void equip() {
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
        foreach(Canal c in this.allCanals) {
            c.getWaterCollider().GetComponents<Collider2D>()[0].isTrigger = false;
            c.getWaterCollider().GetComponents<Collider2D>()[1].isTrigger = false;

            if(c.getSkinnySectionWhenFlooded() != null) {
                c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[0].isTrigger = false;
                c.getSkinnySectionWhenFlooded().GetComponents<Collider2D>()[1].isTrigger = false;
            }
        }
    }


    public override void onGain() {
        // this.player = FindObjectOfType<Player>();
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Floaties;
    }
}
