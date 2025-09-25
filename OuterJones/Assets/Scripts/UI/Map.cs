using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Map: MonoBehaviour, RoomUpdateListener
{
    private MapImageWrangler wrangler;
    [SerializeField] MapExclamationManager exclManager;
    [SerializeField] Image currentRoomOutline;
    [SerializeField] TMP_Text curRoom;

    [Header ("default sprites")]
    [SerializeField] private Sprite lightChecker;
    [SerializeField] private Sprite darkChecker;

    private RoomsLayout layout;

    public void Start() {
        this.wrangler = FindObjectOfType<MapImageWrangler>();

        for(int i = 0; i < 5; i++) {
            for(int j = 0; j < 5; j++) {
                Image img = this.wrangler.getImageAt(i, j);
                img.gameObject.SetActive(true);

                this.wipeImage(img, 5*i+j);
            }
        }

        this.layout = FindObjectOfType<RoomsLayout>();
        this.layout.addRoomUpdateListener(this);

        Room r = GameObject.Find("startingRoom").GetComponent<Room>();
        displayRoom(GameObject.Find("startingRoom").GetComponent<Room>(), this.wrangler.getImageAt(2, 2));
    }


    //TODO: Moved rooms that are null
    public void onRoomUpdate(List<Room> rooms) {
        if(rooms == null) {
            return;
        }

        foreach(Room r in rooms) {
            if(r.getPosition().overworld) {
                displayRoom(r, this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y));
            } else {
                displayRoom(layout.getRoomAt(r.getPosition().swapFloor()), this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y));
            }
            
        }
    }

    public void onNewRoomEntered(Room r) {
        this.curRoom.text = "Current: " + r.getRoomName();
    }

    private void displayRoom(Room r, Image i) {
        i.sprite = r.getRoomSprite();
        i.type = Image.Type.Simple;
        i.preserveAspect = false;

        i.transform.rotation = r.transform.rotation;
    }

    public void onUnderbellyUnlock(Room r) {
        this.exclManager.startFlash(r, this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y).GetComponent<RectTransform>(), false);
    }

    public void onSignificantRoomEvent(Room r) {
        this.exclManager.startFlash(r, this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y).GetComponent<RectTransform>(), true);
    }

    public void wipeSquares(List<RoomCoords> wipeTargets) {
        foreach(RoomCoords rc in wipeTargets) {
            wipeImage(this.wrangler.getImageAt(rc.x, rc.y), rc.x * 5 + rc.y);
        }
    }

    public RectTransform getTransformForRoom(Room r) {
        return this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y).GetComponent<RectTransform>();
    }

    private void wipeImage(Image img, int parity) {
        img.sprite = (parity % 2 == 1 ? lightChecker : darkChecker);
        img.type = Image.Type.Simple;
        img.preserveAspect = false;
    }

    public void onPlayerEntersRoom(Room r) {
        this.exclManager.onRoomEntered(r);
        this.currentRoomOutline.GetComponent<RectTransform>().anchoredPosition = this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y).GetComponent<RectTransform>().anchoredPosition;
    }
}


