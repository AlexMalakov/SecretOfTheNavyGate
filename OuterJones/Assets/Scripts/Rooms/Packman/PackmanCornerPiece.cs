using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanCornerPiece : MonoBehaviour, Effectable
{
    [SerializeField] private PackmanRoom roomOfOrigin;

    bool powered;
    bool vertActivated;
    bool horizActivated;

    public RoomCoords getRoomCoords() {
        return this.roomOfOrigin.getPosition();
    }

    public void setActivationStatus(bool verticalNeighbor, bool status) {
        if(verticalNeighbor) {
            vertActivated = status;
            //light up
        } else {
            horizActivated = status;
            //light up
        }
    }

    public void onEffect() {
        this.powered = true;
        //light up
    }

    public bool isFullyActivated() {
        return powered && vertActivated && horizActivated;
    }
}
