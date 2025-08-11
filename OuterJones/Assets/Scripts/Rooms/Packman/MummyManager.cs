using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManager : MonoBehaviour
{
    [SerializeField] private bool targetPlayer;
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> possibleTargets;


    [SerializeField] Mummy mummy;


    public void FixedUpdate() {

        if(targetPlayer) {
            this.mummy.navigateToTarget(player.transform, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
        } else {
            Transform closest = this.mummy.transform.position;
            float closestVal = (this.mummy.transform.position - this.player.transform.position).magnitude;
            foreach(Transform target in possibleTargets) {
                if((target.position - this.player.transform.position).magnitude < closestVal) {
                    closest = target.position;
                    closestVal = (target.position - this.player.transform.position).magnitude;
                }
            }

            this.mummy.navigateToTarget(target, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
        }
    }
}
