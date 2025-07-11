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

    private void drainAll() {
        foreach(Room r in this.layout.getAllRooms()) {
            r.drainWater();
        }
    }

    public void onRoomUpdate(List<Room> rooms) {
        this.drainAll();

        foreach(WaterSource s in this.sources) {
            s.computeFlow();
        }
    }

}
