using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private DoorDirection northReflectsTo;
    [SerializeField] private GameObject regSprite;
    [SerializeField] private GameObject webbedSprite;
    [SerializeField] private bool isWebbed = false;

    void Start() {
        if(isWebbed) {
            webbedSprite.SetActive(true);
            regSprite.SetActive(true);
        } else {
            webbedSprite.SetActive(false);
            regSprite.SetActive(false);
        }
    }

    public DoorDirection reflect(DoorDirection enter) {
        if(enter == DoorDirection.North) {
            return northReflectsTo;
        } else if(enter == DoorDirection.South) {
            if(northReflectsTo == DoorDirection.East) {
                return DoorDirection.West;
            }
            return DoorDirection.East;
        } else if(enter == northReflectsTo) {
            return DoorDirection.North;
        }
        return DoorDirection.South;
    }

    public void rotate90() {
        if(northReflectsTo == DoorDirection.West) {
            northReflectsTo = DoorDirection.East;
        } else {
            northReflectsTo = DoorDirection.West;
        }
    }

    public bool hasCobWebs() {
        return this.isWebbed;
    }

    public void clearWebs() {
        if(this.isWebbed) {
            this.isWebbed = false;
            this.regSprite.SetActive(true);
            this.webbedSprite.SetActive(false);

            FindObjectOfType<RoomsLayout>().notifyRoomListeners(new List<Room>());
        }
    }
}
