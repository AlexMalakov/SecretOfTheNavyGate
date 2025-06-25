using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileSwapPair {
    public TileBase originalTile;
    public TileBase floodedTile;
}

//canal endings can either be dams or wall
public class Canal : MonoBehaviour
{
    //list of int 0 - 8
    [SerializeField] List<CanalEntrances> canalEntrances;
    [SerializeField] List<Dam> attatchedDams;
    private Room room;

    [SerializeField] private Tilemap canalTilemap; // Assign in inspector
    [SerializeField] private List<TileSwapPair> tileSwaps; // Assign original -> flooded, flooded -> original
    private Dictionary<TileBase, TileBase> swapDict;

    [SerializeField] private Collider2D canalCollider; //trigger when empty, collider when flooded
    [SerializeField] private GameObject edgeCollider; //collider attatched to external object

    private bool flooded = false;

    public void Awake() {
        this.room = GetComponentInParent<Room>();

        swapDict = new Dictionary<TileBase, TileBase>();
        foreach (var pair in tileSwaps) {
            if (pair.originalTile != null && pair.floodedTile != null) {
                swapDict[pair.originalTile] = pair.floodedTile;
                swapDict[pair.floodedTile] = pair.originalTile;
            }
        }
    }
    
    public void onFlood(CanalEntrances? floodingFrom) {
        if(this.flooded) {
            return;
        }

        this.swapTiles();
        this.flooded = true;
        this.canalCollider.isTrigger = false;

        List<CanalEntrances> floodTo = new List<CanalEntrances>(this.canalEntrances);

        if(floodingFrom != null) {
            floodTo.Remove((CanalEntrances)floodingFrom);
        }

        foreach(Dam d in this.attatchedDams) {
            d.onFlood(this, floodingFrom);
        }

        this.room.floodNeighbors(floodTo);
    }

    public bool willFlood(CanalEntrances floodingFrom) {
        return !flooded && this.canalEntrances.Contains(floodingFrom);
    }


    public void drainWater() {
        if(this.flooded) {
            swapTiles();
        }

        this.canalCollider.isTrigger = true;
        this.flooded = false;
    }

    public void swapTiles() {
        BoundsInt bounds = canalTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++) {
            for (int y = bounds.yMin; y < bounds.yMax; y++) {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase currentTile = canalTilemap.GetTile(pos);

                if (currentTile != null && swapDict.TryGetValue(currentTile, out TileBase floodedTile)) {
                    canalTilemap.SetTile(pos, floodedTile);
                }
            }
        }
    }

    //when the player falls in
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.edgeCollider.SetActive(true);
        }
    }

    //once the player takes the ladder, turn off the edge collider
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.edgeCollider.SetActive(false);
        }
    }

    public void rotate90(bool clockwise) {
        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((WaterRoom.CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % WaterRoom.CANAL_ENTRANCE_COUNT);
        }
    }
}

