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

    private void restartFlood() {
        foreach(Room r in this.layout.getAllRooms()) {
            r.restartFlood();
        }
    }

    public void onRoomUpdate(List<Room> rooms) {
        this.restartFlood();

        foreach(WaterSource s in this.sources) {
            s.computeFlow();
        }
    }
}
