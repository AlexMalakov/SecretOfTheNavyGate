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
    private Quaternion locRot;

    private bool isMoving = false;

    private void Start() { 
        rb = GetComponent<Rigidbody2D>();
        this.locRot = transform.rotation;
    }

    private void FixedUpdate() {
        getDirection();
        this.transform.rotation = this.locRot;

        if(movementEnabled) {
            rb.velocity = this.movementInput * moveSpeed;
        } else {
            rb.velocity = Vector2.zero;
        }

        if(rb.velocity.x == 0 && rb.velocity.y == 0) {
            this.isMoving = false;
        } else {
            this.isMoving = true;
        }
    }

    private void getDirection() {
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

        this.movementInput = direction;
    }

    public Vector3 getDirection3D() {
        return new Vector3(movementInput.x, movementInput.y, 0f);
    }

    public void isMovementEnabled(bool enabled) {
        this.movementEnabled = enabled;
    }

    public bool getIfMovementEnabled() {
        return this.movementEnabled;
    }

    public bool isPlayerMoving() {
        return this.isMoving;
    }
    
    public float getSpeedPercentage() {
        return this.rb.velocity.magnitude / this.moveSpeed;
    }

}