using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBridgeEntrance : MonoBehaviour
{
    [SerializeField] private bool upperLevel;
    private LandBridge bridgeParent;

    public void Start() {
        this.bridgeParent = GetComponentInParent<LandBridge>();
    }

    public void onTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.bridgeParent.notifyOfPlayer(this.upperLevel);
        }
    }
}
