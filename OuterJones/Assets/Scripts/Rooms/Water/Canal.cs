using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//canal endings can either be dams or wall
public class Canal : MonoBehaviour
{
    //list of int 0 - 8
    [SerializeField] List<CanalEntrances> canalEntrances;
    [SerializeField] List<Dam> attatchedDams;

    [SerializeField] List<Floodable> floodableObjects;
    private Room room;

    [SerializeField] private Tilemap canalTilemap; // Assign in inspector

    [SerializeField] private Collider2D canalCollider; //trigger when empty, collider when flooded
    [SerializeField] private GameObject edgeCollider; //collider attatched to external object

    private bool flooded = false;

    public void Awake() {
        this.room = GetComponentInParent<Room>();
        this.edgeCollider.SetActive(false);
        this.canalCollider.isTrigger = true;

        foreach(Floodable f in this.floodableObjects) {
            if(f is Ladder) {
                ((Ladder)f).init(this);
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

        foreach(Floodable f in this.floodableObjects) {
            f.onFlood();
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

        foreach(Floodable f in this.floodableObjects) {
            f.drainWater();
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

                if (currentTile != null && WaterSource.swapDict.TryGetValue(currentTile, out TileBase floodedTile)) {
                    canalTilemap.SetTile(pos, floodedTile);
                }
            }
        }
    }

    // //when the player falls in
    // void OnTriggerEnter2D(Collider2D other) {
    //     if(other.gameObject.GetComponent<Player>() != null) {
    //         this.edgeCollider.SetActive(true);
    //     }
    // }

    // //once the player takes the ladder, turn off the edge collider
    // void OnTriggerExit2D(Collider2D other) {
    //     if(other.gameObject.GetComponent<Player>() != null) {
    //         this.edgeCollider.SetActive(false);
    //     }
    // }

    void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            bool allInside = true;
            foreach(PlayerEdgeCollider e in other.gameObject.GetComponent<Player>().getEdgeColliders()) {
                allInside = allInside && e.isCollidingWithCanal();
            }

            if(allInside) {
                this.edgeCollider.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {

        if(other.gameObject.GetComponent<PlayerEdgeCollider>() != null) {
            other.gameObject.GetComponent<PlayerEdgeCollider>().setCanalStatus(false);
        }

        if(other.gameObject.GetComponent<Player>() != null) {
            this.edgeCollider.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerEdgeCollider>() != null) {
            other.gameObject.GetComponent<PlayerEdgeCollider>().setCanalStatus(true);
        }
    }

    public void onLadderUse() {
        this.edgeCollider.SetActive(false);
    }

    public void rotate90(bool clockwise) {
        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((WaterSource.CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % WaterSource.CANAL_ENTRANCE_COUNT);
        }
    }
}

