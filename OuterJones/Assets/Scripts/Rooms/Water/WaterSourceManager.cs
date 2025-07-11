using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSourceManager : MonoBehaviour, RoomUpdateListener
{
    private List<WaterSource> sources = new List<WaterSource>();
    [SerializeField] RoomsLayout layout;

    public void Start() {
        layout.addRoomUpdateListener(this);
    }

    public void addWaterSource(WaterSource source) {
        this.sources.Add(source);
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
