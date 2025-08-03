using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spins R1, then spins the room that is pointed to by what was the south door
public class RoomSpinner : MonoBehaviour
{
    [SerializeField] private GearRoom room;
    [SerializeField] private Door spinThrough;

    private int rotationCounter = 0;

    [SerializeField] private GameObject effectableStaircasesObj;

    [Header ("countdown sprites")]
    [SerializeField] private GameObject threeLeft;
    [SerializeField] private GameObject twoLeft;
    [SerializeField] private GameObject oneLeft;
    [SerializeField] private GameObject zeroLeft;


    public void onActivate(bool clockwise) {

        if(room.getLayoutManager().getRoomAt(room.getPosition().getOffset(spinThrough.getDirection())) != null) {
            room.getLayoutManager().getRoomAt(room.getPosition().getOffset(spinThrough.getDirection())).rotate90(!clockwise);
        }

        room.rotate90(clockwise);
        this.rotationCounter++;

        this.updateSprite();

        if(this.rotationCounter == 3) {
            effectableStaircasesObj.GetComponent<Effectable>().onEffect();
        }
    }

    private void updateSprite() {
        this.threeLeft.SetActive(this.rotationCounter == 0);
        this.twoLeft.SetActive(this.rotationCounter == 1);
        this.oneLeft.SetActive(this.rotationCounter == 2);
        this.zeroLeft.SetActive(this.rotationCounter >= 3);
    }
}
