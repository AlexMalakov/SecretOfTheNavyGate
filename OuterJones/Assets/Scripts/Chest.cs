using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, InputSubscriber
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

    [SerializeField] private GameObject popUp;

    [SerializeField] private bool hideLast = false;

    private PlayerIO playerIO;
    
    private Quaternion initialRot;
    private bool opened = false;

    public IEnumerator Start() {
        this.initialRot = transform.rotation;
        this.updateSprite();
        this.playerIO = FindObjectOfType<PlayerIO>();
        yield return null;
        yield return null; //skips 2 frames before hiding every room

        if(hideLast) {
            yield return null;
        }

        foreach(Room r in this.deck) { //IM SETTING EVERY ROOM TO NOT ACTIVE!
            r.hideRoom();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(opened) {
            return;
        }

        if(this.deck.Count > 0 && other.gameObject.GetComponent<Player>() != null) {
            other.gameObject.GetComponent<Player>().addToDeck(this.deck);
            FindObjectOfType<DeckUI>().onUpdate();
            this.playerIO.requestPopUpMessage(this, this.transform, ("Added " + this.deck.Count + " rooms into your deck!"));
        } else if(this.item != null && other.gameObject.GetComponent<Player>() != null) {
            other.GetComponent<Player>().getInventory().gainItem(this.item);
            this.playerIO.requestPopUpMessage(this, this.transform, "You've found a " + this.item.getName());
        } else if(this.popUp != null && other.gameObject.GetComponent<Player>() != null){
            this.playerIO.displayEndGamePopUp(this, this.transform);
        } else {
            //failed to give the player anything
            return;
        }

        opened = true;
        this.updateSprite();
    }

    void OnTriggerExit2D(Collider2D other) {
        if(this.popUp != null) {
            this.playerIO.cancelRequest();
        }
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

    public void onSpacePress(){}
    
}
