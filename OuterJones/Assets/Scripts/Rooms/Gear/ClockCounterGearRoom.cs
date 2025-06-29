using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockCounterGearRoom : GearRoom
{
    private bool clockwise = true;
    private Player player;
    
    public void Awake() {
        this.player = FindObjectOfType<Player>();
    }

    public override void onEnter(Door entererFrom) {

        //enterFrom.rotateDirection(this.clockwise).useDoor();
        Door exitDoor = this.getEntrance(entererFrom.rotateDirection(this.clockwise));
        
        if(exitDoor.getDestination() != null || this.layoutManager.canPlaceRoom(exitDoor, player.getNextInDeck())) {
            
            exitDoor.useDoor(this.player);
        } else {
            entererFrom.useDoor(this.player);
        }

        clockwise = !clockwise;
    }
}
