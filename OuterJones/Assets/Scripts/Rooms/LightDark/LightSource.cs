using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour, RoomUpdateListener
{
    //room info
    [SerializeField] private LightDarkRoom originRoom;
    private BeamModel beam;
    private DoorDirection castDirection = DoorDirection.North;


    [SerializeField] private Transform beamParent;
    [SerializeField] private GameObject beamPrefab;
    private int BEAM_POOL_SIZE = 15;
    
    public void Awake() {
        BeamPool.init(beamPrefab, beamParent, BEAM_POOL_SIZE);

        //we don't use the pool because this room always has a beam
        this.beam = Instantiate(beamPrefab, beamParent).GetComponent<BeamModel>();
        this.originRoom.setSource(this);
    }

    public void castBeam() {
        this.beam.initBeam(this.originRoom.transform, this.transform.position, this.originRoom.getPointInDirection(castDirection).position);

        this.originRoom.beamNeighbor(castDirection);
    }

    public void onRoomUpdate(List<Room> rooms) {
        //we don't kill our beam because it's ours and no one can take it from us 
        
        foreach(Room room in this.originRoom.getLayoutManager().getAllRooms()) {
            room.removeBeam();
        }

        this.castBeam();
    }

    public void rotate90(bool clockwise) {
        this.castDirection = Door.rotateDoorDirection(this.castDirection, clockwise);
    }
}

public static class BeamPool {
    private static List<BeamModel> pool = new List<BeamModel>();
    private static bool initialized = false;
    private static int EXPAND_POOL_BY = 5;
    
    internal static void init(GameObject beamPrefab, Transform beamParent, int count) {
        if (initialized) return;
        initialized = true;

        for (int i = 0; i < count; i++) {
            pool.Add(GameObject.Instantiate(beamPrefab, beamParent).GetComponent<BeamModel>());
        }
    }

    public static BeamModel getBeam() {

        foreach (BeamModel b in pool) {
            if(!b.isActive()) {
                return b;
            }
        }

        if(EXPAND_POOL_BY > 0) {
            return growPool();
        }

        return null;
    }

    private static BeamModel growPool() {
        int count = pool.Count;

        for (int i = 0; i < EXPAND_POOL_BY; i++) {
            pool.Add(GameObject.Instantiate(pool[0].gameObject, pool[0].transform.parent).GetComponent<BeamModel>());
        }

        return pool[count];
    }
}
