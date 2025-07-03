using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaties : Item
{
    private Player player;


    //ignore collisions with canal Water
    public override void equip() {
        //should ignore collisions with canals
        Physics.IgnoreLayerCollision(4, 7, true);
        // foreach()

    }

    public override void unequip() {
        Physics.IgnoreLayerCollision(4, 7, false);
    }


    public override void onGain() {
        this.player = FindObjectOfType<Player>();
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Floaties;
    }
}
