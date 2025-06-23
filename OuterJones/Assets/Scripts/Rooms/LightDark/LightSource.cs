using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour, RoomUpdateListener
{
    [SerializeField] private Room originRoom;
    [SerializeField] private RoomsLayout layout;

    [SerializeField] private GameObject beamModel; //need to draw a lien between 2 points

    private BeamModel beam;

    private DoorDirection castDirection = DoorDirection.North;

    public void castBeam() {
        GameObject newBeam = Instantiate(beamModel);

        this.beam = newBeam.GetComponent<BeamModel>();
        this.beam.initBeam(this.transform.position, this.originRoom.getPointInDirection(castDirection).position);

        this.originRoom.beamNeighbor(castDirection);
    }

    public void onRoomUpdate(Room r) {
        Destroy(this.beam.gameObject);
        
        foreach(Room room in this.layout.getAllRooms()) {
            room.removeBeam();
        }

        this.castBeam();
    }

    public BeamModel getBeam() {
        GameObject newBeam = Instantiate(beamModel);
        return newBeam.GetComponent<BeamModel>();
    }
}
