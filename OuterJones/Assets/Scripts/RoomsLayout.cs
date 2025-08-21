using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomCoords {
    public int x; public int y; public bool overworld;

    public RoomCoords(int x, int y, bool overworld) {
        this.x = x; this.y = y; this.overworld = overworld;
    }

    public RoomCoords getOffset(int dX, int dY) {
        return new RoomCoords(x + dX, y + dY, this.overworld);
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

    public RoomCoords swapFloor() {
        return new RoomCoords(this.x, this.y, !this.overworld);
    }
}

public class RoomsLayout : MonoBehaviour
{
    private Room[,] rooms;
    private Room[,] underbelly;
    [SerializeField] private float positionOffset;
    [SerializeField] private GameObject cameraObj;
    public static int ROOM_GRID_X = 5;

    private List<RoomUpdateListener> listeners = new List<RoomUpdateListener>();
    private List<RoomUpdateListener> postListeners = new List<RoomUpdateListener>();

    public void Start() {
        //place starting room in the grid
        this.rooms = new Room[ROOM_GRID_X, ROOM_GRID_X];
        this.underbelly = new Room[ROOM_GRID_X, ROOM_GRID_X];

        GameObject obj = GameObject.Find("startingRoom");

        RoomCoords startingPos = new RoomCoords(ROOM_GRID_X/2, ROOM_GRID_X/2, true);
        this.helpPlaceRoom(obj.GetComponent<Room>(), startingPos);

        this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].onEnter(null);
    }

    private helpPlaceRoom(Room r, RoomCoords pos) {
        Room overR;
        Room underR;
        if(pos.overworld) {
            overR = r;
            underR = r.getPair();
        } else {
            overR = r.getPair();
            underR = r;
        }

        this.rooms[pos.x, pos.y] = overR;
        overR.init(pos.overworld ? pos : pos.swapFloor());
        this.moveRoomToSpot(overR);

        this.underbelly[pos.x, pos.y] = underR;
        underR.init(!pos.overworld ? pos : pos.swapFloor());
        this.moveRoomToSpot(underR);
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

        this.helpPlaceRoom(dest, destPos);

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

    private void moveRoomToSpot(Room r) {
        //possible bug in the position placing?
        RoomCoords c = r.getPosition();
        RoomCoords centerPos = this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].getPosition();
        Vector3 offset = new Vector3(this.positionOffset * (c.x - centerPos.x), this.positionOffset * (c.y - centerPos.y), 0);
        r.transform.position = this.rooms[ROOM_GRID_X/2, ROOM_GRID_X/2].transform.position + offset;
    }

    private RoomCoords getPackmanCoords(Door origin) {
        RoomCoords destPos;
        switch(origin.getDirection()) {
            case DoorDirection.North:
                destPos = new RoomCoords(origin.getPosition().x, 0, origin.getPosition().overworld);
                break;
            case DoorDirection.East:
                destPos = new RoomCoords(0, origin.getPosition().y, origin.getPosition().overworld);
                break;
            case DoorDirection.West:
                destPos = new RoomCoords(ROOM_GRID_X-1, origin.getPosition().y, origin.getPosition().overworld);
                break;
            case DoorDirection.South:
                destPos = new RoomCoords(origin.getPosition().y, ROOM_GRID_X-1, origin.getPosition().overworld);
                break;
            default:
                throw new InvalidOperationException("Invalid door direction!");
        }

        return destPos;
    }

    public Room getRoomAt(RoomCoords c) {
        return this.getRoomAt(c.x, c.y, c.overworld);
    }

    public Room getRoomAt(int x, int y, bool overworld) {
        Room[,] checking = overworld? this.rooms : this.underbelly;
        if(x < 0 || x >= ROOM_GRID_X || y < 0 || y >= ROOM_GRID_X) {
            if(checking[(x + ROOM_GRID_X) % ROOM_GRID_X, (y + ROOM_GRID_X) % ROOM_GRID_X] is PackmanRoom) {
                return checking[(x + ROOM_GRID_X) % ROOM_GRID_X, (y + ROOM_GRID_X) % ROOM_GRID_X];
            }
            return null;
        }
        return checking[x,y];
    }

    public Room getRoomFromPackman(RoomCoords c) {
        return this.getRoomFromPackman(c.x, c.y, c.overworld);
    }

    public Room getRoomFromPackman(int x, int y, bool overworld) {
        return overworld ? 
                this.rooms[(x + ROOM_GRID_X) % ROOM_GRID_X, (y + ROOM_GRID_X) % ROOM_GRID_X] : 
                this.underbelly[(x + ROOM_GRID_X) % ROOM_GRID_X, (y + ROOM_GRID_X) % ROOM_GRID_X];
    }

    public List<Room> getAllRooms(bool overworld) {
        List<Room> all = new List<Room>();
        for(int i = 0; i < ROOM_GRID_X; i++) {
            for(int j = 0; j < ROOM_GRID_X; j++) {
                if(this.rooms[i,j] != null) {
                    all.Add(overworld ? this.rooms[i,j] : this.underbelly[i,j]);
                }
            }
        }
        return all;
    }

    //not changing this with underbelly rewrite because it doesn't matter if im rotating the top or the bottom since they move together
    public void slideRoomsAroundCenter(RoomCoords center, bool clockwise) {
        Dictionary<RoomCoords, Room> roomsToShift = new Dictionary<RoomCoords, Room>();
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

        if(clockwise) {
            offsets.Reverse();
        }

        foreach(int[] offset in offsets) {
            if(center.x + offset[0] >= 0 && center.x + offset[0] < ROOM_GRID_X
                && center.y + offset[1] >= 0 && center.y + offset[1] < ROOM_GRID_X) {

                roomsToShift.Add(center.getOffset(offset[0], offset[1]), this.getRoomAt(center.getOffset(offset[0], offset[1])));
            }
        }

        List<RoomCoords> keys = new List<RoomCoords>(roomsToShift.Keys);

        for(int i = 0; i < keys.Count; i++) {
            Room r = roomsToShift[keys[(i+1) % keys.Count]];
            this.rooms[keys[i].x, keys[i].y] = r;
            if(r != null) {
                this.helpPlaceRoom(r, keys[i]);
                toUpdate.Add(r);
            }
        }
        
        foreach(Room r in toUpdate) {
            r.resetAllDoors();
        }
        this.rooms[center.x, center.y].resetAllDoors();

        FindObjectOfType<Map>().wipeSquares(keys);
        this.notifyRoomListeners(toUpdate);
    }



    public GameObject getCam() {
        return this.cameraObj;
    }
}
