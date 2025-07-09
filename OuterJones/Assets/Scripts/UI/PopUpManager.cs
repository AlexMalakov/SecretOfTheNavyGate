using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject spacePopUp;
    [SerializeField] private GameObject roomPopUp;
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas canvas;

    public void displaySpacePopUp(Transform popUpPos, string message) {
        if(this.spacePopUp.activeSelf){
            return;
        }

        this.placeSpacePopUp(popUpPos);

        this.spacePopUp.SetActive(true);
        this.spacePopUp.GetComponentInChildren<TMP_Text>().text = message;
    }

    public void endSpacePopUp() {
        this.spacePopUp.SetActive(false);
    }

    private void placeSpacePopUp(Transform popUpPos) {
        Vector3 screenPos = cam.WorldToScreenPoint(popUpPos.position + new Vector3(0f, -4f, 0f));

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
            out localPoint
        );

        this.spacePopUp.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    public void displayRoomPopUp(string roomName) {
        if(this.roomPopUp.activeSelf) {
            return;
        }

        this.roomPopUp.SetActive(true);
        this.roomPopUp.GetComponentInChildren<TMP_Text>().text = roomName;

        this.StartCoroutine(handleRoomP(this.roomPopUp));
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

        popup.SetActive(false);
    }
}