using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSourceManager : MonoBehaviour, RoomUpdateListener
{
    private List<WaterSource> sources = new List<WaterSource>();
    [SerializeField] RoomsLayout layout;
    
    private DrainManager drainM;

    public void Start() {
        layout.addRoomUpdateListener(this);
        this.drainM = GetComponent<DrainManager>();
    }

    public void addWaterSource(WaterSource source) {
        this.sources.Add(source);
    }

    public void restartFlood() {
        foreach(Room r in this.layout.getAllRooms(true)) {
            r.restartFlood();
        }

        foreach(Room r in this.layout.getAllRooms(false)) {
            r.restartFlood();
        }

        this.drainM.resetDrains();
    }

    public void onRoomUpdate(List<Room> rooms) {
        this.restartFlood();

        foreach(WaterSource s in this.sources) {
            s.computeFlow();
        }

        this.drainM.drainRooms();
    }

    public void recomputeFlow() {
        this.onRoomUpdate(new List<Room>());
    }

}
