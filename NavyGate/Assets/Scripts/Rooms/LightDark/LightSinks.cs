using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSinks : MonoBehaviour
{
    [SerializeField] List<LightSink> lightSinks;



    public bool canActivateSink(DoorDirection incoming) {
        foreach(LightSink s in this.lightSinks) {
            if(s.getIncomingDirectionToActivate() == incoming) {
                return true;
            }
        }

        return false;
    }

    public LightSink activateSink(DoorDirection incoming) {
        foreach(LightSink s in this.lightSinks) {
            if(s.getIncomingDirectionToActivate() == incoming) {
                s.activate(incoming);
                return s;
            }
        }

        return null;
    }

    public void deactivateAll() {
        foreach(LightSink s in this.lightSinks) {
            s.deactivate();
        }
    }

    public void rotate90(bool clockwise) {
        foreach(LightSink s in this.lightSinks) {
            s.rotate90(clockwise);
        }
    }
}
