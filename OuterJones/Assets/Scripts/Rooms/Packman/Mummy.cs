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
    
    [SerializeField] private MummyManager manager;

    private Vector3 startingPos;
    

    //mummy improvement features:
    //on collision - reset the packman room

    public void Awake() {
        this.agent = GetComponent<NavMeshAgent>();
        
        this.agent.updateRotation = false;
        this.agent.updateUpAxis = false;

        this.agent.enabled = false;
        this.startingPos = this.transform.localPosition;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.manager.resetRoom();
        }
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

        if(Mathf.Abs(this.transform.position.x - target.position.x) < .1f) {
            agent.SetDestination((target.position + new Vector3(.1f, 0f, 0f)));
        } else {
            agent.SetDestination(target.position);
        }

        if (agent.isStopped != !playerMoving) {
            agent.isStopped = !playerMoving;
        }
    }

    public void navigateFromTarget(Transform target, bool playerMoving) {
        if((this.transform.position - target.position).magnitude < this.distanceToSpeedUp) { //move faster if closer to player
            this.agent.speed = farSpeed;
        } else {
            this.agent.speed = closeSpeed;
        }

        if (agent.isStopped != !playerMoving) {
            agent.isStopped = !playerMoving;
        }

        if(!agent.isStopped) {
            agent.velocity = this.agent.speed * (this.transform.position - target.position).normalized;
        }
    }

    public void resetPosition() {
        this.agent.enabled = false;
        this.transform.localPosition = startingPos;
        this.agent.enabled = true;
    }
} 
