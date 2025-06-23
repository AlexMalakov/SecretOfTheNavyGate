using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamModel : MonoBehaviour
{
    
    private LineRenderer line;

    public void initBeam(Vector3 startingPos, Vector3 endingPos) {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, startingPos);
        line.SetPosition(1, endingPos);
    }
}
