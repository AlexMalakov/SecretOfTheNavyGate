using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour, RoomUpdateListener
{
    [SerializeField] private Room originRoom;
    [SerializeField] private RoomsLayout layout;

    [SerializeField] private GameObject beamModel; //need to draw a lien between 2 points

    private DoorDirection castDirection = DoorDirection.North;

    public void castBeam() {
        //place beam object
        this.originRoom.beamNeighbor(castDirection);
    }

    public void onRoomUpdate() {
        foreach(Room r in this.layout.getAllRooms()) {
            r.removeBeam();
        }

        this.castBeam();
    }
}
