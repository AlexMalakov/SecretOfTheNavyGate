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

public class Door : MonoBehaviour, InputSubscriber
{
    [SerializeField] private Door destination;
    [SerializeField] private Room room;
    [SerializeField] private DoorDirection direction;
    [SerializeField] private Transform enterPosition;

    [SerializeField] private GameObject openModel;
    [SerializeField] private GameObject closedModel;
    [SerializeField] private GameObject forceField;

    private List<DoorUseListener> listeners = new List<DoorUseListener>();

    private DoorDirection initialDirection;

    private PlayerIO input;
    private Player player;
    private bool forceFieldOn;
    
    public void Awake() {
        this.initialDirection = this.direction;
        this.input = FindObjectOfType<PlayerIO>();
        this.player = FindObjectOfType<Player>();
    }

    public void setDestination(Door newDestination) {
        this.destination = newDestination;
        this.openModel.SetActive(true);
        this.closedModel.SetActive(false);
    }

    public Door getDestination() {
        return this.destination;
    }

    public void addDoorUseListener(DoorUseListener listener) {
        this.listeners.Add(listener);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.checkDoorPlacement();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && this.input != null) {
            this.input.cancelRequest(this);
        }
    }

    public void checkDoorPlacement() {
        if(this.forceFieldOn) {
            this.input.requestPopUpAlert(this, this.transform, "something is stopping you from entering this room!");
        }
        if(this.destination == null) {
            Room next = this.player.getNextInDeck(this.room.getPosition().overworld);
            if(next != null && room.getLayoutManager().canPlaceRoom(this, next)) {
                this.input.requestSpaceInput(this, this.transform, "place room!");
            } else if(next == null) {
                this.input.requestPopUpAlert(this, this.transform, "cannot place room from empty deck!");
            } else {
                this.input.requestPopUpAlert(this, this.transform, "next room cannot be placed here!");
            }
        } else {
            this.input.requestSpaceInput(this, this.transform, "use door");
        }
    }

    public void onSpacePress() {
        useDoor();
    }

    public void useDoor() {
        if(this.destination == null) {
            Room next = this.player.getNextInDeck(this.room.getPosition().overworld);
            if(next != null && room.getLayoutManager().canPlaceRoom(this, next)) {
                this.player.removeNextInDeck();
                this.setDestination(next.getEntrance(this.getInverse()));
                this.destination.setDestination(this);
                room.getLayoutManager().placeRoom(this, next);
                this.onExit();
            } else if(next == null){
                return;
            } else {
                return;
            }
        }

        this.onExit();
        this.destination.onEnter(this.player);

        this.player.setCurrentRoom(this.destination.getRoom());
        foreach(DoorUseListener l in this.listeners) {
            l.onRoomEnter();
        }
    }

    private void onExit() {
        this.room.onExit();
    }

    public void onEnter(Player p) {
        p.transform.position = enterPosition.position;
        this.room.onEnter(this);
    }

    public Transform getEnterPos() {
        return this.enterPosition;
    }

    //TODO: this won't really work for packman rooms.
    //Actually not sure doors work for packman at all
    public void rotate90(bool clockwise) {
        this.openModel.SetActive(false);
        this.closedModel.SetActive(true);

        this.resetDestination();

        this.direction = rotateDirection(clockwise);
    }

    public void resetDestination() {
        if(this.destination != null) {
            this.destination.setDestination(null);
            this.destination = null;
        }
    }

    public void updateNeighbor() {
        RoomCoords neighborPos = this.room.getPosition().getOffset(this.direction);
        Room neighbor = this.room.getLayoutManager().getRoomAt(neighborPos);
        if(neighbor != null && neighbor.hasDoorDirection(this.getInverse())) {
            this.setDestination(neighbor.getEntrance(this.getInverse()));
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

    public void setForceField(bool active) {
        this.forceFieldOn = active;

        this.forceField.SetActive(this.forceFieldOn);
    }


}