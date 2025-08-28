using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private GameObject regSprite;
    [SerializeField] private GameObject webbedSprite;
    [SerializeField] private bool isWebbed = false;

    void Awake() {
        if(isWebbed) {
            webbedSprite.SetActive(true);
            regSprite.SetActive(true);
        } else {
            webbedSprite.SetActive(false);
            regSprite.SetActive(false);
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
