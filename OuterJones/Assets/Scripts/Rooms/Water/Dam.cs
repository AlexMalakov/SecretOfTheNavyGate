using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dam : MonoBehaviour, Effectable
{
    [SerializeField] private Canal c1;
    [SerializeField] private Canal c2;

    [SerializeField] private bool initiallyOpen;
    private bool open;

    [SerializeField] private GameObject openObj;
    [SerializeField] private GameObject closedObj;

    private WaterSourceManager sourceMan;

    public void Awake() {
        this.sourceMan = FindObjectOfType<WaterSourceManager>();
        this.open = this.initiallyOpen;
        this.openObj.SetActive(this.open);
        this.closedObj.SetActive(!this.open);
    }


    public void onFlood(Canal c, CanalEntrances? floodingFrom) {
        if(!open) {
            return;
        }

        if(c == c1) {
            c2.onFlood(floodingFrom);
        }else if(c == c2) {
            c1.onFlood(floodingFrom);
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
        this.sourceMan.onRoomUpdate(new List<Room>());
    }

    public void reset() {
        this.open = this.initiallyOpen;
        this.openObj.SetActive(this.open);
        this.closedObj.SetActive(!this.open);
    }
}
