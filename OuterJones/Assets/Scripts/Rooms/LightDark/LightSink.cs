using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSink : MonoBehaviour
{
    [SerializeField] private DoorDirection receiveBeamFrom;
    [SerializeField] private GameObject effectableObj;
    [SerializeField] private GameObject beamedSprite;
    [SerializeField] private GameObject notBeamedSprite;
    [SerializeField] private RoomListenerObjListener sinkListener;


    private LDPuzzle puzzle;
    private string id;

    public void init(LDPuzzle puzzle, string id) {
        this.puzzle = puzzle;
        this.id = id;
    }

    public void activate(DoorDirection beamFrom) {
        if(/*Door.rotateDoorDirection(Door.rotateDoorDirection(beamFrom, true), true)*/beamFrom == this.receiveBeamFrom) {
            this.effectableObj.GetComponent<Effectable>().onEffect();
            this.beamedSprite.SetActive(true);
            this.notBeamedSprite.SetActive(false);

            if(this.puzzle != null) {
                this.puzzle.onActive(this.id);
            }
        }

        if(this.sinkListener != null) {
            this.sinkListener.onRoomEvent(true);
        }
    }

    public void deactivate() {
        this.beamedSprite.SetActive(false);
        this.notBeamedSprite.SetActive(true);
        if(this.puzzle != null) {
            this.puzzle.onDeactivate(this.id);
        }
        
        this.effectableObj.GetComponent<Effectable>().onEffectOver();
        if(this.sinkListener != null) {
            this.sinkListener.onRoomEvent(false);
        }
    }

    public void rotate90(bool clockwise) {
        this.receiveBeamFrom = Door.rotateDoorDirection(this.receiveBeamFrom, clockwise);
    }

    //gets the incoming direction needed to active the sink
    public DoorDirection getIncomingDirectionToActivate() {
        // return Door.rotateDoorDirection(Door.rotateDoorDirection(this.receiveBeamFrom, true), true);
        return this.receiveBeamFrom;
    }

}
