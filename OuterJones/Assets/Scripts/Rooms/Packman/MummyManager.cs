using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManager : MonoBehaviour
{
    [SerializeField] private bool targetPlayer;
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> possibleTargets;


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
            this.mummyIsAwake = true;
            this.mummy.sleep();
        }
    }

    public void Update() {
        if(!mummyIsAwake) {
            return;
        }

        if(targetPlayer) {
            this.mummy.navigateToTarget(player.transform, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
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
}
