using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManager : MonoBehaviour
{
    [SerializeField] private bool targetPlayer;
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> possibleTargets;
    [SerializeField] private PackmanRoom pRoom;

    private bool targetingLeft = true;

    [SerializeField] Mummy mummy;
    private bool mummyIsAwake = false;

    public void wakeMummy() {
        if(this.mummy != null) {
            this.mummyIsAwake = true;
            this.mummy.wake();
        }
    }

    public void sleepMummy() {
        if(this.mummy != null) {
            this.mummyIsAwake = false;
            this.mummy.sleep();
            this.mummy.resetPosition();
        }
    }

    public void Update() {
        if(this.mummy == null || !mummyIsAwake) {
            return;
        }

        if(targetPlayer) {
            Transform target = this.targetingLeft ? this.player.getMummyTargets()[0] : this.player.getMummyTargets()[1];
            if(Mathf.Abs(this.mummy.transform.position.x - target.position.x) < .001f) {
                Debug.Log("flipped!");
                this.targetingLeft = !this.targetingLeft;
            }
            target = this.targetingLeft ? this.player.getMummyTargets()[0] : this.player.getMummyTargets()[1];

            this.mummy.navigateToTarget(target, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
        } else {
            Transform closest = this.mummy.transform;
            float closestVal = 9999;
            foreach(Transform target in possibleTargets) {
                if((target.position - this.player.transform.position).magnitude < closestVal) {
                    closest = target;
                    closestVal = (target.position - this.player.transform.position).magnitude;
                }
            }

            this.mummy.navigateToTarget(closest, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
        }
    }

    public void resetRoom() {
        this.pRoom.resetPackmanRoom(this.player);
        this.mummy.resetPosition();
    }
}
