using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBridgeEntrance : MonoBehaviour
{
    [SerializeField] private bool upperLevel;
    private LandBridge bridgeParent;

    public void Awake() {
        this.bridgeParent = GetComponentInParent<LandBridge>();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.bridgeParent.notifyOfPlayer(this.upperLevel);
        }
    }
}
