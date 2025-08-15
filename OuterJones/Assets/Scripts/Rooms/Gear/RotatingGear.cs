using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGear : RotationPuzzleElement, InputSubscriber
{
    [SerializeField] private List<GearTooth> teeth;
    [SerializeField] private float rotationAmount;
    [SerializeField] private Player player;

    [SerializeField] private Transform dropOffPoint;
    [SerializeField] private Transform alternateDropOff; //for when gear item reverses the direction
    [SerializeField] private bool oneWay;

    private PlayerIO input;
    private PlayerController controller;
    private bool isAlreadyRotating = false;

    private float ROTATION_DURATION = .5f;

    void Awake() {
        this.input = FindObjectOfType<PlayerIO>();
        this.controller = FindObjectOfType<PlayerController>();

        for(int i = 0; i < teeth.Count; i++) {
            this.teeth[i].init(this, i);
        }
    }

    public void playerOnTooth(GearTooth t) {
        if(this.playerInCanal || this.isAlreadyRotating) {
            return;
        }

        if(!oneWay || t.getID() != getClosestForOneWay().getID()) {
            this.input.requestSpaceInput(this, this.transform, "rotate gear");
        }
    }

    public void playerOffTooth() {
        this.input.cancelRequest(this);
    }

    public override void onPlayerInCanal() {
        base.onPlayerInCanal();
        this.input.cancelRequest(this);
    }

    public void onSpacePress() {
        StartCoroutine(this.rotateGear());
    }

    private IEnumerator rotateGear() {
        if(this.playerInCanal || (this.oneWay && this.getClosestToPlayer().getID() == this.getClosestForOneWay().getID())) {
            yield break;
        }
        this.isAlreadyRotating = true;

        //do that here to prveent gear item toggling from breaking anything
        GearTooth closest = null;
        if(oneWay) {
            closest = this.getClosestForOneWay();
        }

        this.controller.transform.parent = this.transform;
        this.controller.isMovementEnabled(false);

        Quaternion startRotation = transform.rotation;
        float rotAmount = player.getRotationDirection()? -rotationAmount : rotationAmount;

        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, rotAmount);

        float elapsed = 0f;
        while(elapsed < ROTATION_DURATION) {
            
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / ROTATION_DURATION);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if(oneWay && closest.getID() == this.getClosestToPlayer().getID()) {
            elapsed = 0f;
            Vector3 targetForClosest = this.player.getRotationDirection() ? this.dropOffPoint.transform.position : this.alternateDropOff.transform.position;
            float positionCorrectionTimer = .2f;
            Vector3 startPos = this.controller.transform.position;
            
            while(elapsed < positionCorrectionTimer) {
                this.controller.transform.position = Vector3.Lerp(startPos, targetForClosest, elapsed/positionCorrectionTimer);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        
        
        this.controller.transform.parent = null;
        this.controller.isMovementEnabled(true);

        //to chain once it's done
        //since closest is just the result of the method, if the closest point changes (can only happen from gear item)
        //then that means we can keep rotating
        if(!oneWay || (oneWay && closest.getID() != this.getClosestForOneWay().getID())) { 
            this.input.requestSpaceInput(this, this.transform, "rotate gear");
        }

        this.isAlreadyRotating = false;
    }

    private GearTooth getClosestForOneWay() {
        return getClosest(this.player.getRotationDirection() ? this.dropOffPoint.transform.position : this.alternateDropOff.transform.position);
    }

    private GearTooth getClosestToPlayer() {
        return getClosest(this.player.transform.position);
    }

    private GearTooth getClosest(Vector3 targetForClosest) {
        int closest = 0;
        float smallest = (this.teeth[0].transform.position - dropOffPoint.transform.position).magnitude;

        for(int i = 1; i < this.teeth.Count; i++) {
            if((this.teeth[i].transform.position - targetForClosest).magnitude < smallest) {
                closest = i;
                smallest = (this.teeth[i].transform.position - targetForClosest).magnitude;
            }
        }

        return this.teeth[closest];
    }

}
