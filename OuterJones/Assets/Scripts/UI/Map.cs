using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map: MonoBehaviour, RoomUpdateListener
{
    private MapImageWrangler wrangler;
    [SerializeField] Image exclamationImg;
    private Coroutine exclamationBlink;

    public void Start() {
        this.wrangler = FindObjectOfType<MapImageWrangler>();

        FindObjectOfType<RoomsLayout>().addRoomUpdateListener(this);

        Room r = GameObject.Find("startingRoom").GetComponent<Room>();
        Image i = this.wrangler.getImageAt(2, 2);
        displayRoom(GameObject.Find("startingRoom").GetComponent<Room>(), this.wrangler.getImageAt(2, 2));

    }


    //TODO: Moved rooms that are null
    public void onRoomUpdate(List<Room> rooms) {
        foreach(Room r in rooms) {
            displayRoom(r, this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y));
        }

    }

    private void displayRoom(Room r, Image i) {
        i.sprite = r.getRoomSprite();
        i.type = Image.Type.Simple;
        i.preserveAspect = false;
        i.gameObject.SetActive(true);

        i.transform.rotation = r.transform.rotation;
    }

    public void onUnderbellyUnlock(Room r) {
        this.exclamationImg.GetComponent<RectTransform>().anchoredPosition = this.wrangler.getImageAt(r.getPosition().x, r.getPosition().y).GetComponent<RectTransform>().anchoredPosition;
        if(this.exclamationBlink != null) {
            StopCoroutine(this.exclamationBlink);
        }
        this.exclamationBlink = StartCoroutine(flashExclamation());
    }

    private IEnumerator flashExclamation() {
        int num_flashes = 5;
        for(int i = 0; i < num_flashes; i++) {
            float elapsed = 0f;
            float duration = .5f;

            this.exclamationImg.gameObject.SetActive(true);
            while(elapsed < duration) {
                elapsed += Time.deltaTime;
                yield return null;
            }

            this.exclamationImg.gameObject.SetActive(false);

            while(elapsed < 2*duration) {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void wipeSquares(List<RoomCoords> wipeTargets) {
        foreach(RoomCoords rc in wipeTargets) {
            this.wrangler.getImageAt(rc.x, rc.y).gameObject.SetActive(false);
        }
    }
}


