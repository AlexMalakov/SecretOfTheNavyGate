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
    //list of int 0 - 15
    [SerializeField] List<CanalEntrances> canalEntrances;
    [SerializeField] List<Dam> attatchedDams;
    private Room room;

    [SerializeField] private Tilemap canalTilemap; // Assign in inspector
    [SerializeField] private List<TileSwapPair> tileSwaps; // Assign original -> flooded
    private Dictionary<TileBase, TileBase> swapDict;

    [SerializeField] private Collider2D canalTrigger;
    [SerializeField] private Collider2D canalCollider;
    [SerializeField] private GameObject edgeCollider;

    private bool flooded = false;

    public void Awake() {
        this.room = GetComponentInParent<Room>();

        swapDict = new Dictionary<TileBase, TileBase>();
        foreach (var pair in tileSwaps) {
            if (pair.originalTile != null && pair.floodedTile != null) {
                swapDict[pair.originalTile] = pair.floodedTile;
            }
        }
    }
    
    public void onFlood(List<CanalEntrances> floodingFrom) {
        if(this.flooded) {
            return;
        }

        this.swapTiles();
        this.flooded = true;
        this.canalCollider.enabled = true;
        this.canalTrigger.enabled = false;

        List<CanalEntrances> floodTo = new List<CanalEntrances>(this.canalEntrances);

        foreach(CanalEntrances c in floodingFrom) {
            if(floodTo.Contains(c)) {
                floodTo.Remove(c);
            }
        }

        foreach(Dam d in this.attatchedDams) {
            d.onFlood(this, floodingFrom);
        }

        this.room.floodNeighbors(floodTo);
    }

    public bool willFlood(List<CanalEntrances> floodingFrom) {
        if(flooded) {
            return false;
        }

        foreach(CanalEntrances c in this.canalEntrances) {
            if(floodingFrom.Contains(c)) {
                return true;
            }
        }
        return false;
    }


    public void drainWater() {
        if(this.flooded) {
            swapTiles();
        }

        this.canalTrigger.enabled = true;
        this.canalCollider.enabled = false;
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

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.edgeCollider.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.edgeCollider.SetActive(false);
        }
    }
}

