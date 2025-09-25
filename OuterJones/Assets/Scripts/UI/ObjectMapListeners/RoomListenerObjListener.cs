using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListenerObjListener : ObjListener, RoomUpdateListener
{

    [SerializeField] private RoomsLayout layout;
    private bool status;
    private bool statusBank;
    private bool updateInitiated = false;

    void Start()
    {
        layout.addPreRoomUpdateListener(this);
        layout.addPostRoomUpdateListener(this);
    }

    public void onRoomUpdate(List<Room> r) {
        this.updateInitiated = !this.updateInitiated;

        if(this.updateInitiated) {
            this.statusBank = status;
        } else if(this.statusBank != this.status) {
            this.onStatusChanged(this.status);
        }
    }

    public void onRoomEvent(bool outcome) {
        this.status = outcome;
    }
}
