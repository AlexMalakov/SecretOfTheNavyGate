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

    [SerializeField] private GameObject doorLight;
    [SerializeField] private GameObject forceFieldOther;

    private List<DoorUseListener> listeners = new List<DoorUseListener>();

    private DoorDirection initialDirection;

    private PlayerIO input;
    private Player player;
    private bool forceFieldOn;
    
    public void Awake() {
        this.initialDirection = this.direction;
        this.input = FindObjectOfType<PlayerIO>();
        this.player = FindObjectOfType<Player>();
        if(this.room is LightDarkRoom) {
            this.doorLight.SetActive(true);
        } 

        this.listeners.Add(FindObjectOfType<DeckUI>());
        this.listeners.Add(FindObjectOfType<RoomsLayout>());
    }

    public void setDestination(Door newDestination) {
        this.destination = newDestination;
        this.openModel.SetActive(this.destination != null);
        this.closedModel.SetActive(this.destination == null);
        if(this.destination != null) {
            this.destination.setOtherForceField(this.forceFieldOn);
        }
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
            this.input.cancelInputRequest(this);
        }
    }

    public void checkDoorPlacement() {
        if(this.forceFieldOn) {
            this.input.requestPopUpAlert(this.transform, "A strange field is stoping you from leaving!");
            return;
        }

        if(this.destination == null) {
            Room next = this.player.getNextInDeck(this.room.getPosition().overworld);
            if(next != null && room.getLayoutManager().canPlaceRoom(this, next)) {
                if(this.player.getNextInDeck(this.room.getPosition().overworld).getEntrance(this.getInverse()) != null
                    && this.player.getNextInDeck(this.room.getPosition().overworld).getEntrance(this.getInverse()).hasForceField()) {

                    this.input.requestPopUpAlert(this.transform, "A strange field is stoping you from placing!");
                    return;
                } 
                else if(this.player.getNextInDeck(this.room.getPosition().overworld).getEntrance(this.getInverse()) == null){
                    throw new InvalidOperationException("impossibile event? where we're able to place a room but also it doesn't have a door?");
                }
                this.input.requestSpaceInput(this, this.transform, "place room!");
            } else if(next == null) {
                this.input.requestPopUpAlert(this.transform, "cannot place room from empty deck!");
            } else {
                this.input.requestPopUpAlert(this.transform, "next room cannot be placed here!");
            }
        } else if(this.destination.hasForceField()) {
            this.input.requestPopUpAlert(this.transform, "A strange field is stoping you from entering!");
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
            if(next != null 
                            && next.hasDoorDirection(this.getInverse())
                            && next.getEntrance(this.getInverse()) != null
                            && next.getEntrance(this.getInverse()).hasForceField()) {
                return;
            }
            else if(next != null && room.getLayoutManager().canPlaceRoom(this, next)) {
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
        } else if(this.destination.hasForceField() || this.hasForceField()) {
            return;
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
            this.openModel.SetActive(false);
            this.closedModel.SetActive(true);
            this.destination.setOtherForceField(false);
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
        } else {
            this.openModel.SetActive(false);
            this.closedModel.SetActive(true);
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

        if(this.destination != null) {
            this.destination.setOtherForceField(this.forceFieldOn);
        }
    }

    public void setOtherForceField(bool forceFieldStatus) {
        this.forceFieldOther.SetActive(forceFieldStatus);
    }

    public bool hasForceField() {
        return this.forceFieldOn;
    }
}