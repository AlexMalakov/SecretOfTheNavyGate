using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResetButton : MonoBehaviour
{
    //im a bozo and wrote my own script called "button"
    [SerializeField] UnityEngine.UI.Button button;
    [SerializeField] Game game;

    public void Start() {
        UnityEngine.UI.Button btn = button.GetComponent<UnityEngine.UI.Button>();
		btn.onClick.AddListener(onButtonClick);
    }

    public void onButtonClick() {
        this.game.resetGame();
    }
}
