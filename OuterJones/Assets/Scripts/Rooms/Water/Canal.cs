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
    [SerializeField] RotationPuzzleManager rPuzzleManager;
    private Room room;

    [SerializeField] private Tilemap canalTilemap; // Assign in inspector

    [SerializeField] private GameObject waterCollider;
    [SerializeField] private SkinnySectionManager skinnySection;
    [SerializeField] private GameObject edgeCollider;
    [SerializeField] private List<Transform> backupTransforms;

    [SerializeField] private bool flooded = false;
    [SerializeField] private Player player;
    [SerializeField] private bool keepsWaterWhenBlocked = false;

    private bool playerInCanal = false;

    private bool reachedThisFlood = false;
    private bool reachedThisDrain = false;

    private Renderer rend;
    private List<CanalEntrances> initialCanalEntrances;

    public void Awake() { //awake or start?
        this.room = GetComponentInParent<Room>();
        this.edgeCollider.SetActive(false);

        this.waterCollider.SetActive(this.flooded);

        this.rend = GetComponent<Renderer>();
        this.player = FindObjectOfType<Player>();

        this.initialCanalEntrances = new List<CanalEntrances>(this.canalEntrances);

        foreach(Grate g in this.grates) {
            g.init(this);
        }

        if(this.flooded) {
            foreach(Floodable f in this.floodableObjects) {
                f.onFlood(false);
            }

            this.skinnySection.onFlood();
        }

        FindObjectOfType<Inventory>().addItemListener(PossibleItems.Floaties, this.waterCollider.GetComponent<WaterColliderManager>());

    }

    public Room TEMP_DELETE_ME() {
        return this.room;
    }
    
    public void onFlood(CanalEntrances? floodingFrom, bool fromSource) {
        if(this.reachedThisFlood || !this.gameObject.activeInHierarchy) {
            return;
        }

        this.reachedThisFlood = true;

        if(!this.flooded) {
            this.flooded = true;

            this.waterCollider.SetActive(true);

            foreach(Floodable f in this.floodableObjects) {
                f.onFlood(fromSource);
            }
        }

        this.skinnySection.onFlood();

        foreach(Dam d in this.attatchedDams) {
            d.onFlood(this, floodingFrom, fromSource);
        }

        List<CanalEntrances> floodTo = new List<CanalEntrances>(this.canalEntrances);

        if(floodingFrom != null) {
            floodTo.Remove((CanalEntrances)floodingFrom);
        }  

        this.room.floodNeighbors(floodTo, fromSource);
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
            this.waterCollider.SetActive(false);
            this.flooded = false;

            foreach(Floodable f in this.floodableObjects) {
                f.drainWater();
            }
        }

        this.skinnySection.onDrain();

        foreach(Dam d in this.attatchedDams) {
            d.drainWater(this, drainingFrom);
        }

        List<CanalEntrances> drainTo = new List<CanalEntrances>(this.canalEntrances);

        if(drainingFrom != null) {
            drainTo.Remove((CanalEntrances)drainingFrom);
        }

        this.room.drainNeighbors(drainTo);
    }

    public bool willDrain(CanalEntrances drainingFrom) {
        return !this.reachedThisDrain && this.canalEntrances.Contains(drainingFrom);
    }
    void OnTriggerStay2D(Collider2D other) {
        //this should not be triggering
        if(flooded || player.isGrappling()) {
            return;
        }

        if(other.gameObject.GetComponent<CanalFinderManager>() != null && !other.gameObject.GetComponent<CanalFinderManager>().isInCanal()) {
            StartCoroutine(checkPlayerGrateStatus(other.gameObject.GetComponent<CanalFinderManager>()));
        }

        if(other.gameObject.GetComponent<Player>() != null && !this.playerInCanal) {
            
            bool playerOnGrate = false;
            foreach(Grate g in this.grates) {
                playerOnGrate = playerOnGrate || g.isPlayerOnGrate();
            }

            if(playerOnGrate) {
                return;
            }

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

        if(!this.keepsWaterWhenBlocked && this.flooded) {
            this.waterCollider.SetActive(false);
            this.flooded = false;

            foreach(Floodable f in this.floodableObjects) {
                f.drainWater();
            }

            this.skinnySection.onDrain();
        }
    }

    void OnTriggerExit2D(Collider2D other) {

        if(other.gameObject.GetComponent<PlayerEdgeCollider>() != null) {
            other.gameObject.GetComponent<PlayerEdgeCollider>().setCanalStatus(false);
        }

        if(flooded) {
            return;
        }

        if(other.gameObject.GetComponent<Player>() != null) {
            this.onPlayerOutCanal();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerEdgeCollider>() != null) {
            other.gameObject.GetComponent<PlayerEdgeCollider>().setCanalStatus(true);
        }

        if(flooded || this.player.isGrappling()) {
            return;
        }

        if(other.gameObject.GetComponent<CanalFinderManager>() != null) {
            StartCoroutine(checkPlayerGrateStatus(other.gameObject.GetComponent<CanalFinderManager>()));
        }        
    }

    private IEnumerator checkPlayerGrateStatus(CanalFinderManager finder) {
        Transform cTarget = finder.fallInCanal(this);

        yield return new WaitForFixedUpdate();

        bool playerOnGrate = false;
        foreach(Grate g in this.grates) {
            playerOnGrate = playerOnGrate || g.isPlayerOnGrate();
        }

        if(!playerOnGrate) {
            StartCoroutine(finder.shoveIntoCanal(cTarget));
        }
    }


    public void rotate90(bool clockwise) {
        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((WaterSource.CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % WaterSource.CANAL_ENTRANCE_COUNT);
        }
    }

    private void onPlayerInCanal() {
        this.playerInCanal = true;
        this.edgeCollider.SetActive(true);

        this.room.onPlayerInCanal();

        foreach(Grate g in this.grates) {
            g.onPlayerInCanal();
        }

        if(this.rPuzzleManager != null) {
            rPuzzleManager.onPlayerInCanal();
        }

    }

    private void onPlayerOutCanal() {
        this.edgeCollider.SetActive(false);
        this.playerInCanal = false;

        this.room.onPlayerOutCanal();

        foreach(Grate g in this.grates) {
            g.onPlayerOutCanal();
        }

        if(this.rPuzzleManager != null) {
            rPuzzleManager.onPlayerOutCanal();
        }
    }

    //this is scuffed but kinda needed for R4 to not make the room even worse
    public void hideCanal() {
        this.rend.enabled = false;
        this.waterCollider.SetActive(false);
    }

    public void returnCanal() {
        this.rend.enabled = true;
        
        this.waterCollider.SetActive(this.flooded);
    }

    public WaterColliderManager getWaterCollider() {
        return this.waterCollider.GetComponent<WaterColliderManager>();
    }

    public SkinnySectionManager getSkinnyCollider() {
        return this.skinnySection;
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

