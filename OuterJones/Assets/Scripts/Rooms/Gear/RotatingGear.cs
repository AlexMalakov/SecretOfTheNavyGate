using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGear : MonoBehaviour
{
    [SerializeField] private List<GearTooth> teeth;
    [SerializeField] private float rotationAmount;

    [SerializeField] private Transform dropOffPoint;

    private void Start() {
        for(int i = 0; i < teeth.Count; i++) {
            this.teeth[i].init(this, i);
        }
    }

    public void playerOnTooth(GearTooth t, PlayerController controller) {
        if(t.getID() != getClosest().getID()) {
            StartCoroutine(this.rotateGear(controller));
        }
    }

    private IEnumerator rotateGear(PlayerController controller) {

        controller.transform.parent = this.transform;
        controller.isMovementEnabled(false);

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, rotationAmount);

        float elapsed = 0f;
        while(elapsed < .5f) {
            
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / .5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        controller.transform.parent = null;
        controller.isMovementEnabled(true);

    }

    private GearTooth getClosest() {
        int closest = 0;
        float smallest = (this.teeth[0].transform.position - dropOffPoint.transform.position).magnitude;

        for(int i = 1; i < this.teeth.Count; i++) {
            if((this.teeth[i].transform.position - dropOffPoint.transform.position).magnitude < smallest) {
                closest = i;
                smallest = (this.teeth[i].transform.position - dropOffPoint.transform.position).magnitude;
            }
        }

        return this.teeth[closest];
    }

}
