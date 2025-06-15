using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dam : MonoBehaviour
{
    [SerializeField] private Canal c1;
    [SerializeField] private Canal c2;

    [SerializeField] private bool open;

    private WaterSource source;

    public void Start() {
        this.source = FindObjectOfType<WaterSource>();
    }


    public void onFlood(Canal c, List<CanalEntrances> floodingFrom) {
        if(!open) {
            return;
        }

        if(c == c1) {
            c2.onFlood(floodingFrom);
        }else if(c == c2) {
            c1.onFlood(floodingFrom);
        }
    }

    public void OnTriggerStay2D(Collider2D other) {
        if(Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponent<Player>() != null) {
            this.open = !open;
            source.onWaterUpdate();
        }
    }
}
