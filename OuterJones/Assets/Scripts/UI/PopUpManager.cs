using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject spacePopUp;
    [SerializeField] private GameObject roomPopUp;


    public void displaySpacePopUp(Transform popUpPos) {
        GameObject newP = Instantiate(this.spacePopUp, popUpPos);

        this.StartCoroutine(displaySpacePopUp(newP));
    }

    private IEnumerator displaySpacePopUp(GameObject popup) {
        CanvasGroup canvasG = popup.GetComponent<CanvasGroup>();
        float duration = .5f;
        float elapsed = 0f;

        while(elapsed < duration) {
            elapsed += Time.deltaTime;
            yield return null;
        }

        duration = .5f;
        elapsed = 0f;
        while(elapsed < duration) {

            canvasG.alpha = 1f - elapsed/duration;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(popup);
    }


    public void displayRoomEnterPopUp(Transform popUpPos) {
        GameObject newP = Instantiate(this.roomPopUp, popUpPos);

        this.StartCoroutine(displaySpacePopUp(newP));
    }

    private IEnumerator displpayRoomPopUp(GameObject popup) {
        yield return null;
    }
}
