using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] public PlayerController pc;
    [SerializeField] public Animator animator;



    public void Update() {
        if(this.pc.getIfMovementEnabled() && this.pc.isPlayerMoving()) {
            this.animator.SetBool("isMoving", true);

            if(pc.getDirection3D().y > .05f) {
                this.animator.SetInteger("direction", 0);
            } else if(pc.getDirection3D().y < -.05f) {
                this.animator.SetInteger("direction", 2);
            } else if(pc.getDirection3D().x > .05f) {
                this.animator.SetInteger("direction", 3);
            } else if(pc.getDirection3D().x < -.05f) {
                this.animator.SetInteger("direction", 1);
            }
        } else {
            this.animator.SetBool("isMoving", false);
        }
    }
}
