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

    public RoomCoords getOffest(DoorDirection d) {
        switch(this.direction) {
            case DoorDirection.North:
                return this.getOffest(0,1);
            case DoorDirection.South:
                return this.getOffest(0,-1);
            case DoorDirection.East:
                return this.getOffest(1,0);
            case DoorDirection.West:
                return this.getOffest(-1,0);
        }

        return this;
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

        if(origin.getRoom() is PackmanRoom) {
            return destination.hasDoorDirection(origin.getInverse());
        }

        RoomCoods newPos = origin.getPosition().getOffset(origin.getDirection());

        if(newPos.x >= 0 && newPos.x < ROOM_GRID_X && newPos.y >= 0 && newPos.y < ROOM_GRID_X) {
            return destination.hasDoorDirection(origin.getInverse());
        }
        return false;

    }

    public void placeRoom(Door origin, Room dest) {

        RoomCoords destPos;
        if(origin.getRoom() is PackmanRoom && PackmanRoom.isPackmanPlace(origin, ROOM_GRID_X, ROOM_GRID_X)) {
            destPos = getPackmanCoords(origin);
        } else {
            destPos = origin.getPosition().getOffest(origin.getDirection());
        }

        
        this.rooms[destPos.x, destPos.y] = dest;
        dest.init(destPos);

        this.moveRoomToSpot(dest, destPos);

        foreach(RoomUpdateListener l in this.listeners) {
            l.onRoomUpdate(dest);
        }
    }

    private void moveRoomToSpot(Room r, RoomCoords c) {
        //possible bug in the position placing?
        Vector3 offset = new Vector3(this.positionOffset * (c.x - this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].x), this.positionOffset * (c.y - this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].y), 0);
        dest.transform.position = this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].transform.position + offset;
    }

    private RoomCoords getPackmanCoords(Door origin) {
        RoomCoords destPos;
        switch(origin.getDirection()) {
            case DoorDirection.North:
                destPos = new RoomCoords(origin.getPosition().x, 0);
                break;
            case DoorDirection.East:
                destPos = new RoomCoords(0, origin.getPosition().y);
                break;
            case DoorDirection.West:
                destPos = new RoomCoords(ROOM_GRID_X-1, origin.getPosition().y);
                break;
            case DoorDirection.South:
                destPos = new RoomCoords(origin.getPosition().y, ROOM_GRID_X-1);
                break;
            default:
                throw new InvalidOperationException("Invalid door direction!");
        }

        return destPos;
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

    public void shiftRoomPositionsAround(RoomCoords center, bool clockwise) {
        List<RoomCoords> roomsToShift = new List<RoomCoords>();

        List<int[]> offsets = new List<int[]>(){
            new int[]{1,1},
            new int[]{1,0},
            new int[]{1,-1},
            new int[]{0,-1},
            new int[]{-1,-1},
            new int[]{-1,0},
            new int[]{-1,1},
            new int[]{0,1},
        }

        if(!clockwise) {
            offsets = offests.Reverse();
        }

        for(int[] offset in offsets) {
            if(center.x - offset[0] >= 0 && center.x - offset[0] < ROOM_GRID_X
                && center.y - offset[1] >= 0 && center.y - offset[1] < ROOM_GRID_X) {

                roomsToShift.Add(center.getOffset(offset[0], offset[1]));
            }
        }

        Room swapper = this.rooms[roomsToShift[0].x, roomsToShift[0].y];

        for(int i = 0; i < roomsToShift.Count-1; i++) {
            //update list in internal grid
            this.rooms[roomsToShift[i].x, roomsToShift[i].y] = this.rooms[roomsToShift[i+1].x, roomsToShift[i+1].y];

            //change room physical position
            this.moveRoomToSpot(this.rooms[roomsToShift[i].x, roomsToShift[i].y], roomsToSift[i]);
            
            //TODO: update map
        }

        this.rooms[roomsToShift[roomsToShift.Count-1].x, roomsToShift[roomsToShift.Count-1].y] = swapper;

        this.moveRoomToSpot(swapper, roomsToSift[roomsToShift.Count-1]);
            
        //TODO: update map
    }
}
