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

    [SerializeField] private UnderbellyRoom underbellyRoom; //this is bad but like i didn't do rooms as a prefab (mistake) and don't want to manually
    //swap 25 rooms so this is going to be the solution

    protected RoomCoords position;
    protected Quaternion initialRotation;
    private bool playerInRoom = false;

    private PopUpManager manager;
    
    public void Awake() {
        this.floorAndWallHolderRot = this.floorAndWallHolder.transform.rotation;
        this.initialRotation = transform.rotation;
        this.manager = FindObjectOfType<PopUpManager>();
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

        if(this.position.overworld && this.getUnderbellyPair() != null) {
            this.getUnderbellyPair().init(position.swapFloor());
            this.layoutManager.placeInUnderbelly(position.swapFloor(), this.getUnderbellyPair());
        }
    }

    public virtual void onEnter(Door d) {
        // this.gameObject.SetActive(true);
        this.layoutManager.getCam().transform.position = new Vector3(this.transform.position.x, this.transform.position.y ,this.layoutManager.getCam().transform.position.z);
        globalLighting.intensity = this.roomLighting;
        this.manager.displayRoomPopUp(this.getRoomName());
        playerInRoom = true;

        if(this.getUnderbellyPair() != null) {
            this.getUnderbellyPair().onEnter(d);
        }
    }

    public virtual void onExit() {
        // this.gameObject.SetActive(false);
        playerInRoom = false;
        if(this.getUnderbellyPair() != null) {
            this.getUnderbellyPair().onExit();
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

    public virtual UnderbellyRoom getPair() {
        return this.underbellyRoom;
    }


    public string getRoomName() {
        return this.roomName;
    }

    ///////////////////////////////////////////// CANAL ROOMS
    [Header ("Canal Info")]
    [SerializeField] protected List<CanalEntrances> canalEntrances;
    [SerializeField] protected List<Canal> canals;

    [SerializeField] private GameObject canalEnviromentHider;
    //since canals can exist in non water rooms, all water functionality gets to live in room :'(


    public virtual void onFlood(CanalEntrances floodingFrom) {
        foreach(Canal c in this.canals) {
            if(c.willFlood(floodingFrom)) {
                c.onFlood(floodingFrom);
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

    public virtual void floodNeighbors(List<CanalEntrances> exits) {
        foreach(CanalEntrances exit in exits) {
            if(this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1], this.position.overworld) != null) {
                CanalEntrances opposite = (CanalEntrances)(((int)exit + (WaterSource.CANAL_ENTRANCE_COUNT/2)) % WaterSource.CANAL_ENTRANCE_COUNT);
                this.layoutManager.getRoomAt(this.position.x + WaterSource.CANAL_N_MAP[exit][0], this.position.y + WaterSource.CANAL_N_MAP[exit][1], this.position.overworld).onFlood(opposite);
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
                c.onFlood(null);
            }
        }
    }

    //idea: darken areas outside canal to make it more obvious that the player is now inside the canal
    public void onPlayerInCanal() {
        if(this.canalEnviromentHider != null) {
            this.canalEnviromentHider.SetActive(true);
        }
    }

    public void onPlayerOutCanal() {
        if(this.canalEnviromentHider != null) {
            this.canalEnviromentHider.SetActive(false);
        }
    }

    //////////////////////////////////////////////
    //functionality for L/D rooms
    [Header ("LD Info")]
    [SerializeField] protected Mirror mirror;
    [SerializeField] protected LightSink lSink;
    protected List<BeamModel> beams = new List<BeamModel>();

    //this is a chungus of a method cuz of mirrors and non mirrors :'(
    //since all rooms can have mirrors/sinks then a lot of code gets to be moved here yipee I love big classes!!!!!! :D
    //incoming direction is the door from which the beam is coming from
    public virtual void receiveBeam(DoorDirection incomingDirection) {
        //only receive beam if we have a door that it can enter through
        if(this.hasDoorDirection(this.getEntrance(incomingDirection).getDirection()/*.getInverse()*/)) {
    
            //if we have a sink power it, then pass the b
            if(this.lSink != null && this.lSink.getIncomingDirectionToActivate() == incomingDirection) {
                this.lSink.activate(incomingDirection);

                if(!isUniqueBeam(incomingDirection, incomingDirection)) {
                    return;
                }
                BeamModel b = BeamPool.getBeam();
                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.lSink.transform.position,
                    incomingDirection,
                    incomingDirection);
                this.beams.Add(b);
                return; //the beam "ends here"
            }

            DoorDirection exitDirection;
            if(this.mirror != null && !this.mirror.hasCobWebs()) { //if we have a mirror, we draw the light as if it bounces
                exitDirection = this.mirror.reflect(incomingDirection); //exit direction is wherever we get reflected

                if(!isUniqueBeam(null, exitDirection)) {
                    return;
                }

                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);

                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.mirror.transform.position,
                    incomingDirection,
                    null);

                BeamModel bb = BeamPool.getBeam();
                this.beams.Add(bb);

                bb.initBeam(
                    this.transform,
                    this.mirror.transform.position,
                    this.getPointInDirection(exitDirection).position,
                    null,
                    exitDirection);

            } else if(this.mirror != null) { //we have webs
                if(!isUniqueBeam(incomingDirection, null)) {
                    return;
                }
                BeamModel b = BeamPool.getBeam();
                this.beams.Add(b);

                b.initBeam(
                    this.transform,
                    this.getPointInDirection(incomingDirection).position,
                    this.mirror.transform.position,
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
        if(this.lSink != null) {
            this.lSink.deactivate();
        }

        for(int i = 0; i < this.beams.Count; i++) {
            this.beams[i].killBeam();
        }

        this.beams = new List<BeamModel>();
    }

    public virtual void rotateLight90(bool clockwise) {
        if(this.mirror != null) {
            this.mirror.rotate90();
        }

        if(this.lSink != null) {
            this.lSink.rotate90(clockwise);
        }
    }

    ///////////////////////////////////////////////
    //rotation room functionality

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

        if(this.position.overworld && this.getUnderbellyPair() != null) {
            this.getUnderbellyPair().rotate90(clockwise); //flip if we want to change the direction
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
}