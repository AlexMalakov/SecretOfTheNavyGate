using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floaties : Item
{
    // private Player player;


    //TODO: player with floaties should be able to cross through

    //ignore collisions with canal Water
    public override void equip() {
        //should ignore collisions with canals (so this wont work is the funny story... but ill dewal with this later)
        Physics.IgnoreLayerCollision(4, 7, true);
        // foreach()

    }

    public override void unequip() {
        Physics.IgnoreLayerCollision(4, 7, false);
    }


    public override void onGain() {
        // this.player = FindObjectOfType<Player>();
    }

    public override PossibleItems getItemType() {
        return PossibleItems.Floaties;
    }
}
