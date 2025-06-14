using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//canal endings can either be dams or wall
public class Canal : MonoBehaviour
{
    //list of int 0 - 15
    [SerializeField] List<CanalEntrances> canalEntrances;
    [SerializeField] List<Dam> attatchedDams;
    [SerializeField] Room room;


    [SerializeField] GameObject floodedSprite;
    [SerializeField] GameObject drainedSprite;

    [SerializeField] GameObject edgeCollider;
    [SerializeField] Collider2d enterCollider;

    private bool flooded = false;
    
    public void onFlood(List<CanalEntrances> floodingFrom) {
        if(this.flooded) {
            return;
        }

        this.flooded = true;
        this.floodedSprite.SetActive(true);
        this.edgeCollider.SetActive(true);
        this.enterCollider.SetActive(false);
        this.drainedSprite.SetActive(false);

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
        this.flooded = false;
        this.floodedSprite.SetActive(false);
        this.edgeCollider.SetActive(false);
        this.enterCollider.SetActive(true);
        this.drainedSprite.SetActive(true);
    }
}

