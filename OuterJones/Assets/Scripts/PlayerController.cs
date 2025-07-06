using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//could use input system but ive never bothered to learn it so for now it wont :)

public class PlayerController : MonoBehaviour 
{
    [SerializeField] private float moveSpeed =  12f;//5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;

    private bool movementEnabled = true;

    private bool isMoving = false;

    private void Start() { 
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if(movementEnabled) {
            rb.velocity = getDirection() * moveSpeed;
        } else {
            rb.velocity = Vector2.zero;
        }

        if(rb.velocity.x == 0 && rb.velocity.y == 0) {
            this.isMoving = false;
        } else {
            this.isMoving = true;
        }
    }

    private Vector2 getDirection() {
        Vector2 direction = Vector2.zero;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            direction+= Vector2.up;
        }

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            direction+= Vector2.down;
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            direction+= Vector2.left;
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            direction+= Vector2.right;
        }

        return direction;
    }

    public void isMovementEnabled(bool enabled) {
        this.movementEnabled = enabled;
    }

    public bool isPlayerMoving() {
        return this.isMoving;
    }
    
    //just in case a reset happens mid gear platform
    public void reset() {
        this.movementEnabled = true;
    }
}