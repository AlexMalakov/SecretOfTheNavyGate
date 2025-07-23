using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingSpitter : MonoBehaviour
{
    [SerializeField] protected GameObject defaultSprite;
    [SerializeField] protected GameObject cwSprite;
    [SerializeField] protected GameObject ccwSprite;

    [SerializeField] protected float blinkDelay = 1f;

    private bool clockwise = true;

    private bool playerInRoom = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            this.activateAlternatingSpitter(other.GetComponent<PlayerController>());
        }
    }

    protected virtual void activateAlternatingSpitter(PlayerController controller) {
        StartCoroutine(rotateSpitter(controller));
    }

    protected IEnumerator rotateSpitter(PlayerController controller) {
        controller.isMovementEnabled(false);
        controller.transform.parent = this.transform;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, clockwise? 90 : -90);

        float elapsed = 0f;
        while(elapsed < .5f) {
            
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / .5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        clockwise = !clockwise;

        controller.transform.parent = null;
        controller.isMovementEnabled(true);
    }



    public void onPlayerEnter() {
        playerInRoom = true;
        this.blinkSprites();
    }

    public void onPlayerExit() {
        playerInRoom = false;
    }



    private void blinkSprites() {
        if(!playerInRoom) {
            return;
        }

        this.defaultSprite.SetActive(false);

        if(clockwise) {
            this.cwSprite.SetActive(true);
            this.ccwSprite.SetActive(false);
        } else {
            this.cwSprite.SetActive(false);
            this.ccwSprite.SetActive(true);
        }

        Invoke(nameof(unblinkSprites), this.blinkDelay);

    } 

    private void unblinkSprites() {
        if(!playerInRoom) {
            return;
        }

        this.defaultSprite.SetActive(true);
        this.cwSprite.SetActive(false);
        this.ccwSprite.SetActive(false);

        Invoke(nameof(blinkSprites), this.blinkDelay);
    }
}
