using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{

    [Header ("Room info")]
    [SerializeField] private string roomName;
    [SerializeField] protected float roomLighting = .5f;
    [SerializeField] private Light2D globalLighting;
    [SerializeField] protected RoomsLayout layoutManager;
    [SerializeField] protected List<Door> doors;
    [SerializeField] protected Sprite roomSprite;

    [Header("underbelly")]
    [SerializeField] private Room roomPair;

    [Header ("Beam target transforms")]
    [SerializeField] private Transform northPosition;
    [SerializeField] private Transform eastPosition;
    [SerializeField] private Transform southPosition;
    [SerializeField] private Transform westPosition;

    [Header("Other (key-doors)")]
    [SerializeField] private GameObject floorAndWallHolder;
    private Quaternion floorAndWallHolderRot;
    [SerializeField] private List<GateDoor> gateDoors;
    [SerializeField] private List<Chest> chests;
    [SerializeField] private ListenerController objListeners;

    protected RoomCoords position;
    protected Quaternion initialRotation;
    private bool playerInRoom = false;

    private Map map;
    
    public void Awake() {
        this.floorAndWallHolderRot = this.floorAndWallHolder.transform.rotation;
        this.initialRotation = transform.rotation;
        this.map = FindObjectOfType<Map>();

        this.effectibleButtons = GetComponentsInChildren<TemporaryEffectableButton>();
        this.powerableButtons = GetComponentsInChildren<PowerableButton>();
        this.rotationButtons = GetComponentsInChildren<RotationPuzzleButton>();
    }

    public virtual void init(RoomCoords position) {
        this.gameObject.SetActive(true);
        this.position = position;

        foreach(Door d in this.doors) {
            if(d.getDestination() == null 
                && this.layoutManager.getRoomAt(this.position.getOffset(d.getDirection())) != null
                && this.layoutManager.getRoomAt(this.position.getOffset(d.getDirection())).hasDoorDirection(d.getInverse())) {

                
                d.setDestination(this.layoutManager.getRoomAt(this.position.getOffset(d.getDirection())).getEntrance(d.getInverse()));
                d.getDestination().getRoom().getEntrance(d.getInverse()).setDestination(d);
            }
        }

        if(this.objListeners != null) {
            objListeners.onRoomMove();
        }
    }

    public virtual void onEnter(Door d) {
        this.enterAbstraction();
    }

    public void hideRoom() {
        this.getPair().gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public virtual void onEnter(UnderbellyStaircase staircase) {
        this.enterAbstraction();
    }

    private void enterAbstraction() {
        this.layoutManager.getCam().transform.position = new Vector3(this.transform.position.x, this.transform.position.y ,this.layoutManager.getCam().transform.position.z);
        globalLighting.intensity = this.roomLighting;
        this.map.onNewRoomEntered(this);
        playerInRoom = true;
    }

    public virtual void onExit() {
        // this.gameObject.SetActive(false);
        playerInRoom = false;
        if(this.getPosition().overworld) {
            this.getPair().onExit();
        }
    }


    public bool hasDoorDirection(DoorDirection direction) {
        foreach(Door d in this.doors) {
            if(d.getDirection() == direction) {
                return true;
            }
        }
        return false;
    }

    public RoomsLayout getLayoutManager() {
        return this.layoutManager;
    }

    public RoomCoords getPosition() {
        return this.position;
    }

    public Door getEntrance(DoorDirection direction) {
        foreach(Door d in this.doors) {
            if(d.getDirection() == direction) {
                return d;
            }
        }
        throw new InvalidOperationException("DOOR WITH THE DIRECTION " + direction + "  DOES NOT EXIST IN ROOM " + gameObject.name);
    }

    public virtual Sprite getRoomSprite() {
        return this.roomSprite;
    }

    public List<Door> getDoors() {
        return this.doors;
    }

    public Transform getPointInDirection(DoorDirection d) {
        switch(d) {
            case DoorDirection.North:
                return this.northPosition;
            case DoorDirection.East:
                return this.eastPosition;
            case DoorDirection.West:
                return this.westPosition;
            case DoorDirection.South:
                return this.southPosition;
        }

        return null;
    }

    public virtual Room getPair() {
        return this.roomPair;
    }


    public string getRoomName() {
        return this.roomName;
    }

    ///////////////////////////////////////////// CANAL ROOMS
    [Header ("Canal Info")]
    [SerializeField] protected List<CanalEntrances> canalEntrances;
    [SerializeField] protected List<Canal> canals;

    [SerializeField] private GameObject canalEnviromentHider;
    private List<Canal> canalsPlayerIn = new List<Canal>();
    //since canals can exist in non water rooms, all water functionality gets to live in room :'(


    public virtual void onFlood(CanalEntrances floodingFrom, bool fromSource) {
        foreach(Canal c in this.canals) {
            if(c.willFlood(floodingFrom)) {
                c.onFlood(floodingFrom, fromSource);
            }
        }
    }

    public virtual void drainWater(CanalEntrances floodingFrom) {
        foreach(Canal c in this.canals) {
            if(c.willDrain(floodingFrom)) {
                c.drainWater(floodingFrom);
            }
        }
    }

    public virtual void restartFlood() {
        foreach(Canal c in this.canals) {
            c.restartFlood();
        }
    }

    public virtual void floodNeighbors(List<CanalEntrances> exits, bool fromSource) {
        foreach(CanalEntrances exit in exits) {
            if(this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1], this.position.overworld) != null) {
                CanalEntrances opposite = (CanalEntrances)(((int)exit + (WaterSource.CANAL_ENTRANCE_COUNT/2)) % WaterSource.CANAL_ENTRANCE_COUNT);
                this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1], this.position.overworld).onFlood(opposite, fromSource);
            }
        }
    }

    public virtual void drainNeighbors(List<CanalEntrances> exits) {
        foreach(CanalEntrances exit in exits) {
            if(this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1], this.position.overworld) != null) {
                CanalEntrances opposite = (CanalEntrances)(((int)exit + (WaterSource.CANAL_ENTRANCE_COUNT/2)) % WaterSource.CANAL_ENTRANCE_COUNT);
                this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1], this.position.overworld).drainWater(opposite);
            }
        }
    }

    private void rotateCanals90(bool clockwise) {
        for(int i = 0; i < this.canalEntrances.Count; i++) {
            this.canalEntrances[i] = (CanalEntrances)((WaterSource.CANAL_ENTRANCE_COUNT + (int)this.canalEntrances[i] + (clockwise ? 2 : -2)) % WaterSource.CANAL_ENTRANCE_COUNT);
        }
        foreach(Canal c in this.canals) {
            c.rotate90(clockwise);
        }
    }


    //floods from all canals that have not already flooded and are able
    public void floodAllRemainingCanals() {
        foreach(Canal c in this.canals) {
            if(c.isFlooded()) {
                //if reachedThisFlood = true, then this will immediately exit
                c.onFlood(null, false);
            }
        }
    }

    //idea: darken areas outside canal to make it more obvious that the player is now inside the canal
    public void onPlayerInCanal(Canal c) {
        this.canalsPlayerIn.Add(c);
        if(this.canalEnviromentHider != null) {
            this.canalEnviromentHider.SetActive(true);
        }
    }

    public void onPlayerOutCanal(Canal c) {
        this.canalsPlayerIn.Remove(c);
        if(this.canalEnviromentHider != null && this.canalsPlayerIn.Count == 0) {
            this.canalEnviromentHider.SetActive(false);
        }
    }

    //////////////////////////////////////////////
    //functionality for L/D rooms
    [Header ("LD Info")]
    [SerializeField] protected Mirrors mirrors;
    [SerializeField] protected LightSinks lSinks;
    protected List<BeamModel> beams = new List<BeamModel>();
    private List<LightSource> sources = new List<LightSource>();

    //this is a chungus of a method cuz of mirrors and non mirrors :'(
    //since all rooms can have mirrors/sinks then a lot of code gets to be moved here yipee I love big classes!!!!!! :D
    //incoming direction is the door from which the beam is coming from
    public virtual void receiveBeam(DoorDirection incomingDirection) {
        //only receive beam if we have a door that it can enter through
        if(this.hasDoorDirection(this.getEntrance(incomingDirection).getDirection()/*.getInverse()*/)) {
    
            //if we have a sink power it, then pass the b
            if(this.lSinks != null && this.lSinks.canActivateSink(incomingDirection)) {
                LightSink sink = this.lSinks.activateSink(incomingDirection);

                if(!isUniqueBeam(incomingDirection, incomingDirection)) {
                    return;
                }
                BeamModel b = BeamPool.getBeam();
                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    sink.transform.position,
                    incomingDirection,
                    incomingDirection);
                this.beams.Add(b);
                return; //the beam "ends here"
            }

            DoorDirection exitDirection;
            if(this.mirrors != null && !this.mirrors.hasCobWebs(incomingDirection)) { //if we have a mirror, we draw the light as if it bounces
                exitDirection = this.mirrors.reflect(incomingDirection); //exit direction is wherever we get reflected

                if(!isUniqueBeam(null, exitDirection)) {
                    return;
                }

                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);

                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.mirrors.getStartingPoint(incomingDirection).position,
                    incomingDirection,
                    null);

                BeamModel bb = BeamPool.getBeam();
                this.beams.Add(bb);

                bb.initBeam(
                    this.transform,
                    this.mirrors.getEndingPoint(incomingDirection).position,
                    this.getPointInDirection(exitDirection).position,
                    null,
                    exitDirection);

            } else if(this.mirrors != null) { //we have webs
                if(!isUniqueBeam(incomingDirection, null)) {
                    return;
                }

                this.mirrors.reflect(incomingDirection);

                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);

                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.mirrors.getStartingPoint(incomingDirection).position,
                    incomingDirection,
                    null
                    );
                return;
            }else {
                exitDirection = this.getEntrance(incomingDirection).getInverse(); //exit direction is opposite of enter direction

                if(!isUniqueBeam(incomingDirection, exitDirection)) {
                    return;
                }

                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);
                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.getPointInDirection(exitDirection).position,
                    incomingDirection,
                    exitDirection);

            }

            //if we have a door at our exit direction, we'll send the beam through
            if(this.hasDoorDirection(exitDirection)) {
                this.beamNeighbor(exitDirection);
            }
        }
    }

    private bool isUniqueBeam(DoorDirection? start, DoorDirection? end) {
        foreach(BeamModel b in this.beams) {
            if(b.sameBeam(start, end)) {
                return false;
            }
        }
        return true;
    }

    
    public virtual void beamNeighbor(DoorDirection exitDirection) {
        if(this.layoutManager.getRoomAt(this.position.getOffset(exitDirection)) != null) {

            DoorDirection arrivedFrom = Door.rotateDoorDirection(Door.rotateDoorDirection(exitDirection, true), true); //im too lazy to right a static inverse
            this.layoutManager.getRoomAt(this.position.getOffset(exitDirection)).receiveBeam(arrivedFrom);
        }
    }

    public virtual void removeBeam() {
        if(this.lSinks != null) {
            this.lSinks.deactivateAll();
        }

        for(int i = 0; i < this.beams.Count; i++) {
            this.beams[i].killBeam();
        }

        if(this.mirrors != null) {
            this.mirrors.resetMirrorBeams();
        }

        this.beams = new List<BeamModel>();
    }

    public virtual void rotateLight90(bool clockwise) {
        if(this.mirrors != null) {
            this.mirrors.rotate90(clockwise);
        }

        foreach(LightSource source in this.sources) {
            source.rotate90(clockwise);
        }


        if(this.lSinks != null) {
            this.lSinks.rotate90(clockwise);
        }
    }

    public void setSource(LightSource s) {
        this.sources.Add(s);
    }

    public virtual bool canCastBeam() {
        return true;
    }

    ///////////////////////////////////////////////
    //rotation room functionality
    [Header ("packman corner piece for rotation")]
    [SerializeField] List<PackmanCornerPiece> cornerPieces;

    //ok so this is really where i needed to have a rotationProof super class or something
    //but also this class has the word temporary in it and ive had it for like several months...
    //anyway i need the like final final build on sat so you know im not writing that superclass :'(
    //sorry prof. lerner :(
    private TemporaryEffectableButton[] effectibleButtons;
    private PowerableButton[] powerableButtons;
    private RotationPuzzleButton[] rotationButtons;

    public virtual bool rotate90() {
        return this.rotate90(FindObjectOfType<Player>().getRotationDirection());
    }

    public virtual bool rotate90(bool clockwise) {
        //rotate game object
        PlayerController pc;
        if(this.playerInRoom) {
            pc = FindObjectOfType<PlayerController>();
            pc.transform.parent = this.transform;
            transform.Rotate(0f, 0f, (clockwise ? -90f : 90f));
            pc.transform.parent = null;
        } else {
            transform.Rotate(0f, 0f, (clockwise ? -90f : 90f));
        }

        this.floorAndWallHolder.transform.rotation = this.floorAndWallHolderRot;

        //fix non-rotatatble game objects (chests, fences)
        foreach(Chest c in this.chests) {
            c.rotate90(clockwise);
        }

        foreach(PackmanCornerPiece p in this.cornerPieces) {
            p.rotate90(clockwise);
        }

        foreach(GateDoor d in this.gateDoors) {
            d.rotate90(clockwise);
        }
    
        //rotate doors
        foreach(Door d in this.doors) {
            d.rotate90(clockwise);
        }

        foreach(Door d in this.doors) {
            d.updateNeighbor();
        }

        foreach(TemporaryEffectableButton b in this.effectibleButtons) {
            b.rotate90();
        }

        foreach(PowerableButton b in this.powerableButtons) {
            b.rotate90();
        }

        foreach(RotationPuzzleButton b in this.rotationButtons) {
            b.rotate90();
        }


        if(this.objListeners != null) {
            objListeners.rotate90(clockwise);
        }

        //rotate beam transforms :)
        Transform swapper = northPosition;
        if(clockwise) {
            northPosition = westPosition;
            westPosition = southPosition;
            southPosition = eastPosition;
            eastPosition = swapper;
        } else {
            northPosition = eastPosition;
            eastPosition = southPosition;
            southPosition = westPosition;
            westPosition = swapper;
        }

        rotateCanals90(clockwise);

        rotateLight90(clockwise);

        if(this.getPosition().overworld) {
            this.getPair().rotate90(clockwise); //flip if we want to change the direction
        }

        //handles canal and light re set, and map rotate
        this.layoutManager.notifyRoomListeners(new List<Room>(){this});

        return clockwise;
    }

    public void resetAllDoors() {
        foreach(Door d in this.doors) {
            d.resetDestination();
            d.updateNeighbor();
        }
    }

    //507 line class like could someone put out a warrant for my arrest
}