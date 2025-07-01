using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSink : MonoBehaviour
{
    private bool beamed = false;
    [SerializeField] DoorDirection receiveBeamFrom;

    public void activate(DoorDirection beamFrom) {

        if(Door.rotateDoorDirection(Door.rotateDoorDirection(beamFrom, true), true) == this.receiveBeamFrom) {
            this.beamed = true;
        }
    }

    public void deactivate() {
        this.beamed = false;
    }

    public bool getActive() {
        return this.beamed;
    }

    public void rotate90(bool clockwise) {
        this.receiveBeamFrom = Door.rotateDoorDirection(this.receiveBeamFrom, clockwise);
    }

}
