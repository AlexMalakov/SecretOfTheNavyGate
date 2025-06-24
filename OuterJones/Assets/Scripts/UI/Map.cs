using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map: MonoBehaviour, RoomUpdateListener
{
    private MapImageWrangler wrangler;


    public void Start() {
        this.wrangler = FindObjectOfType<MapImageWrangler>();

        FindObjectOfType<RoomsLayout>().addRoomUpdateListener(this);

        Room r = GameObject.Find("startingRoom").GetComponent<Room>();
        Image i = this.wrangler.getImageAt(2, 2);
        displayRoom(GameObject.Find("startingRoom").GetComponent<Room>(), this.wrangler.getImageAt(2, 2));

    }

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
}


