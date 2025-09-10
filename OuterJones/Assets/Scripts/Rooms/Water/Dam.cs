using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dam : MonoBehaviour, Effectable
{
    [SerializeField] private Canal c1;
    [SerializeField] private Canal c2;

    [SerializeField] private bool open;

    [SerializeField] private GameObject openObj;
    [SerializeField] private GameObject closedObj;

    private WaterSourceManager sourceMan;

    public void Awake() {
        this.sourceMan = FindObjectOfType<WaterSourceManager>();
        this.openObj.SetActive(this.open);
        this.closedObj.SetActive(!this.open);
    }


    public void onFlood(Canal c, CanalEntrances? floodingFrom, bool fromSource) {
        if(!open) {
            return;
        }

        if(c == c1) {
            c2.onFlood(floodingFrom, fromSource);
        }else if(c == c2) {
            c1.onFlood(floodingFrom, fromSource);
        }
    }

    public void drainWater(Canal c, CanalEntrances? floodingFrom) {
        if(!open) {
            return;
        }

        if(c == c1) {
            c2.drainWater(floodingFrom);
        }else if(c == c2) {
            c1.drainWater(floodingFrom);
        }
    }


    public void openDam() {
        if(!this.open) {
            this.onEffect();
        }
    }

    public void closeDam() {
        if(this.open) {
            this.onEffect();
        }
    }

    public void onEffect() {
        this.open = !this.open;

        this.openObj.SetActive(this.open);
        this.closedObj.SetActive(!this.open);
        this.sourceMan.recomputeFlow();
    }

    public void onEffectOver(){}
}
