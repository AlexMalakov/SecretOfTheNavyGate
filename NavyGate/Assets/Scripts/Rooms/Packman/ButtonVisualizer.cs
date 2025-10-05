using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject pressedSprite;
    [SerializeField] private GameObject unpressedSprite;
    private bool mummyPressed = false;

    public void Awake() {
        this.setSprite();
    }

    public void onButtonPress() {
        if(mummyPressed) {
            return;
        }

        mummyPressed = true;
        this.setSprite();

        Invoke(nameof(fixSprite), 1f);
    }

    public void fixSprite() {
        this.mummyPressed = false;
        this.setSprite();
    }

    private void setSprite() {
        this.unpressedSprite.SetActive(!this.mummyPressed);
        this.pressedSprite.SetActive(this.mummyPressed);
    }


}
