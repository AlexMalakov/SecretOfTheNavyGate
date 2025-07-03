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
    [SerializeField] List<Grate> grates;
    private Room room;

    [SerializeField] private Tilemap canalTilemap; // Assign in inspector

    [SerializeField] private CompositeCollider2D canalCollider; //trigger when empty, collider when flooded
    [SerializeField] private GameObject waterCollider;
    [SerializeField] private GameObject edgeCollider; //collider attatched to external object

    private bool flooded = false;
    private Renderer rend;

    public void Awake() {
        this.room = GetComponentInParent<Room>();
        this.edgeCollider.SetActive(false);

        foreach(Floodable f in this.floodableObjects) {
            if(f is Ladder) {
                ((Ladder)f).init(this);
            }
        }

        this.rend = GetComponent<Renderer>();

        StartCoroutine(this.copyCollider());
        
    }

    //this is awful but i need it for floaties to not be a testing headache
    private IEnumerator copyCollider() {

        var tilemap = waterCollider.AddComponent<Tilemap>();
        var tilemapRenderer = waterCollider.AddComponent<TilemapRenderer>();

        var tilemapCollider = waterCollider.AddComponent<TilemapCollider2D>();
        tilemapCollider.usedByComposite = true;

        var compositeCollider = waterCollider.AddComponent<CompositeCollider2D>();
        compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;

        var rb = waterCollider.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        yield return new WaitForFixedUpdate();

        tilemapCollider.enabled = false;

        Destroy(tilemap);
        Destroy(tilemapRenderer);

    }
    
    public void onFlood(CanalEntrances? floodingFrom) {
        if(this.flooded) {
            return;
        }

        this.swapTiles();
        this.flooded = true;
        this.canalCollider.enabled = false;
        this.waterCollider.SetActive(true);

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

        this.canalCollider.enabled = true;
        this.waterCollider.SetActive(false);
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

    public void onLadderUse() {
        this.onPlayerOutCanal();
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
}

