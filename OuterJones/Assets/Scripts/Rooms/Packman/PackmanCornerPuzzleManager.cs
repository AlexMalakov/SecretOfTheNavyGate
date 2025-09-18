using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CornerPosition {
    TL = 0, TR = 1, BR = 2, BL = 3
}

public class PackmanCornerPuzzleManager : MonoBehaviour, RoomUpdateListener
{
    [SerializeField] private List<PackmanCornerPiece> cornerPieces;
    [SerializeField] private RoomsLayout layout;

    [SerializeField] private GameObject effectableObj;
    private bool completed = false;

    public void Awake() {
        this.layout.addRoomUpdateListener(this);
    }

    public void onRoomUpdate(List<Room> rooms) {
        //check that all corners are in the correct spot
        if(completed) {
            return;
        }

        Dictionary<CornerPosition, List<PackmanCornerPiece>> keyPos = new Dictionary<CornerPosition, List<PackmanCornerPiece>>();
        keyPos.Add(CornerPosition.TL, new List<PackmanCornerPiece>());
        keyPos.Add(CornerPosition.TR, new List<PackmanCornerPiece>());
        keyPos.Add(CornerPosition.BL, new List<PackmanCornerPiece>());
        keyPos.Add(CornerPosition.BR, new List<PackmanCornerPiece>());

        foreach(PackmanCornerPiece piece in cornerPieces) {
            keyPos[piece.getCornerPosition()].Add(piece);
        }

        this.checkNeighbors(keyPos[CornerPosition.TL], keyPos[CornerPosition.TR], keyPos[CornerPosition.BL]);
        this.checkNeighbors(keyPos[CornerPosition.TR], keyPos[CornerPosition.BR], keyPos[CornerPosition.TL]);
        this.checkNeighbors(keyPos[CornerPosition.BL], keyPos[CornerPosition.TL], keyPos[CornerPosition.BR]);
        this.checkNeighbors(keyPos[CornerPosition.BR], keyPos[CornerPosition.BL], keyPos[CornerPosition.TR]);


        foreach(PackmanCornerPiece piece in cornerPieces) {
            if(!piece.isFullyActivated()) {
                return;
            }
        }
        completed = true;
        effectableObj.gameObject.GetComponent<Effectable>().onEffect();
    }

    private void checkNeighbors(List<PackmanCornerPiece> pieces, List<PackmanCornerPiece> cwNeighbors, List<PackmanCornerPiece> ccwNeighbors) {
        foreach(PackmanCornerPiece piece in pieces) {
            piece.deactivate();

            foreach(PackmanCornerPiece cwNeighbor in cwNeighbors) {
                if(areNeighbors(piece, cwNeighbor, isHorizontalOffsetDirection(piece.getCornerPosition(), true))) {
                    piece.activate(true);
                    break;
                }
            }

            foreach(PackmanCornerPiece ccwNeighbor in ccwNeighbors) {
                if(areNeighbors(piece, ccwNeighbor, isHorizontalOffsetDirection(piece.getCornerPosition(), false))) {
                    piece.activate(false);
                    break;
                }
            }
        }
    }

    private bool isHorizontalOffsetDirection(CornerPosition position, bool clockwise) {
        int intPos = (int)position;
        return (intPos % 2 == 0 && clockwise) || (intPos % 2 == 1 && !clockwise);
    }

    private bool areNeighbors(PackmanCornerPiece p1, PackmanCornerPiece p2, bool isHorizontal) {
        RoomCoords p1RC = p1.getRoomCoords();
        RoomCoords p2RC = p2.getRoomCoords();

        if(isHorizontal) {
            return p1RC.y - p2RC.y == 0 && (Mathf.Abs(p1RC.x - p2RC.x) == 1 || (p1RC.x == 0 && p2RC.x == RoomsLayout.ROOM_GRID_X-1) || (p2RC.x == 0 && p1RC.x == RoomsLayout.ROOM_GRID_X-1));
        }
        return p1RC.x - p2RC.x == 0 && (Mathf.Abs(p1RC.y - p2RC.y) == 1 || (p1RC.y == 0 && p2RC.y == RoomsLayout.ROOM_GRID_X-1) || (p2RC.y == 0 && p1RC.y == RoomsLayout.ROOM_GRID_X-1));
    }

    private bool correctOrientation(PackmanCornerPiece p1, PackmanCornerPiece p2, bool vertical) {
        return vertical ? (p1.getRoomCoords().y - p2.getRoomCoords().y == 1) : (p1.getRoomCoords().x - p2.getRoomCoords().x == -1);
    }

    public static CornerPosition rotateCornerPosition(bool clockwise, CornerPosition pos) {
        return (CornerPosition)(((int)pos + 4 + (clockwise ? 1 : -1))%4);
    }
}
