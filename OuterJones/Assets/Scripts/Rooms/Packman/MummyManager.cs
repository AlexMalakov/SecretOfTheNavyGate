using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManager : MonoBehaviour, ItemListener
{
    [SerializeField] private bool targetPlayer;
    [SerializeField] private Player player;
    [SerializeField] private List<Transform> possibleTargets;
    [SerializeField] private PackmanRoom pRoom;
    [SerializeField] Inventory inventory;
    [SerializeField] private PlayerShadow playerShadow;

    private bool amuletActive = false;

    private float timeBetweenFlip = .1f; //.35 this is a really dumb idea but will probably work

    private bool targetingLeft = true;
    private bool chasingShadow = false;

    [SerializeField] Mummy mummy;
    private bool mummyIsAwake = false;

    void Awake() {
        this.inventory.addItemListener(PossibleItems.Amulet, this);
    }

    public void onItemEvent(bool status) {
        this.amuletActive = status; 
    }

    public void wakeMummy(bool shadow) {
        if(this.mummy != null) {
            this.chasingShadow = shadow;
            this.mummyIsAwake = true;
            this.mummy.wake();
        }

        if(this.chasingShadow) {
            this.playerShadow.wakeShadow(this.pRoom);
        }
    }

    public void sleepMummy() {
        if(this.mummy != null) {
            this.mummyIsAwake = false;
            this.mummy.sleep();
            this.mummy.resetPosition();
        }

        if(this.chasingShadow) {
            this.playerShadow.endTask();
        }
    }

    public void Update() {
        if(this.mummy == null || !mummyIsAwake) {
            return;
        }
        
        if(targetPlayer) {
            this.targetingLeft = (((int)(Time.time / this.timeBetweenFlip))%2) == 0;

            Transform target = this.targetingLeft ? this.player.getMummyTargets()[0] : this.player.getMummyTargets()[1];
            if(this.chasingShadow) {
                target = this.targetingLeft ? this.playerShadow.getMummyTargets()[0] : this.playerShadow.getMummyTargets()[1];
            }

            if(this.amuletActive) {
                this.mummy.navigateFromTarget(target, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
            } else {
                this.mummy.navigateToTarget(target, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
            }
        } else {
            Transform best = this.mummy.transform;
            float bestVal = amuletActive? -9999 : 9999;
            Vector3 getBestFor = this.chasingShadow ? this.playerShadow.transform.position : this.player.transform.position;
            foreach(Transform target in possibleTargets) {
                if((!this.amuletActive && ((target.position - getBestFor).magnitude < bestVal))
                      || (this.amuletActive && ((target.position - getBestFor).magnitude > bestVal))) {

                    best = target;
                    bestVal = (target.position - getBestFor).magnitude;
                }
            }

            this.mummy.navigateToTarget(best, this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving());
        }
    }

    public void resetRoom() {
        this.pRoom.resetPackmanRoom(this.player);
        this.mummy.resetPosition();
    }
}
