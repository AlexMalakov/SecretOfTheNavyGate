using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceManager : MonoBehaviour, RoomUpdateListener
{
    private List<LightSource> sources = new List<LightSource>();
    [SerializeField] RoomsLayout layout;

    public void Start() {
        layout.addRoomUpdateListener(this);
    }

    public void addLightSource(LightSource source) {
        this.sources.Add(source);
    }

    private void resetBeams() {
        foreach(Room r in this.layout.getAllRooms()) {
            r.removeBeam();
        }
    }

    public void onRoomUpdate(List<Room> rooms) {
        this.resetBeams();

        foreach(LightSource s in this.sources) {
            s.castBeam();
        }
    }
}
