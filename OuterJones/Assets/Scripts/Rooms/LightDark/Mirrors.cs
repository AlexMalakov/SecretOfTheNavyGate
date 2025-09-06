using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirrors : MonoBehaviour
{
    [SerializeField] List<Mirror> topMirrors;
    [SerializeField] List<Mirror> botMirrors;
    [SerializeField] private DoorDirection topMirrorsDirection;
    [SerializeField] private DoorDirection topMirrorsReflectTo;
    private List<BeamModel> beams = new List<BeamModel>();


    public Transform getStartingPoint(DoorDirection enter) { //this may not work
        if(enter == this.topMirrorsDirection) {
            return this.topMirrors[0].transform;
        } else if(enter == this.topMirrorsReflectTo) {
            return this.topMirrors[this.topMirrors.Count-1].transform;
        } else if(enter == Door.rotateDoorDirection(Door.rotateDoorDirection(this.topMirrorsDirection, true), true)) {
            return this.botMirrors[0].transform;
        } else {
            return this.botMirrors[this.botMirrors.Count-1].transform;
        }
    }

    public Transform getEndingPoint(DoorDirection enter) { //this may not work
        if(enter == this.topMirrorsDirection) {
            return this.topMirrors[this.topMirrors.Count-1].transform;
        } else if(enter == this.topMirrorsReflectTo) {
            return this.topMirrors[0].transform;
        } else if(enter == Door.rotateDoorDirection(Door.rotateDoorDirection(this.topMirrorsDirection, true), true)) {
            return this.botMirrors[this.botMirrors.Count-1].transform;
        } else {
            return this.botMirrors[0].transform;
        }
    }

    public bool hasCobWebs(DoorDirection enter) {
        List<Mirror> listToCheck = this.isTopMirrors(enter) ? this.topMirrors : this.botMirrors;
        foreach(Mirror m in listToCheck) {
            if(m.hasCobWebs()) {
                return true;
            }
        }
        return false;
    }

    public DoorDirection reflect(DoorDirection enter) {
        List<Mirror> listToUse = isTopMirrors(enter) ? this.topMirrors : this.botMirrors;
        if(this.beams.Count > 0) {
            this.resetMirrorBeams();
        }


        bool startsFromZero = this.mirrorsStartsFromZero(enter);
        int start = startsFromZero ? 0 : listToUse.Count - 1;
        int end = startsFromZero ? listToUse.Count - 1 : 0;
        int next = startsFromZero ? 1 : -1;

        

        for(int i = start; (i < listToUse.Count-1) || i >= 1; i = i + next) {
            if(listToUse[start].hasCobWebs())
                return enter;

            BeamModel b = BeamPool.getBeam();
            b.initBeam(
                    this.transform,
                    listToUse[i+next].transform.position,
                    listToUse[i].transform.position,
                    null,
                    null);

            this.beams.Add(b);

        }
        return this.getExitDirection(enter);
    }

    private DoorDirection getExitDirection(DoorDirection enter) {
        if(enter == this.topMirrorsDirection) {
            return this.topMirrorsReflectTo;
        } else if(enter == this.topMirrorsReflectTo) {
            return this.topMirrorsDirection;
        } else if(enter == Door.rotateDoorDirection(Door.rotateDoorDirection(this.topMirrorsDirection, true), true)) {
            return Door.rotateDoorDirection(Door.rotateDoorDirection(this.topMirrorsReflectTo, true), true);
        }
        return Door.rotateDoorDirection(Door.rotateDoorDirection(this.topMirrorsDirection, true), true);
    }

    public void resetMirrorBeams() {
        foreach(BeamModel b in this.beams) {
            b.killBeam();
        }

        this.beams = this.beams = new List<BeamModel>();
    }

    public void rotate90(bool clockwise) {
        this.topMirrorsDirection = Door.rotateDoorDirection(this.topMirrorsDirection, clockwise);
        this.topMirrorsReflectTo = Door.rotateDoorDirection(this.topMirrorsReflectTo, clockwise);
    }

    private bool isTopMirrors(DoorDirection enter) {
        return enter == this.topMirrorsDirection || enter == this.topMirrorsReflectTo;
    }

    private bool mirrorsStartsFromZero(DoorDirection enter) {
        return enter == this.topMirrorsDirection || enter == Door.rotateDoorDirection(Door.rotateDoorDirection(this.topMirrorsDirection, true), true);
    }
}
