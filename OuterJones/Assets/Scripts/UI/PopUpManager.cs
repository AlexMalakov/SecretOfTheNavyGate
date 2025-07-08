using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject spacePopUp;
    [SerializeField] private GameObject roomPopUp;


    public void displaySpacePopUp(Transform popUpPos, string message) {
        GameObject newP = Instantiate(this.spacePopUp, popUpPos);

        newP.transform.position = newP.transform.position + new Vector3(0f, -2.5f, 0f);
        newP.GetComponent<TMP_Text>().text = message;

        this.StartCoroutine(handleSpaceP(newP));
    }

    private IEnumerator handleSpaceP(GameObject popup) {
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


    public void displayRoomPopUp(Transform popUpPos, string roomName) {
        GameObject newP = Instantiate(this.roomPopUp, popUpPos);
        newP.GetComponent<TMP_Text>().text = roomName;

        this.StartCoroutine(handleRoomP(newP));
    }

    private IEnumerator handleRoomP(GameObject popup) {
        CanvasGroup canvasG = popup.GetComponent<CanvasGroup>();
        float duration = .5f;
        float elapsed = 0f;
        
        float ScaleTo = 2f;
        Vector3 initial = popup.transform.localScale;

        while(elapsed < duration) {
            popup.transform.localScale = Vector3.Lerp(initial, initial * ScaleTo, elapsed/duration);
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
}
