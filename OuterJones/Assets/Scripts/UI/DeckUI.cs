using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckUI : MonoBehaviour, RoomUpdateListener, DoorUseListener
{

    private Player player;
    [SerializeField] private Sprite cannotPlace;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text; //temporary

    public void init(Player p) {
        this.player = p;

        FindObjectOfType<RoomsLayout>().addRoomUpdateListener(this);

        this.gameObject.SetActive(true);

        onUpdate();
        this.image.type = Image.Type.Simple;
        this.image.preserveAspect = false;
    }


    public void onRoomUpdate(List<Room> rooms) {
        this.onUpdate();
    }

    public void onRoomEnter() {
        this.onUpdate();
    }

    public void onUpdate() {
        if(!canPlaceNextRoom()) {
            this.image.sprite = cannotPlace;
        } else {
            this.image.sprite = this.player.getNextInDeck(true).getRoomSprite();
        }

        text.text = this.player.getDeckSize() + " remaining in deck";
    }

    private bool canPlaceNextRoom() {
        if(this.player.getDeckSize() == 0) {
            return false;
        }

        foreach(Door d in this.player.getNextInDeck(true).getDoors()) {
            if(this.player.getCurrentRoom().hasDoorDirection(d.getInverse())) {
                return true;
            }
        }

        return false;
    }

}
