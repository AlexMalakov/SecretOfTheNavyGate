using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainManager : MonoBehaviour, RoomUpdateListener
{
    private List<WaterDrain> drains = new List<WaterDrain>();
    [SerializeField] RoomsLayout layout;


    public void Start() {
        this.layout.addPostRoomUpdateListener(this);
    }

    public void addDrain(WaterDrain drain) {
        this.drains.Add(drain);
    }

    public void onRoomUpdate(List<Room> rooms) {
        Debug.Log("HELLO FROM MANAGER!");
        foreach(WaterDrain d in this.drains) {
            d.drainWater();
        }

        this.floodRemainingCanals();

        foreach(WaterDrain d in this.drains) {
            d.reset();
        }
    }

    //floods from canals that are not reachable from a canal or a drain, and have water in them
    private void floodRemainingCanals() {
        foreach(Room r in this.layout.getAllRooms()) {
            r.floodAllRemainingCanals();
        }
    }
}
