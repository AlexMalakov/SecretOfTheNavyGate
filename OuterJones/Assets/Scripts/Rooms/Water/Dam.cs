using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dam : MonoBehaviour, Effectable
{
    [SerializeField] private Canal c1;
    [SerializeField] private Canal c2;

    [SerializeField] private bool open;

    private WaterSource source;
    private Collider2D damCollider;

    public void Awake() {
        this.source = FindObjectOfType<WaterSource>();
        this.damCollider = GetComponent<Collider2D>();
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

    // void OnTriggerStay2D(Collider2D other) {
    //     if(Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponent<Player>() != null) {
    //         this.open = !open;
    //         source.onWaterUpdate();
    //     }
    // }

    public void openDam() {
        this.open = true;
        source.onWaterUpdate();
        this.damCollider.enabled = false;
    }

    public void closeDam() {
        this.open = false;
        this.damCollider.enabled = true;
        source.onWaterUpdate();
    }

    public void onEffect() {
        if(this.open) {
            this.closeDam();
        } else {
            this.openDam();
        }
    }

    public void reset() {

    }
}
