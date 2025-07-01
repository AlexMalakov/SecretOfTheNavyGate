using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileSwapPair {
    public TileBase originalTile;
    public TileBase floodedTile;
}

public enum CanalEntrances {
    NW = 0, N = 1, NE = 2, E = 3, SE = 4, S = 5, SW = 6, W = 7
}

public abstract class Floodable : MonoBehaviour {
    public abstract void onFlood();
    public abstract void drainWater();
}

public class WaterSource : MonoBehaviour, RoomUpdateListener
{
    [SerializeField] RoomsLayout layout;
    [SerializeField] Canal waterOrigin;
    
    //this goes here since i only want to define this once
    [SerializeField] private List<TileSwapPair> tileSwaps; // Assign original -> flooded, flooded -> original
    public static Dictionary<TileBase, TileBase> swapDict;

    public static int CANAL_ENTRANCE_COUNT = 8;

    public static Dictionary<CanalEntrances, int[]> CANAL_N_MAP = new Dictionary<CanalEntrances, int[]>() {
        {CanalEntrances.NW, new int[]{-1, 1}},
        {CanalEntrances.N, new int[]{0, 1}},
        {CanalEntrances.NE, new int[]{1, 1}},
        {CanalEntrances.E, new int[]{1, 0}},
        {CanalEntrances.SE, new int[]{1, -1}},
        {CanalEntrances.S, new int[]{0, -1}},
        {CanalEntrances.SW, new int[]{-1, -1}},
        {CanalEntrances.W, new int[]{-1, 0}},
    };

    public void Awake() {
        swapDict = new Dictionary<TileBase, TileBase>();
        foreach (var pair in tileSwaps) {
            if (pair.originalTile != null && pair.floodedTile != null) {
                swapDict[pair.originalTile] = pair.floodedTile;
                swapDict[pair.floodedTile] = pair.originalTile;
            }
        }
    }

    public void Start() {
        layout.addRoomUpdateListener(this);
    }

    public void onWaterUpdate() {
        this.drainAll();
        this.computeFlow();
    }

    private void drainAll() {
        foreach(Room r in this.layout.getAllRooms()) {
            r.drainWater();
        }
    }

    private void computeFlow() {
        waterOrigin.onFlood(null);
    }

    public void onRoomUpdate(List<Room> rooms) {
        this.onWaterUpdate();
    }
}
