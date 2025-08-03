using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDPuzzle : MonoBehaviour
{
    [Header ("sinks")]
    [SerializeField] private LightSink r3Sink;
    [SerializeField] private LightSink l4Sink;
    [SerializeField] private LightSink r6Sink;

    [Header ("toggleable objs")]
    [SerializeField] private GameObject r3OnObj;
    [SerializeField] private GameObject r3OffObj;
    [SerializeField] private GameObject l4OnObj;
    [SerializeField] private GameObject l4OffObj;
    [SerializeField] private GameObject r6OnObj;
    [SerializeField] private GameObject r6OffObj;

    [SerializeField] private UnderbellyStaircase staircase;

    //this is spaghetti code but it's fine for throw away classes like this i hope
    private bool r3On;
    private bool l4On;
    private bool r6On;

    public void Awake() {
        this.r3Sink.init(this, "r3");
        this.l4Sink.init(this, "l4");
        this.r6Sink.init(this, "r6");
    }

    public void onActive(string id) {
        switch(id) {
            case "r3":
                this.r3On = true;
                break;
            case "l4":
                this.l4On = true;
                break;
            case "r6":
                this.r6On = true;
                break;
        }

        if(this.r3On && this.l4On && this.r6On) {
            this.staircase.onEffect();
        }

        this.updateObjs();
    }

    public void onDeactivate(string id) {
        switch(id) {
            case "r3":
                this.r3On = false;
                break;
            case "l4":
                this.l4On = false;
                break;
            case "r6":
                this.r6On = false;
                break;
        }

        this.updateObjs();
    }

    private void updateObjs() {
        this.r3OnObj.SetActive(this.r3On);
        this.r3OffObj.SetActive(!this.r3On);
        this.l4OnObj.SetActive(this.l4On);
        this.l4OffObj.SetActive(!this.l4On);
        this.r6OnObj.SetActive(this.r6On);
        this.r6OffObj.SetActive(!this.r6On);
    }
    
}
