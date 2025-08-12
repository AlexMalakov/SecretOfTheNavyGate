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

    [SerializeField] private UnderbellyStaircase staircaseToOpen;
    private bool completed = false;

    public void Awake() {
        this.layout.addRoomUpdateListener(this);
    }

    public void onRoomUpdate(List<Room> rooms) {
        if(completed) {
            return;
        }
        bool TlrNeighbors = this.areNeighbors(this.topLeftPiece, this.topRightPiece) && this.correctOrientation(this.topLeftPiece, this.topRightPiece, false);
        this.topLeftPiece.setActivationStatus(false, TlrNeighbors);
        this.topRightPiece.setActivationStatus(false, TlrNeighbors);

        bool LtbNeighbors = this.areNeighbors(this.topLeftPiece, this.bottomLeftPiece) && this.correctOrientation(this.topLeftPiece, this.bottomLeftPiece, true);
        this.topLeftPiece.setActivationStatus(true, LtbNeighbors);
        this.bottomLeftPiece.setActivationStatus(true, LtbNeighbors);

        bool RtbNeighbors = this.areNeighbors(this.topRightPiece, this.bottomRightPiece) && this.correctOrientation(this.topRightPiece, this.bottomRightPiece, true);
        this.topRightPiece.setActivationStatus(true, RtbNeighbors);
        this.bottomRightPiece.setActivationStatus(true, RtbNeighbors);

        bool BlrNeighbors = this.areNeighbors(this.bottomLeftPiece, this.bottomRightPiece) && this.correctOrientation(this.bottomLeftPiece, this.bottomRightPiece, false);
        this.bottomLeftPiece.setActivationStatus(false, BlrNeighbors);
        this.bottomRightPiece.setActivationStatus(false, BlrNeighbors);

        if(this.topLeftPiece.isFullyActivated() 
                && this.topRightPiece.isFullyActivated()
                && this.bottomLeftPiece.isFullyActivated() 
                && this.bottomRightPiece.isFullyActivated()) {

            completed = true;
            staircaseToOpen.onEffect();
        }
    }

    private bool areNeighbors(PackmanCornerPiece p1, PackmanCornerPiece p2) {
        return (Mathf.Abs(p1.getRoomCoords().x - p2.getRoomCoords().x) == 1 && p1.getRoomCoords().y - p2.getRoomCoords().y == 0)
            || (p1.getRoomCoords().x - p2.getRoomCoords().x == 0 && Mathf.Abs(p1.getRoomCoords().y - p2.getRoomCoords().y) == 1)
            || (((p1.getRoomCoords().x == 0 && p2.getRoomCoords().x == RoomsLayout.ROOM_GRID_X-1) || (p1.getRoomCoords().x == RoomsLayout.ROOM_GRID_X-1 && p2.getRoomCoords().x == 0)) && p1.getRoomCoords().y - p2.getRoomCoords().y == 0)
            || (p1.getRoomCoords().x - p2.getRoomCoords().x == 0 && ((p1.getRoomCoords().y == 0 && p2.getRoomCoords().y == RoomsLayout.ROOM_GRID_X-1) || (p1.getRoomCoords().y == RoomsLayout.ROOM_GRID_X-1 && p2.getRoomCoords().y == 0)));
    }

    private bool correctOrientation(PackmanCornerPiece p1, PackmanCornerPiece p2, bool vertical) {
        return vertical ? (p1.getRoomCoords().y - p2.getRoomCoords().y == 1) : (p1.getRoomCoords().x - p2.getRoomCoords().x == -1);
    }
}
