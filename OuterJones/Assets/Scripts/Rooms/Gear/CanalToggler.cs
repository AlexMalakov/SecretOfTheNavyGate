using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalToggler : MonoBehaviour
{

    //ok so basically here's the idea:
    //instead of creating and deleting canals, or creating a new type of canal
    //ill have 4 corner canals going into r4
    //there will be a canal connecting the corners traveling parallel to the room wall (4 canals)
    //2 of these are horiz, 2 are vertical
    //these canals have a dam that connects them to the corner they go between (8 dams)
    //this way when we toggle, we hide the canals that owuld be blocked by the dams
    //and leave the canals that are unblocked by the dams


    public List<Dam> horizDams;
    public List<Dam> vertDams;


    public List<GameObject> horizCanals;
    public List<GameObject> vertCanals;

    public void Start() {
        this.openVertical();
    }


    public void openHorizontal() {
        foreach(Dam d in horizDams) {
            d.closeDam();
        }

        foreach(Dam d in vertDams) {
            d.openDam();
        }

        foreach(Canal c in horizCanals) {
            c.hideCanal();
        }

        foreach(Canal c in vertCanals) {
            c.returnCanal();
        }
    }

    public void openVertical() {
        foreach(Dam d in vertDams) {
            d.closeDam();
        }

        foreach(Dam d in horizDams) {
            d.openDam();
        }

        foreach(Canal c in vertCanals) {
            c.hideCanal();
        }

        foreach(Canal c in horizCanals) {
            c.returnCanal();
        }
    }

}
