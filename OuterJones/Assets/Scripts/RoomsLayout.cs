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

    public RoomCoords getOffset(DoorDirection d) {
        switch(d) {
            case DoorDirection.North:
                return this.getOffset(0,1);
            case DoorDirection.South:
                return this.getOffset(0,-1);
            case DoorDirection.East:
                return this.getOffset(1,0);
            case DoorDirection.West:
                return this.getOffset(-1,0);
        }

        return this;
    }
}

public class RoomsLayout : MonoBehaviour
{
    private Room[,] rooms;
    [SerializeField] private float positionOffset;
    [SerializeField] private GameObject cameraObj;
    public static int ROOM_GRID_X = 5;

    private List<RoomUpdateListener> listeners = new List<RoomUpdateListener>();
    private List<RoomUpdateListener> postListeners = new List<RoomUpdateListener>();

    public void Start() {
        //place starting room in the grid
        this.rooms = new Room[ROOM_GRID_X, ROOM_GRID_X];

        GameObject obj = GameObject.Find("startingRoom");
        this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2] = obj.GetComponent<Room>();
        this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].init(new RoomCoords(ROOM_GRID_X/2, ROOM_GRID_X/2));
        this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].onEnter(null);
    }

    public void addRoomUpdateListener(RoomUpdateListener l) {
        this.listeners.Add(l);
    }

    public void addPostRoomUpdateListener(RoomUpdateListener l) {
        this.postListeners.Add(l);
    }

    //needs to be overriden for packman rooms
    public bool canPlaceRoom(Door origin, Room destination) {

        RoomCoords newPos = origin.getPosition().getOffset(origin.getDirection());
        if(origin.getRoom() is PackmanRoom) {
            if(PackmanRoom.isPackmanPlace(origin, ROOM_GRID_X, ROOM_GRID_X)) {
                newPos = getPackmanCoords(origin);
            }

            return this.rooms[newPos.x, newPos.y] == null && destination.hasDoorDirection(origin.getInverse());
        }

        if(newPos.x >= 0 && newPos.x < ROOM_GRID_X && newPos.y >= 0 && newPos.y < ROOM_GRID_X) {
            return this.rooms[newPos.x, newPos.y] == null && destination.hasDoorDirection(origin.getInverse());
        }
        return false;

    }

    public void placeRoom(Door origin, Room dest) {
        
        RoomCoords destPos;
        if(origin.getRoom() is PackmanRoom && PackmanRoom.isPackmanPlace(origin, ROOM_GRID_X, ROOM_GRID_X)) {
            destPos = getPackmanCoords(origin);
        } else {
            destPos = origin.getPosition().getOffset(origin.getDirection());
        }

        
        this.rooms[destPos.x, destPos.y] = dest;
        dest.init(destPos);

        this.moveRoomToSpot(dest, destPos);

        this.notifyRoomListeners(new List<Room>(){dest});
    }

    public void notifyRoomListeners(List<Room> r) {
        foreach(RoomUpdateListener l in this.listeners) {
            l.onRoomUpdate(r);
        }

        foreach(RoomUpdateListener pl in this.postListeners) {
            pl.onRoomUpdate(r);
        }
    }

    private void moveRoomToSpot(Room r, RoomCoords c) {
        //possible bug in the position placing?
        RoomCoords centerPos = this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].getPosition();
        Vector3 offset = new Vector3(this.positionOffset * (c.x - centerPos.x), this.positionOffset * (c.y - centerPos.y), 0);
        r.transform.position = this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].transform.position + offset;
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

    public Room getRoomAt(RoomCoords c) {
        return this.getRoomAt(c.x, c.y);
    }

    public Room getRoomAt(int x, int y) {
        if(x < 0 || x >= ROOM_GRID_X || y < 0 || y >= ROOM_GRID_X) {
            if(this.rooms[(x + ROOM_GRID_X) % ROOM_GRID_X, (y + ROOM_GRID_X) % ROOM_GRID_X] is PackmanRoom) {
                return this.rooms[(x + ROOM_GRID_X) % ROOM_GRID_X, (y + ROOM_GRID_X) % ROOM_GRID_X];
            }
            return null;
        }
        return this.rooms[x,y];
    }

    public Room getRoomFromPackman(RoomCoords c) {
        return this.getRoomFromPackman(c.x, c.y);
    }

    public Room getRoomFromPackman(int x, int y) {
        return this.rooms[(x + ROOM_GRID_X) % ROOM_GRID_X, (y + ROOM_GRID_X) % ROOM_GRID_X];
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



    //TODO: FIX THE CASE WHERE UNPLACED ROOMS GET SHIFTED AROUND!!!!!!!!
    public void slideRoomsAroundCenter(RoomCoords center, bool clockwise) {
        //Enforces that the slide room must not be on an edge
        //delete to allow it to be on an edge
        Debug.Log("ORIGIN!" + center.x + ", " + center.y);
        if(center.x == 0 || center.x == ROOM_GRID_X || center.y == 0 || center.y == ROOM_GRID_X) {
            return;
        }

        List<RoomCoords> roomsToShift = new List<RoomCoords>();
        List<Room> toUpdate = new List<Room>();

        List<int[]> offsets = new List<int[]>(){
            new int[]{1,1},
            new int[]{1,0},
            new int[]{1,-1},
            new int[]{0,-1},
            new int[]{-1,-1},
            new int[]{-1,0},
            new int[]{-1,1},
            new int[]{0,1},
        };

        if(!clockwise) {
            offsets.Reverse();
        }

        foreach(int[] offset in offsets) {
            if(center.x - offset[0] >= 0 && center.x - offset[0] < ROOM_GRID_X
                && center.y - offset[1] >= 0 && center.y - offset[1] < ROOM_GRID_X) {

                roomsToShift.Add(center.getOffset(offset[0], offset[1]));
            }
        }

        Room swapper = this.rooms[roomsToShift[0].x, roomsToShift[0].y];

        for(int i = 0; i < roomsToShift.Count-1; i++) {
            //update list in internal grid
            this.rooms[roomsToShift[i].x, roomsToShift[i].y] = this.rooms[roomsToShift[i+1].x, roomsToShift[i+1].y];

            if(this.rooms[roomsToShift[i].x, roomsToShift[i].y] != null) {
                this.rooms[roomsToShift[i].x, roomsToShift[i].y].init(roomsToShift[i]);
                this.moveRoomToSpot(this.rooms[roomsToShift[i].x, roomsToShift[i].y], roomsToShift[i]);
                toUpdate.Add(this.rooms[roomsToShift[i].x, roomsToShift[i].y]);
            }
            
        }

        this.rooms[roomsToShift[roomsToShift.Count-1].x, roomsToShift[roomsToShift.Count-1].y] = swapper;

        if(swapper != null) {
            swapper.init(roomsToShift[roomsToShift.Count-1]);
            toUpdate.Add(swapper);
            this.moveRoomToSpot(swapper, roomsToShift[roomsToShift.Count-1]);
        }
            
        this.notifyRoomListeners(toUpdate);
    }



    public GameObject getCam() {
        return this.cameraObj;
    }
}
