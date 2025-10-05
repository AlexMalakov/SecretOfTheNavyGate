using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightSourceManager : MonoBehaviour, RoomUpdateListener
{
    private List<LightSource> sources = new List<LightSource>();
    [SerializeField] RoomsLayout layout;

    [Header ("beam pool")]
    [SerializeField] private Transform beamParent;
    [SerializeField] private GameObject beamPrefab;
    private int BEAM_POOL_SIZE = 20;

    public void Awake() {
        //TODO: Shouldn't have a beam in a dark room
        BeamPool.init(beamPrefab, beamParent, BEAM_POOL_SIZE);
    }

    public void Start() {
        layout.addRoomUpdateListener(this);
    }

    public BeamModel addLightSource(LightSource source) {
        this.sources.Add(source);
        return Instantiate(beamPrefab, beamParent).GetComponent<BeamModel>();
    }

    public void resetBeams() {
        foreach(Room r in this.layout.getAllRooms(true)) {
            r.removeBeam();
        }

        foreach(Room r in this.layout.getAllRooms(false)) {
            r.removeBeam();
        }
    }

    public void onRoomUpdate(List<Room> rooms) {
        this.resetBeams();

        foreach(LightSource s in this.sources) {
            s.castBeam();
        }
    }
}


public static class BeamPool {
    private static List<BeamModel> pool = new List<BeamModel>();
    private static int EXPAND_POOL_BY = 5;
    
    internal static void init(GameObject beamPrefab, Transform beamParent, int count) {
        pool = new List<BeamModel>();
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
