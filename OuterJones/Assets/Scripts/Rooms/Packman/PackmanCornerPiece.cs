using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackmanCornerPiece : MonoBehaviour
{
    [SerializeField] private PackmanRoom roomOfOrigin;
    [SerializeField] private CornerPosition currentPos;

    [SerializeField] private GameObject cwLit;
    [SerializeField] private GameObject ccwLit;

    private bool cwActivated = false;
    private bool ccwActivated = false;

    public RoomCoords getRoomCoords() {
        return this.roomOfOrigin.getPosition();
    }

    public void deactivate() {
        this.cwActivated = false;
        this.ccwActivated = false;
        this.cwLit.SetActive(false);
        this.ccwLit.SetActive(false);
    }

    public void activate(bool cwNeighbor) {
        this.cwActivated = cwActivated || cwNeighbor;
        this.ccwActivated = ccwActivated || !cwNeighbor;
        this.cwLit.SetActive(this.cwActivated);
        this.ccwLit.SetActive(this.ccwActivated);
    }

    public CornerPosition getCornerPosition() {
        return this.currentPos;
    }


    public bool isFullyActivated() {
        return cwActivated && ccwActivated;
    }

    public void rotate90(bool clockwise) {
        this.currentPos = PackmanCornerPuzzleManager.rotateCornerPosition(clockwise, this.currentPos);
    }
}
