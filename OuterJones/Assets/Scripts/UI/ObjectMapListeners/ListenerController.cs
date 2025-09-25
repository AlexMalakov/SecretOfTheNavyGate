using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListenerController : MonoBehaviour
{
    //attatched to a room
    
    [SerializeField] private Room originRoom;
    [SerializeField] private List<ObjListener> listeners;
    [SerializeField] private Map map;


    public void onRoomMove() {
        this.GetComponent<RectTransform>().gameObject.SetActive(true);
        this.GetComponent<RectTransform>().position = this.map.getTransformForRoom(this.originRoom).position;
    }

    public void rotate90(bool clockwise) {
        this.GetComponent<RectTransform>().transform.Rotate(0f, 0f, (clockwise ? -90f : 90f));
    }

    public List<ObjListener> getListeners() {
        return this.listeners;
    }

    public void requestRoomExclamation() {
        this.map.onSignificantRoomEvent(this.originRoom);
    }
}
