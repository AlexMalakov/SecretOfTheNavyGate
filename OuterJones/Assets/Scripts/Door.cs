using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public enum DoorDirection {
    North, East, West, South
}

public class Door : MonoBehaviour 
{
    [SerializeField] private Door destination;
    [SerializeField] private Room room;
    [SerializeField] private DoorDirection direction;
    [SerializeField] private Transform enterPosition;
    
    public void setDestination(Door newDestination) {
        this.destination = newDestination;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(/*Input.GetKeyDown(KeyCode.Space) && */other.gameObject.GetComponent<Player>() != null) {
            this.useDoor(other.gameObject.GetComponent<Player>());
        }
    }

    private void useDoor(Player player) {
        if(this.destination == null) {
            Room next = player.getNextInDeck();
            if(room.getLayoutManager().canPlaceRoom(this, next)) {
                player.removeNextInDeck();
                room.getLayoutManager().placeRoom(this, next);
                this.setDestination(next.getEntrance(this.getInverse()));
                this.destination.setDestination(this);
                this.onExit();
            } else {
                return;
            }
        }

        this.onExit();
        this.destination.onEnter(player);
    }

    private void onExit() {
        this.room.onExit();
    }

    public void onEnter(Player player) {
        player.transform.position = enterPosition.position;
        this.room.onEnter();
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

    public RoomCoords getPosition() {
        return this.room.getPosition();
    }
}