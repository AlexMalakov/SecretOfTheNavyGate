using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanCornerPiece : MonoBehaviour
{
    [SerializeField] private PackmanRoom roomOfOrigin;

    [SerializeField] private GameObject vertLit;
    [SerializeField] private GameObject horizLit;

    private bool vertActivated = false;
    private bool horizActivated = false;

    public RoomCoords getRoomCoords() {
        return this.roomOfOrigin.getPosition();
    }

    public void setActivationStatus(bool verticalNeighbor, bool status) {
        if(verticalNeighbor) {
            vertActivated = status;
            vertLit.SetActive(status);
            //light up
        } else {
            horizActivated = status;
            horizLit.SetActive(status);
            //light up
        }
    }


    public bool isFullyActivated() {
        return vertActivated && horizActivated;
    }
}
