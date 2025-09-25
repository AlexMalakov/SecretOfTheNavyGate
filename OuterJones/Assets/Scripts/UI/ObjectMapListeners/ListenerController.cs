using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListenerController : MonoBehaviour
{
    //attatched to a room
    
    [SerializeField] private Room originRoom;
    [SerializeField] private List<ObjListener> listeners;
    [SerializeField] private Image img;
    [SerializeField] private Map map;



    public void onRoomMove() {
        this.img.gameObject.SetActive(true);
        this.img.GetComponent<RectTransform>().anchoredPosition = this.map.getTransformForRoom(this.originRoom).anchoredPosition;
    }

    public void rotate90(bool clockwise) {
        this.img.transform.Rotate(0f, 0f, (clockwise ? -90f : 90f));
    }

    public List<ObjListener> getListeners() {
        return this.listeners;
    }

    public void requestRoomExclamation() {
        this.map.onSignificantRoomEvent(this.originRoom);
    }
}
