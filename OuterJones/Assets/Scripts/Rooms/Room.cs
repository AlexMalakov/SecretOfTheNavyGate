using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    [SerializeField] private float roomLighting = .5f;
    [SerializeField] private Light2D globalLighting;
    // [SerializeField] private GameObject cameraObj;
    [SerializeField] protected RoomsLayout layoutManager;
    [SerializeField] protected List<Door> doors;

    protected RoomCoords position;

    public void init(RoomCoords position) {
        this.position = position;
    }

    public void onEnter() {
        this.gameObject.SetActive(true);
        // cameraObj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y ,this.cameraObj.transform.position.z);
        globalLighting.intensity = this.roomLighting;
    }

    public void onExit() {
        this.gameObject.SetActive(false);
    }

    public RoomsLayout getLayoutManager() {
        return this.layoutManager;
    }

    public RoomCoords getPosition() {
        return this.position;
    }

    public bool hasDoorDirection(DoorDirection direction) {
        foreach(Door d in this.doors) {
            if(d.getDirection() == direction) {
                return true;
            }
        }
        return false;
    }

    public Door getEntrance(DoorDirection direction) {
        foreach(Door d in this.doors) {
            if(d.getDirection() == direction) {
                return d;
            }
        }
        throw new InvalidOperationException("DOOR WITH THE DIRECTION " + direction + "  DOES NOT EXIST IN ROOM " + gameObject.name);
    }
}