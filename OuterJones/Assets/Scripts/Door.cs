using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public enum DoorDirection {
    North, East, West, South
}

public interface DoorUseListener {
    void onRoomEnter();
}

public class Door : MonoBehaviour 
{
    [SerializeField] private Door destination;
    [SerializeField] private Room room;
    [SerializeField] private DoorDirection direction;
    [SerializeField] private Transform enterPosition;

    private List<DoorUseListener> listeners = new List<DoorUseListener>();

    private DoorDirection initialDirection;
    
    public void Awake() {
        this.initialDirection = this.direction;
    }

    public void setDestination(Door newDestination) {
        this.destination = newDestination;
    }

    public Door getDestination() {
        return this.destination;
    }

    public void addDoorUseListener(DoorUseListener listener) {
        this.listeners.Add(listener);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(/*Input.GetKeyDown(KeyCode.Space) && */other.gameObject.GetComponent<Player>() != null) {
            this.useDoor(other.gameObject.GetComponent<Player>());
        }
    }

    public void useDoor(Player player) {
        if(this.destination == null) {
            Room next = player.getNextInDeck();
            if(next != null && room.getLayoutManager().canPlaceRoom(this, next)) {
                player.removeNextInDeck();
                this.setDestination(next.getEntrance(this.getInverse()));
                this.destination.setDestination(this);
                room.getLayoutManager().placeRoom(this, next);
                this.onExit();
            } else if(next == null){
                //TODO: display a message on the screen maybe?
                return;
            } else {
                return;
            }
        }

        this.onExit();
        this.destination.onEnter(player);

        player.setCurrentRoom(this.destination.getRoom());
        foreach(DoorUseListener l in this.listeners) {
            l.onRoomEnter();
        }
    }

    private void onExit() {
        this.room.onExit();
    }

    public void onEnter(Player player) {
        player.transform.position = enterPosition.position;
        this.room.onEnter();
    }

    //TODO: this won't really work for packman rooms.
    //Actually not sure doors work for packman at all
    public void rotate90(bool clockwise) {
        this.destination.setDestination(null);
        this.destination = null;

        this.direction = rotateDirection(clockwise);

        RoomCoords neighborPos = this.room.getPosition().getOffset(this.direction);
        Room neighbor = this.room.getLayoutManager().getRoomAt(neighborPos);
        if(neighbor.hasDoorDirection(this.direction)) {
            this.setDestination(neighbor.getEntrance(this.direction));
            this.destination.setDestination(this);
        }
    }

    public DoorDirection getDirection() {
        return this.direction;
    }

    public DoorDirection getInverse() {
        switch(this.direction) {
            case DoorDirection.North:
                return DoorDirection.South;
            case DoorDirection.South:
                return DoorDirection.North;
            case DoorDirection.East:
                return DoorDirection.West;
            case DoorDirection.West:
                return DoorDirection.East;
        }
        throw new InvalidOperationException("direction does not exist");
    }

    public DoorDirection rotateDirection(bool clockwise) {
        return rotateDoorDirection(this.direction, clockwise); 
    }

    public static DoorDirection rotateDoorDirection(DoorDirection d, bool clockwise) {
        switch(d) {
            case DoorDirection.North:
                if(clockwise)
                    return DoorDirection.East;
                return DoorDirection.West;
            case DoorDirection.South:
                if(clockwise)
                    return DoorDirection.West;
                return DoorDirection.East;
            case DoorDirection.East:
                if(clockwise)
                    return DoorDirection.South;
                return DoorDirection.North;
            case DoorDirection.West:
                if(clockwise)
                    return DoorDirection.North;
                return DoorDirection.South;
        }
        throw new InvalidOperationException("direction does not exist");
    }

    public Room getRoom() {
        return this.room;
    }

    public RoomCoords getPosition() {
        return this.room.getPosition();
    }


}