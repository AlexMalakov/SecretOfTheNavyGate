using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mummy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float closeSpeed;
    [SerializeField] private float farSpeed;

    [SerializeField] private float distanceToSpeedUp;

    private Transform curTarget;
    

    //mummy improvement features:
    //on collision - reset the packman room

    public void Awake() {
        this.agent = GetComponent<NavMeshAgent>();
        
        this.agent.updateRotation = false;
        this.agent.updateUpAxis = false;

        this.agent.enabled = false;
    }

    public void wake() {
        this.agent.enabled = true;
    }

    public void sleep() {
        this.agent.enabled = false;
    }

    public void navigateToTarget(Transform target, bool playerMoving) {
        if((this.transform.position - target.position).magnitude > this.distanceToSpeedUp) {
            this.agent.speed = farSpeed;
        } else {
            this.agent.speed = closeSpeed;
        }

        if(target != this.curTarget) {
            agent.SetDestination(target.position);
            this.curTarget = target;
        }
        

        if (agent.isStopped != !playerMoving) {
            agent.isStopped = !playerMoving;
        }
    }
}
