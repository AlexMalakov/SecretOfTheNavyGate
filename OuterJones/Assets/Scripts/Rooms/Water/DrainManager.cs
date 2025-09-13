using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainManager : MonoBehaviour
{
    private List<WaterDrain> drains = new List<WaterDrain>();
    [SerializeField] RoomsLayout layout;


    public void addDrain(WaterDrain drain) {
        this.drains.Add(drain);
    }

    public void drainRooms() {
        this.floodRemainingCanals();
        
        foreach(WaterDrain d in this.drains) {
            d.drainWater();
        }

        resetDrains();
    }

    public void resetDrains() {
        foreach(WaterDrain d in this.drains) {
            d.reset();
        }
    }

    //floods from canals that are not reachable from a canal or a drain, and have water in them
    private void floodRemainingCanals() {
        foreach(Room r in this.layout.getAllRooms(true)) {
            r.floodAllRemainingCanals();
        }

        foreach(Room r in this.layout.getAllRooms(false)) {
            r.floodAllRemainingCanals();
        }
    }
}
