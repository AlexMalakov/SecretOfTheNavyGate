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
    }

    public virtual void onEnter() {
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
            if(this.layoutManager.getRoomAt(this.position.x + WaterRoom.CANAL_N_MAP[exit][0], this.position.y + WaterRoom.CANAL_N_MAP[exit][1]) != null) {
                this.layoutManager.getRoomAt(this.position.x + WaterRoom.CANAL_N_MAP[exit][0], this.position.y + WaterRoom.CANAL_N_MAP[exit][1]).onFlood(exit);
            }
        }
    }

    private void rotateCanals90(bool clockwise) {
        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((WaterRoom.CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % WaterRoom.CANAL_ENTRANCE_COUNT);
        }
        foreach(Canal c in this.canals) {
            c.rotate90(clockwise);
        }
    }


    //////////////////////////////////////////////
    //functionality for L/D rooms
    [SerializeField] protected List<BeamModel> beams = new List<BeamModel>();

    //im assuming non L/D cannot have mirrors in them?
    public virtual void receiveBeam(DoorDirection incomingDirection) {
        if(this.hasDoorDirection(this.getEntrance(incomingDirection).getInverse())) {
            BeamModel b = BeamPool.getBeam();
            this.beams.Add(b);
            b.initBeam(
                this.transform,
                this.getPointInDirection(incomingDirection).position,
                this.getPointInDirection(this.getEntrance(incomingDirection).getInverse()).position);
        }
    }

    public virtual void beamNeighbor(DoorDirection exitDirection) {
        if(this.layoutManager.getRoomAt(this.position.getOffset(exitDirection).x, this.position.getOffset(exitDirection).y) != null) {
            this.layoutManager.getRoomAt(this.position.getOffset(exitDirection).x, this.position.getOffset(exitDirection).y).receiveBeam(exitDirection);
        }
    }

    public virtual void removeBeam() {
        for(int i = 0; i < this.beams.Count; i++) {
            this.beams[i].killBeam();
        }

        this.beams = new List<BeamModel>();
    }

    ///////////////////////////////////////////////
    //rotation room functionality

    public virtual void rotate90(bool clockwise) {
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

        //handles canal and light reset, and map rotate
        this.layoutManager.notifyRoomListeners(new List<Room>(){this});
    }
    
}