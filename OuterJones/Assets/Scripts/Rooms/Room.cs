using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    [SerializeField] protected float roomLighting = .5f;
    [SerializeField] private Light2D globalLighting;
    // [SerializeField] private GameObject cameraObj;
    [SerializeField] protected RoomsLayout layoutManager;
    [SerializeField] protected List<Door> doors;
    [SerializeField] protected Sprite roomSprite;

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

    /////////////////////////////////////////////
    //adding this functionality to all rooms incase we want canals in non water rooms, or similar shananigans
    public virtual void onFlood(List<CanalEntrances> entrances) {}

    public virtual void drainWater() {}

    public virtual void floodNeighbors(List<CanalEntrances> exits) {}


    //////////////////////////////////////////////
    //functionality for L/D rooms

    public virtual void receiveBeam(DoorDirection incomingDirection) {}

    public virtual void removeBeam() {}
    
}