using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour, Effectable
{
    //room info
    [SerializeField] private LightDarkRoom originRoom;
    private BeamModel beam;
    [SerializeField] private DoorDirection castDirection = DoorDirection.North;
    [SerializeField] private LightSourceManager manager;
    
    private bool powered = false;
    

    public void Awake() {
        //we don't use the pool because this room always has a beam
        this.beam = this.manager.addLightSource(this);
        this.originRoom.setSource(this);
    }

    public void castBeam() {
        if(this.powered) {
            this.beam.initBeam(this.originRoom.transform, this.transform.position, this.originRoom.getPointInDirection(castDirection).position);

            this.originRoom.beamNeighbor(castDirection);
        }
    }

    public void rotate90(bool clockwise) {
        this.castDirection = Door.rotateDoorDirection(this.castDirection, clockwise);
    }

    public void onEffect() {
        this.powered = true;
        this.manager.onRoomUpdate(new List<Room>());
    }
}