using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomCoords {
    public int x; public int y;

    public RoomCoords(int x, int y) {
        this.x = x; this.y = y;
    }

    public RoomCoords getOffset(int dX, int dY) {
        return new RoomCoords(x + dX, y + dY);
    }
}

public class RoomsLayout : MonoBehaviour
{
    private Room[,] rooms;
    [SerializeField] private float positionOffset;
    int ROOM_GRID_X = 5;

    private List<RoomUpdateListener> listeners = new List<RoomUpdateListener>();

    public void Awake() {
        //place starting room in the grid
        this.rooms = new Room[ROOM_GRID_X, ROOM_GRID_X];

        GameObject obj = GameObject.Find("startingRoom");
        this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2] = obj.GetComponent<Room>();
        this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].init(new RoomCoords(ROOM_GRID_X/2, ROOM_GRID_X/2));
    }

    public void addRoomUpdateListener(RoomUpdateListener l) {
        this.listeners.Add(l);
    }

    //needs to be overriden for packman rooms
    public bool canPlaceRoom(Door origin, Room destination) {
        switch(origin.getDirection()) {
            case DoorDirection.North:
                if(origin.getPosition().getOffset(0, 1).y < ROOM_GRID_X - 1) {
                    return destination.hasDoorDirection(DoorDirection.South);
                }
                break;
            case DoorDirection.East:
                if(origin.getPosition().getOffset(1, 0).y < ROOM_GRID_X - 1) {
                    return destination.hasDoorDirection(DoorDirection.West);
                }
                break;
            case DoorDirection.West:
                if(origin.getPosition().getOffset(-1, 0).y < ROOM_GRID_X - 1) {
                    return destination.hasDoorDirection(DoorDirection.East);
                }
                break;
            case DoorDirection.South:
                if(origin.getPosition().getOffset(0, -1).y < ROOM_GRID_X - 1) {
                    return destination.hasDoorDirection(DoorDirection.North);
                }
                break;
        }
        return false;
    }

    public void placeRoom(Door origin, Room dest) {
        RoomCoords destPos;
        switch(origin.getDirection()) {
            case DoorDirection.North:
                destPos = origin.getPosition().getOffset(0, 1);
                break;
            case DoorDirection.East:
                destPos = origin.getPosition().getOffset(1, 0);
                break;
            case DoorDirection.West:
                destPos = origin.getPosition().getOffset(-1, 0);
                break;
            case DoorDirection.South:
                destPos = origin.getPosition().getOffset(0, -1);
                break;
            default:
                throw new InvalidOperationException("Invalid door direction!");
        }

        this.rooms[destPos.x, destPos.y] = dest;
        dest.init(destPos);

        Vector3 offset = new Vector3(positionOffset * (destPos.x - origin.getPosition().x), positionOffset * (destPos.y - origin.getPosition().y), 0);
        dest.transform.position = this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].transform.position + offset;

        foreach(RoomUpdateListener l in this.listeners) {
            l.onRoomUpdate(dest);
        }
    }

    public Room getRoomAt(int x, int y) {
        return this.rooms[x,y];
    }

    public List<Room> getAllRooms() {
        List<Room> all = new List<Room>();
        for(int i = 0; i < ROOM_GRID_X; i++) {
            for(int j = 0; j < ROOM_GRID_X; j++) {
                if(this.rooms[i,j] != null) {
                    all.Add(this.rooms[i,j]);
                }
            }
        }
        return all;
    }
}
