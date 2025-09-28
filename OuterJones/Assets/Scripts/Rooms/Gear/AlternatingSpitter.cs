using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingSpitter : RotationPuzzleElement, InputSubscriber, ItemListener
{
    [SerializeField] private Player player;

    [SerializeField] private GameObject defaultSprite;
    [SerializeField] private GameObject cwSprite;
    [SerializeField] private GameObject ccwSprite;

    [SerializeField] private float blinkDelay = 1f;

    [SerializeField] private AlternatingSpitterListener listener;

    [SerializeField] private List<Transform> rotationPoints;

    private float ROTATION_DURATION = .25f;

    [SerializeField] private bool clockwise = true;
    private bool startDirection;
    private bool rotatingPlayer = false;

    private PlayerIO input;
    private PlayerController controller;

    protected void Awake() {
        this.input = FindObjectOfType<PlayerIO>();
        this.controller = this.player.gameObject.GetComponent<PlayerController>();
        FindObjectOfType<Inventory>().addItemListener(PossibleItems.GearItem, this);

        this.startDirection = this.clockwise;
    }

    private bool playerInRoom = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(this.playerInCanal) {
            return;
        }
        if(other.GetComponent<PlayerController>() != null) {
            this.input.requestSpaceInput(this, this.transform, "rotate spinner");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            this.input.cancelInputRequest(this);
        }
    }

    public override void onPlayerInCanal() {
        base.onPlayerInCanal();
        this.input.cancelInputRequest(this);
    }

    public override void resetElement() {
        base.resetElement();
        this.clockwise = startDirection;
    }

    public void onSpacePress() {
        StartCoroutine(rotateSpitter());
    }

    protected IEnumerator rotateSpitter() {
        if(this.playerInCanal || this.rotatingPlayer) {
            yield break;
        }

        this.rotatingPlayer = true;
        this.controller.isMovementEnabled(false);
        this.controller.transform.parent = this.transform;

        float elapsed = 0f;
        float positionCorrectionTimer = .1f;
        Vector3 startPos = this.controller.transform.position;
        Vector3 endPos = this.rotationPoints[0].position;
        foreach(Transform t in this.rotationPoints) {
            if((t.position - this.controller.transform.position).magnitude < (endPos - this.controller.transform.position).magnitude) {
                endPos = t.position;
            }
        }
        
        while(elapsed < positionCorrectionTimer) {
            this.controller.transform.position = Vector3.Lerp(startPos, endPos, elapsed/positionCorrectionTimer);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Quaternion startRotation = transform.rotation;

        //if playerDirection = true, then clockwise.     if !playerDirection, then !clockwise
        bool direction = !(clockwise ^ this.player.getRotationDirection());

        if(this.listener != null) {
            this.listener.onSpin(direction);
        }

        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, direction? -90 : 90);

        elapsed = 0f;
        while(elapsed < ROTATION_DURATION) {
            
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / ROTATION_DURATION);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(.1f);
        transform.rotation = endRotation; //helps with float impression

        clockwise = !clockwise;
        this.checkSprite();
        this.rotatingPlayer = false;

        this.controller.transform.parent = null;
        this.controller.isMovementEnabled(true);

        //to chain once it's done
        this.input.requestSpaceInput(this, this.transform, "rotate spinner");
    }


    public void onPlayerEnter() {
        playerInRoom = true;
        this.blinkSprites();
    }

    public void onPlayerExit() {
        playerInRoom = false;
    }



    protected void blinkSprites() {
        if(!playerInRoom) {
            return;
        }


        this.defaultSprite.SetActive(false);

        if((this.clockwise && this.player.getRotationDirection()) || (!this.clockwise && ! this.player.getRotationDirection())) {
            this.cwSprite.SetActive(true);
            this.ccwSprite.SetActive(false);
        } else {
            this.cwSprite.SetActive(false);
            this.ccwSprite.SetActive(true);
        }

        Invoke(nameof(unblinkSprites), this.blinkDelay);

    } 

    private void checkSprite() {
        bool shouldBeCW = (this.clockwise && this.player.getRotationDirection()) || (!this.clockwise && !this.player.getRotationDirection());
        if(this.cwSprite.activeInHierarchy && !shouldBeCW) {
            this.cwSprite.SetActive(false);
            this.ccwSprite.SetActive(true);
        } else if(this.ccwSprite.activeInHierarchy && shouldBeCW) {
            this.cwSprite.SetActive(true);
            this.ccwSprite.SetActive(false);
        }
    }

    private void unblinkSprites() {
        if(!playerInRoom) {
            return;
        }

        defaultSprite.SetActive(true);

        this.cwSprite.SetActive(false);
        this.ccwSprite.SetActive(false);

        Invoke(nameof(blinkSprites), this.blinkDelay);
    }

    public void onItemEvent(bool status) {
        this.checkSprite();
    }
}
