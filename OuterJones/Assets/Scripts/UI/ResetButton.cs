using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResetButton : MonoBehaviour
{
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
