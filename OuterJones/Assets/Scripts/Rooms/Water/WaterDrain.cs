using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrain : Floodable, RoomUpdateListener
{
    private bool reachedByFlood;
    [SerializeField] RoomsLayout layout;

    [SerializeField] Canal origin;

    public void Start() {
        //we want to be updated once the sources are done
        this.layout.addPostRoomUpdateListener(this);
    }

    public override void onFlood() {
        this.reachedByFlood = true;
    }


    public override void drainWater() {
        origin.drainWater();
    }


    public void onRoomUpdate(List<Room> rooms) {
        if(!reachedByFlood) {
            this.drainWater();
        }

        //reset it so that every time rooms are updated they have to reach here...
        //TODO: concern that not flooding already "flooded rooms" will break this
        //hopefully the rewrite will not run into that problem
        this.reachedByFlood = false;
    }
}
