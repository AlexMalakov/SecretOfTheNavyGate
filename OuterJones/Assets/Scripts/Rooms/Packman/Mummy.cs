using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mummy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float closeSpeed;
    [SerializeField] private float farSpeed;
    [SerializeField] private Animator animator;

    [SerializeField] private float distanceToSpeedUp;
    
    [SerializeField] private MummyManager manager;

    private Vector3 startingPos;

    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private Sprite downSprite;

    [SerializeField] private PlayerController playerController;
    

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
        if(other.GetComponent<Player>() != null && !other.GetComponent<Player>().isPlayerInBush()) {
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
            this.agent.speed = this.playerAugmentedSpeed(farSpeed);
        } else {
            this.agent.speed = this.playerAugmentedSpeed(closeSpeed);
        }

        if(Mathf.Abs(this.transform.position.x - target.position.x) < .1f) {
            agent.SetDestination((target.position + new Vector3(.1f, 0f, 0f)));
        } else {
            agent.SetDestination(target.position);
        }

        if (agent.isStopped != !playerMoving) {
            agent.isStopped = !playerMoving;
        }

        selectSprite(agent.velocity);
    }

    public void navigateFromTarget(Transform target, bool playerMoving) {
        if((this.transform.position - target.position).magnitude < this.distanceToSpeedUp) { //move faster if closer to player
            this.agent.speed = this.farSpeed;
        } else {
            this.agent.speed = this.closeSpeed;
        }

        if (agent.isStopped != !playerMoving) {
            agent.isStopped = !playerMoving;
        }

        if(!agent.isStopped) {
            agent.velocity = playerAugmentedSpeed(this.agent.speed) * (this.transform.position - target.position).normalized;
        }

        selectSprite(agent.velocity);
    }

    public void resetPosition() {
        this.agent.enabled = false;
        this.transform.localPosition = startingPos;
        this.agent.enabled = true;
    }

    private float playerAugmentedSpeed(float maxSpeed) {
        return Mathf.Max(2, this.playerController.getSpeedPercentage() * maxSpeed);
    }
    
    private void selectSprite(Vector3 movingIn) {
        // float margin = .25f;
        // if(movingIn.x > margin) {
        //     rend.sprite = this.rightSprite;
        // } else if(movingIn.x < -margin) {
        //     rend.sprite = this.leftSprite;
        // } else if(movingIn.y > margin) {
        //     rend.sprite = this.upSprite;
        // } else if(movingIn.y < -margin) {
        //     rend.sprite = this.downSprite;
        // }
    }

    public void Update() {
        if(agent.isOnNavMesh && this.gameObject.activeInHierarchy && !this.agent.isStopped) {
            this.animator.SetBool("isMoving", true);
            float margin = .05f;

            if(agent.velocity.x > margin) {
                this.animator.SetInteger("direction", 3);
            } else if(agent.velocity.x < -margin) {
                this.animator.SetInteger("direction", 1);
            } else if(agent.velocity.y > margin) {
                this.animator.SetInteger("direction", 0);
            } else if(agent.velocity.y < -margin) {
                this.animator.SetInteger("direction", 2);
            } 
        } else {
            this.animator.SetBool("isMoving", false);
        }
    }
} 
