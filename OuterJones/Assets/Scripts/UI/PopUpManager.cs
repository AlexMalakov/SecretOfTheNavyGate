using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject spacePopUp;
    [SerializeField] private GameObject popUpMessage;
    [SerializeField] private GameObject popUpAlertMessage;
    [SerializeField] private GameObject roomPopUp;
    [SerializeField] private GameObject finalPopUp;
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Transform popUpBottomBorder;
    [SerializeField] private Transform popUpLeftBorder;
    [SerializeField] private Transform popUpRightBorder;

    private Coroutine roomEnterCoroutine;
    private Vector3 resetRoomPopUpSize;

    public void Start() {
        this.resetRoomPopUpSize = roomPopUp.transform.localScale;
    }

    public void displaySpacePopUp(Transform popUpPos, string message) {
        if(this.spacePopUp.activeSelf){
            return;
        }

        this.placePopUpMessage(this.spacePopUp, popUpPos);

        this.spacePopUp.SetActive(true);
        this.spacePopUp.GetComponentInChildren<TMP_Text>().text = message;
    }

    public void endSpacePopUp() {
        this.spacePopUp.SetActive(false);
        this.popUpMessage.SetActive(false);
        this.popUpAlertMessage.SetActive(false);
        this.finalPopUp.SetActive(false);
    }

    public void displayPopUpAlert(Transform popUpPos, string message) {
        if(this.popUpAlertMessage.activeSelf) {
            return;
        }

        this.placePopUpMessage(this.popUpAlertMessage, popUpPos);

        this.popUpMessage.SetActive(true);
        this.popUpMessage.GetComponentInChildren<TMP_Text>().text = message;
    }

    public void displayPopUpMessage(Transform popUpPos, string message) {
        if(this.popUpMessage.activeSelf) {
            return;
        }

        this.placePopUpMessage(this.popUpMessage, popUpPos);

        this.popUpMessage.SetActive(true);
        this.popUpMessage.GetComponentInChildren<TMP_Text>().text = message;
    }

    public void displayEndGamePopUp(Transform popUpPos) {
        if(this.finalPopUp.activeSelf) {
            return;
        }

        this.placePopUpMessage(this.finalPopUp, popUpPos);

        this.finalPopUp.SetActive(true);
    }

    private void placePopUpMessage(GameObject popUp, Transform popUpPos) {

        Vector3 yOffset = (popUpPos.position.y > this.popUpBottomBorder.position.y) ? new Vector3(0f, -4f, 0f) : new Vector3(0f, 4f, 0f);

        Vector3 xOffset = Vector3.zero;
        if(popUpPos.position.x < this.popUpLeftBorder.position.x) {
            xOffset = new Vector3(4f, 0f, 0f);
        } else if(this.popUpLeftBorder.position.x > this.popUpRightBorder.position.x) {
            xOffset = new Vector3(-4f, 0f, 0f);
        }

        Vector3 screenPos = cam.WorldToScreenPoint(popUpPos.position + xOffset + yOffset);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
            out localPoint
        );

        popUp.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    public void displayRoomPopUp(string roomName) {
        if(this.roomPopUp.activeSelf && this.roomEnterCoroutine != null) {
            StopCoroutine(this.roomEnterCoroutine);
        }

        this.roomPopUp.SetActive(true);
        this.roomPopUp.GetComponentInChildren<TMP_Text>().text = roomName;

        this.roomEnterCoroutine = this.StartCoroutine(handleRoomP(this.roomPopUp));
    }

    private IEnumerator handleRoomP(GameObject popup) {
        CanvasGroup canvasG = popup.GetComponent<CanvasGroup>();
        canvasG.alpha = 1f;
        float duration = 3.5f;
        float elapsed = 0f;

        popup.transform.localScale = this.resetRoomPopUpSize;
        
        Vector3 initial = popup.transform.localScale/4f;
        Vector3 target = popup.transform.localScale * 1.5f;

        while(elapsed < duration) {
            popup.transform.localScale = Vector3.Lerp(initial, target, elapsed/duration);

            if(elapsed > 2f) {
                canvasG.alpha = 1f - (elapsed - 2f)/(3.5f - 2f);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // duration = 1.5f;
        // elapsed = 0f;
        // while(elapsed < duration) {

        //     canvasG.alpha = 1f - elapsed/duration;
        //     elapsed += Time.deltaTime;
        //     yield return null;
        // }

        popup.transform.localScale = this.resetRoomPopUpSize;
        canvasG.alpha = 1f;
        popup.SetActive(false);
    }
}