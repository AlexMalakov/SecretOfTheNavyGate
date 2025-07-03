using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    [Header ("Room info")]
    [SerializeField] protected float roomLighting = .5f;
    [SerializeField] private Light2D globalLighting;
    // [SerializeField] private GameObject cameraObj;
    [SerializeField] protected RoomsLayout layoutManager;
    [SerializeField] protected List<Door> doors;
    [SerializeField] protected Sprite roomSprite;

    [Header ("Beam target transforms")]
    [SerializeField] private Transform northPosition;
    [SerializeField] private Transform eastPosition;
    [SerializeField] private Transform southPosition;
    [SerializeField] private Transform westPosition;

    protected RoomCoords position;

    public virtual void init(RoomCoords position) {
        this.position = position;

        foreach(Door d in this.doors) {
            if(d.getDestination() == null 
                && this.layoutManager.getRoomAt(this.position.getOffset(d.getDirection())) != null
                && this.layoutManager.getRoomAt(this.position.getOffset(d.getDirection())).hasDoorDirection(d.getInverse())) {

                
                d.setDestination(this.layoutManager.getRoomAt(this.position.getOffset(d.getDirection())).getEntrance(d.getInverse()));
                d.getDestination().getRoom().getEntrance(d.getInverse()).setDestination(d);
            }
        }
    }

    public virtual void onEnter(Door enteredFrom) {
        this.gameObject.SetActive(true);
        // cameraObj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y ,this.cameraObj.transform.position.z);
        globalLighting.intensity = this.roomLighting;
    }

    public virtual void onExit() {
        this.gameObject.SetActive(false);
    }


    public bool hasDoorDirection(DoorDirection direction) {
        foreach(Door d in this.doors) {
            if(d.getDirection() == direction) {
                return true;
            }
        }
        return false;
    }

    public RoomsLayout getLayoutManager() {
        return this.layoutManager;
    }

    public RoomCoords getPosition() {
        return this.position;
    }

    public Door getEntrance(DoorDirection direction) {
        foreach(Door d in this.doors) {
            if(d.getDirection() == direction) {
                return d;
            }
        }
        throw new InvalidOperationException("DOOR WITH THE DIRECTION " + direction + "  DOES NOT EXIST IN ROOM " + gameObject.name);
    }

    public virtual Sprite getRoomSprite() {
        return this.roomSprite;
    }

    public List<Door> getDoors() {
        return this.doors;
    }

    public Transform getPointInDirection(DoorDirection d) {
        switch(d) {
            case DoorDirection.North:
                return this.northPosition;
            case DoorDirection.East:
                return this.eastPosition;
            case DoorDirection.West:
                return this.westPosition;
            case DoorDirection.South:
                return this.southPosition;
        }

        Debug.Log("IMPOSSIBLE DIRECTION!");
        return null;
    }

    ///////////////////////////////////////////// CANAL ROOMS
    [Header ("Canal Info")]
    [SerializeField] protected List<CanalEntrances> canalEntrances;
    [SerializeField] protected List<Canal> canals;
    //since canals can exist in non water rooms, all water functionality gets to live in room :'(


    public virtual void onFlood(CanalEntrances floodingFrom) {
        foreach(Canal c in this.canals) {
            if(c.willFlood(floodingFrom)) {
                c.onFlood(floodingFrom);
            }
        }
    }

    public virtual void drainWater() {
        foreach(Canal c in this.canals) {
            c.drainWater();
        }
    }

    public virtual void floodNeighbors(List<CanalEntrances> exits) {
        foreach(CanalEntrances exit in exits) {
            if(this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1]) != null) {
                CanalEntrances opposite = (CanalEntrances)(((int)exit + (WaterSource.CANAL_ENTRANCE_COUNT/2)) % WaterSource.CANAL_ENTRANCE_COUNT);
                this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1]).onFlood(opposite);
            }
        }
    }

    private void rotateCanals90(bool clockwise) {
        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((WaterSource.CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % WaterSource.CANAL_ENTRANCE_COUNT);
        }
        foreach(Canal c in this.canals) {
            c.rotate90(clockwise);
        }
    }


    //////////////////////////////////////////////
    //functionality for L/D rooms
    [Header ("LD Info")]
    [SerializeField] protected Mirror mirror;
    [SerializeField] protected LightSink lSink;
    protected List<BeamModel> beams = new List<BeamModel>();

    //this is a chungus of a method cuz of mirrors and non mirrors :'(
    //since all rooms can have mirrors/sinks then a lot of code gets to be moved here yipee I love big classes!!!!!! :D
    //incoming direction is the door from which the beam is coming from
    public virtual void receiveBeam(DoorDirection incomingDirection) {

        //only receive beam if we have a door that it can enter through
        if(this.hasDoorDirection(this.getEntrance(incomingDirection).getInverse())) {
    
            //if we have a sink power it, then pass the b
            if(this.lSink != null) {
                this.lSink.activate(incomingDirection);

                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);
                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.lSink.transform.position);
                return; //the beam "ends here"
            }

            DoorDirection exitDirection;
            if(this.mirror != null) { //if we have a mirror, we draw the light as if it bounces
                exitDirection = this.mirror.reflect(incomingDirection); //exit direction is wherever we get reflected

                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);

                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.mirror.transform.position);

                BeamModel bb = BeamPool.getBeam();
                this.beams.Add(bb);

                bb.initBeam(
                    this.transform,
                    this.mirror.transform.position,
                    this.getPointInDirection(exitDirection).position);
            } else {
                exitDirection = this.getEntrance(incomingDirection).getInverse(); //exit direction is opposite of enter direction
                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);
                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.getPointInDirection(exitDirection).position);
            }

            //if we have a door at our exit direction, we'll send the beam through
            if(this.hasDoorDirection(exitDirection)) {
                this.beamNeighbor(exitDirection);
            }
        }
    }

    
    public virtual void beamNeighbor(DoorDirection exitDirection) {
        if(this.layoutManager.getRoomAt(this.position.getOffset(exitDirection)) != null) {

            DoorDirection arrivedFrom = Door.rotateDoorDirection(Door.rotateDoorDirection(exitDirection, true), true); //im too lazy to right a static inverse
            this.layoutManager.getRoomAt(this.position.getOffset(exitDirection)).receiveBeam(arrivedFrom);
        }
    }

    public virtual void removeBeam() {
        if(this.lSink != null) {
            this.lSink.deactivate();
        }

        for(int i = 0; i < this.beams.Count; i++) {
            this.beams[i].killBeam();
        }

        this.beams = new List<BeamModel>();
    }

    public virtual void rotateLight90(bool clockwise) {
        if(this.mirror != null) {
            this.mirror.rotate90();
        }

        if(this.lSink != null) {
            this.lSink.rotate90(clockwise);
        }
    }

    ///////////////////////////////////////////////
    //rotation room functionality

    public virtual bool rotate90() {
        return this.rotate90(FindObjectOfType<Player>().getRotationDirection());
    }

    public virtual bool rotate90(bool clockwise) {
        
        //rotate game object
        transform.Rotate(0f, 0f, (clockwise ? -90f : 90f));
    
        //rotate doors
        foreach(Door d in this.doors) {
            d.rotate90(clockwise);
        }

        //rotate beam transforms :)
        Transform swapper = northPosition;
        if(clockwise) {
            northPosition = westPosition;
            westPosition = southPosition;
            southPosition = eastPosition;
            eastPosition = swapper;
        } else {
            northPosition = eastPosition;
            eastPosition = southPosition;
            southPosition = westPosition;
            westPosition = swapper;
        }

        rotateCanals90(clockwise);

        rotateLight90(clockwise);

        //handles canal and light reset, and map rotate
        this.layoutManager.notifyRoomListeners(new List<Room>(){this});

        return clockwise;
    }
    
}