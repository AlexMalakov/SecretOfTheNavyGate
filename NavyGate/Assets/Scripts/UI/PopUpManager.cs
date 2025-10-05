using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ScreenPopUpType {
    Endgame, Whip, Magnet, Grapple, Amulet, Torch, P1, W2, L3, R2
}

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
    }

    public void displayPopUpAlert(Transform popUpPos, string message) {
        if(this.popUpAlertMessage.activeSelf) {
            return;
        }

        this.placePopUpMessage(this.popUpAlertMessage, popUpPos);

        this.popUpAlertMessage.SetActive(true);
        this.popUpAlertMessage.GetComponentInChildren<TMP_Text>().text = message;

        StartCoroutine(fadeMessage(this.popUpAlertMessage));
    }

    public void displayPopUpMessage(Transform popUpPos, string message) {
        if(this.popUpMessage.activeSelf) {
            return;
        }

        this.placePopUpMessage(this.popUpMessage, popUpPos);

        this.popUpMessage.SetActive(true);
        this.popUpMessage.GetComponentInChildren<TMP_Text>().text = message;

        StartCoroutine(fadeMessage(this.popUpMessage));
    }

    private IEnumerator fadeMessage(GameObject popUp) {
        CanvasGroup canvasG = popUp.GetComponent<CanvasGroup>();
        canvasG.alpha = 1f;
        yield return new WaitForSeconds(1.5f);
        
        float elapsed = 0f;
        while(elapsed < 1f) {
            canvasG.alpha = 1f - elapsed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        popUp.SetActive(false);
    }

    public void displayScreenPopUp(Sprite screenPopUpImg) {
        if(this.finalPopUp.activeSelf) {
            return;
        }

        this.finalPopUp.GetComponent<Image>().sprite = screenPopUpImg;
        this.finalPopUp.SetActive(true);
    }

    public void endScreenPopUp() {
        this.finalPopUp.SetActive(false);
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
}