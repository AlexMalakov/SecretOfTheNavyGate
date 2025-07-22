using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//canal endings can either be dams or wall
public class Canal : MonoBehaviour
{

    //TODO: 

    //list of int 0 - 8
    [SerializeField] List<CanalEntrances> canalEntrances;
    [SerializeField] List<Dam> attatchedDams;

    [SerializeField] List<Floodable> floodableObjects;
    [SerializeField] List<Grate> grates;
    private Room room;

    [SerializeField] private Tilemap canalTilemap; // Assign in inspector

    [SerializeField] private CompositeCollider2D canalCollider; //trigger when empty, collider when flooded
    [SerializeField] private GameObject waterCollider;
    [SerializeField] private GameObject edgeCollider;
    [SerializeField] private List<Transform> backupTransforms;

    [SerializeField] private bool flooded = false;

    private bool reachedThisFlood = false;
    private bool reachedThisDrain = false;

    private Renderer rend;
    private List<CanalEntrances> initialCanalEntrances;

    public void Awake() { //awake or start?
        this.room = GetComponentInParent<Room>();
        this.edgeCollider.SetActive(false);

        this.canalCollider.enabled = !this.flooded;
        this.waterCollider.SetActive(this.flooded);

        this.rend = GetComponent<Renderer>();

        this.initialCanalEntrances = new List<CanalEntrances>(this.canalEntrances);
    }

    private void copyMap(Tilemap source, Tilemap destination) {
        destination.ClearAllTiles();

        // Get the bounds of all tiles in source
        BoundsInt bounds = source.cellBounds;

        // Loop through each position in the source bounds
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = source.GetTile(pos);

                if (tile != null)
                {
                    destination.SetTile(pos, tile);

                    // Optional: copy tile flags and color too
                    destination.SetTileFlags(pos, source.GetTileFlags(pos));
                    destination.SetColor(pos, source.GetColor(pos));
                }
            }
        }
    }
    
    public void onFlood(CanalEntrances? floodingFrom) {
        if(this.reachedThisFlood || !this.gameObject.activeInHierarchy) {
            return;
        }

        this.reachedThisFlood = true;

        if(!this.flooded) {
            this.flooded = true;

            this.canalCollider.enabled = false;
            this.waterCollider.SetActive(true);
        }

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
        return !this.reachedThisFlood && this.canalEntrances.Contains(floodingFrom);
    }

    //dont need to drain dams: this is because instead of draining sequentially all objects are
    //drained and then flow is recalcualted
    public void drainWater(CanalEntrances? drainingFrom) {
        if(this.reachedThisDrain) {
            return;
        }

        this.reachedThisDrain = true;

        if(this.flooded) {
            this.canalCollider.enabled = true;
            this.waterCollider.SetActive(false);
            this.flooded = false;
        }

        List<CanalEntrances> drainTo = new List<CanalEntrances>(this.canalEntrances);

        if(drainingFrom != null) {
            drainTo.Remove((CanalEntrances)drainingFrom);
        }

        foreach(Floodable f in this.floodableObjects) {
            f.drainWater();
        }

        foreach(Dam d in this.attatchedDams) {
            d.drainWater(this, drainingFrom);
        }

        this.room.drainNeighbors(drainTo);
    }

    public bool willDrain(CanalEntrances drainingFrom) {
        return !this.reachedThisDrain && this.canalEntrances.Contains(drainingFrom);
    }
    void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            bool allInside = true;
            foreach(PlayerEdgeCollider e in other.gameObject.GetComponent<Player>().getEdgeColliders()) {
                allInside = allInside && e.isCollidingWithCanal();
            }

            if(allInside) {
                this.onPlayerInCanal();
            }
        }
    }

    public void restartFlood() {
        this.reachedThisFlood = false;
        this.reachedThisDrain = false;
    }

    void OnTriggerExit2D(Collider2D other) {

        if(other.gameObject.GetComponent<PlayerEdgeCollider>() != null) {
            other.gameObject.GetComponent<PlayerEdgeCollider>().setCanalStatus(false);
        }

        if(other.gameObject.GetComponent<Player>() != null) {
            this.onPlayerOutCanal();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerEdgeCollider>() != null) {
            other.gameObject.GetComponent<PlayerEdgeCollider>().setCanalStatus(true);
        }
    }


    public void rotate90(bool clockwise) {
        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((WaterSource.CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % WaterSource.CANAL_ENTRANCE_COUNT);
        }
    }

    private void onPlayerInCanal() {
        this.edgeCollider.SetActive(true);

        foreach(Grate g in this.grates) {
            g.onPlayerInCanal();
        }
    }

    private void onPlayerOutCanal() {
        this.edgeCollider.SetActive(false);

        foreach(Grate g in this.grates) {
            g.onPlayerOutCanal();
        }
    }

    //this is scuffed but kinda needed for R4 to not make the room even worse
    public void hideCanal() {
        this.rend.enabled = false;
        this.canalCollider.enabled = false;
        this.waterCollider.SetActive(false);
    }

    public void returnCanal() {
        this.rend.enabled = true;

        this.canalCollider.enabled = !this.flooded;
        this.waterCollider.SetActive(this.flooded);
    }

    public GameObject getWaterCollider() {
        return this.waterCollider;
    }

    public bool isFlooded() {
        return this.flooded;
    }

    public Transform getClosestBackup(Transform target) {
        float closestVal = (this.backupTransforms[0].position - target.position).magnitude;
        Transform closest = this.backupTransforms[0];

        for(int i = 1; i < this.backupTransforms.Count; i++) {
            if((this.backupTransforms[i].position - target.position).magnitude < closestVal) {
                closestVal = (this.backupTransforms[i].position - target.position).magnitude;
                closest = this.backupTransforms[i];
            }
        }

        return closest;
    }
}

