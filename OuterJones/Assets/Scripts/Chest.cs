using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private List<Room> deck;
    [SerializeField] private Item item;

    //this is a warcrime im so so so sorry
    [Header ("direction")]
    [SerializeField] private GameObject openNorth;
    [SerializeField] private GameObject openEast;
    [SerializeField] private GameObject openSouth;
    [SerializeField] private GameObject openWest;
    [SerializeField] private GameObject closedNorth;
    [SerializeField] private GameObject closedEast;
    [SerializeField] private GameObject closedSouth;
    [SerializeField] private GameObject closedWest;

    [SerializeField] private DoorDirection chestFacing;
    
    private Quaternion initialRot;
    private bool opened = false;

    public IEnumerator Start() {
        this.initialRot = transform.rotation;
        this.updateSprite();
        yield return null;
        yield return null; //skips 2 frames before hiding every room
        foreach(Room r in this.deck) { //IM SETTING EVERY ROOM TO NOT ACTIVE!
            r.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(opened) {
            return;
        }

        if(this.deck != null && other.gameObject.GetComponent<Player>() != null) {
            other.gameObject.GetComponent<Player>().addToDeck(this.deck);
            FindObjectOfType<DeckUI>().onUpdate();
        } else if(this.item != null && other.gameObject.GetComponent<Player>() != null) {
            opened = true;
            other.GetComponent<Player>().getInventory().gainItem(this.item);
        } else {
            //failed to give the player anything
            return;
        }

        opened = true;
        this.updateSprite();
    }

    public void rotate90(bool clockwise) {
        this.chestFacing = Door.rotateDoorDirection(this.chestFacing, clockwise);
        this.transform.rotation = this.initialRot;
        this.updateSprite();
    }

    private void updateSprite() {
        this.openNorth.SetActive(this.opened && this.chestFacing == DoorDirection.North);
        this.openEast.SetActive(this.opened && this.chestFacing == DoorDirection.East);
        this.openSouth.SetActive(this.opened && this.chestFacing == DoorDirection.South);
        this.openWest.SetActive(this.opened && this.chestFacing == DoorDirection.West);
        this.closedNorth.SetActive(!this.opened && this.chestFacing == DoorDirection.North);
        this.closedEast.SetActive(!this.opened && this.chestFacing == DoorDirection.East);
        this.closedSouth.SetActive(!this.opened && this.chestFacing == DoorDirection.South);
        this.closedWest.SetActive(!this.opened && this.chestFacing == DoorDirection.West);
    }
}
