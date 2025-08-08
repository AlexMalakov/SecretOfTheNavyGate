using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mummy : MonoBehaviour
{
    private Player player;
    private NavMeshAgent agent;

    private bool isAwake;

    //mummy improvement features:
    //on collision - reset the packman room

    public void Awake() {
        this.player = FindObjectOfType<Player>();
        this.agent = GetComponent<NavMeshAgent>();
        
        this.agent.updateRotation = false;
        this.agent.updateUpAxis = false;

        this.agent.enabled = false;
    }

    public void wake() {
        this.isAwake = true;
        this.agent.enabled = true;
    }

    public void sleep() {
        this.isAwake = false;
        this.agent.enabled = false;
    }

    public void FixedUpdate() {
        if(isAwake) {
            agent.SetDestination(this.player.transform.position);

            if (agent.isStopped != !this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving()) {
                agent.isStopped = !this.player.gameObject.GetComponent<PlayerController>().isPlayerMoving();
            }
        }
        
    }
}
