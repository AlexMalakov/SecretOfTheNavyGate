using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationListener : MonoBehaviour
{
    private int totalRotations = 0;
    private bool direction = true;
    [SerializeField] private List<GameObject> rotationSprites;
    [SerializeField] private GameObject effectableTarget;
    [SerializeField] private CounterObjListener rotListener;

    private Quaternion initialRot;

    public void Awake() {
        this.initialRot = this.transform.rotation;
    }

    public void onRotation(bool clockwise) {
        this.transform.rotation = this.initialRot;

        if(clockwise == !this.direction) {
            this.direction = clockwise;

            if(this.totalRotations <= rotationSprites.Count - 1) {
                rotationSprites[this.totalRotations].SetActive(false);
            } else {
                rotationSprites[rotationSprites.Count - 1].SetActive(false);
            }
            
            this.totalRotations = 1;
        } else {
            this.totalRotations++;
        }

        if(this.totalRotations <= rotationSprites.Count - 1) {
            rotationSprites[this.totalRotations-1].SetActive(false);
            rotationSprites[this.totalRotations].SetActive(true);
        }

        if(totalRotations == rotationSprites.Count-1) {
            effectableTarget.GetComponent<Effectable>().onEffect();
        }

        this.rotListener.activateNumber((int)(Mathf.Min(this.totalRotations, this.rotationSprites.Count - 1)));
    }
}
