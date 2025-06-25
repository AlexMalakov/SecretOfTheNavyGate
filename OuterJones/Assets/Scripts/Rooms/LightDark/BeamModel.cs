using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamModel : MonoBehaviour
{
    
    private LineRenderer line;
    private bool active;

    public void Start() {
        gameObject.SetActive(false);
        active = false;
    }

    public void initBeam(Transform roomParent, Vector3 startingPos, Vector3 endingPos) {
        this.transform.parent = roomParent;
        active = true;
        gameObject.SetActive(true);

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, startingPos);
        line.SetPosition(1, endingPos);
    }

    public void killBeam() {
        gameObject.SetActive(false);
        active = false;
    }

    public bool isActive() {
        return this.active;
    }
}
