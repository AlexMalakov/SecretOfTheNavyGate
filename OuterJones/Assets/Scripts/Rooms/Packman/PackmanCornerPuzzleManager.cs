using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanCornerPuzzleManager : MonoBehaviour, RoomUpdateListener
{
    [SerializeField] private PackmanCornerPiece topLeftPiece;
    [SerializeField] private PackmanCornerPiece topRightPiece;
    [SerializeField] private PackmanCornerPiece bottomLeftPiece;
    [SerializeField] private PackmanCornerPiece bottomRightPiece;

    [SerializeField] private RoomsLayout layout;

    public void Awake() {
        this.layout.addRoomUpdateListener(this);
    }



    public void onRoomUpdate(List<Room> rooms) {
        bool TlrNeighbors = this.areNeighbors(this.topLeftPiece, this.topRightPiece);
        this.topLeftPiece.setActivationStatus(false, TlrNeighbors);
        this.topRightPiece.setActivationStatus(false, TlrNeighbors);

        bool LtbNeighbors = this.areNeighbors(this.topLeftPiece, this.bottomLeftPiece);
        this.topLeftPiece.setActivationStatus(true, LtbNeighbors);
        this.bottomLeftPiece.setActivationStatus(true, LtbNeighbors);

        bool RtbNeighbors = this.areNeighbors(this.topRightPiece, this.bottomRightPiece);
        this.topRightPiece.setActivationStatus(true, RtbNeighbors);
        this.bottomRightPiece.setActivationStatus(true, RtbNeighbors);

        bool BlrNeighbors = this.areNeighbors(this.bottomLeftPiece, this.bottomRightPiece);
        this.bottomLeftPiece.setActivationStatus(false, BlrNeighbors);
        this.bottomRightPiece.setActivationStatus(false, BlrNeighbors);

        if(this.topLeftPiece.isFullyActivated() 
                && this.topRightPiece.isFullyActivated()
                && this.bottomLeftPiece.isFullyActivated() 
                && this.bottomRightPiece.isFullyActivated()) {

            //OPEN ENTRANCE TO UNDERBELLY!!!!!
        }
    }

    private bool areNeighbors(PackmanCornerPiece p1, PackmanCornerPiece p2) {
        return (Mathf.Abs(p1.getRoomCoords().x - p2.getRoomCoords().x) == 1 && p1.getRoomCoords().y - p2.getRoomCoords().y == 0)
            || (p1.getRoomCoords().x - p2.getRoomCoords().x == 0 && Mathf.Abs(p1.getRoomCoords().y - p2.getRoomCoords().y) == 1);
    }
}
