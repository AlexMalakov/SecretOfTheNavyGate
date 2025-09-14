using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamModel : MonoBehaviour
{
    
    private LineRenderer line;
    private bool active;
    private DoorDirection? start;
    private DoorDirection? end;
    public int beamID;

    public void Start() {
        gameObject.SetActive(false);
        active = false;
    }

    public void initBeam(Transform roomParent, Vector3 startingPos, Vector3 endingPos, DoorDirection? start, DoorDirection? end) {
        Debug.Log("INITING BEAM " + beamID + " FOR " + roomParent.gameObject.name);
        this.transform.parent = roomParent;
        active = true;
        gameObject.SetActive(true);

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, startingPos);
        line.SetPosition(1, endingPos);
    }

    public void claimBeam(Transform parent) {
        Debug.Log("CLAIMING BEAM " + beamID + " FOR " + parent.gameObject.name);
        this.transform.parent = parent;
        this.active = true;
    }

    public void killBeam() {
        Debug.Log("CLAIMING BEAM " + beamID + " FOR " + this.transform.parent.gameObject.name);
        gameObject.SetActive(false);
        active = false;
    }

    public bool isActive() {
        return this.active;
    }

    public bool sameBeam(DoorDirection? start, DoorDirection? end) {
        return start == this.start && end == this.end;
    }
}
