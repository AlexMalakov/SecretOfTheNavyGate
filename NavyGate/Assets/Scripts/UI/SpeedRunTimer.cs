using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedRunTimer : MonoBehaviour
{
    

    private bool hasMoved = false;
    [SerializeField] private TMP_Text text;
    private float startingTime;

    public void playerHasMoved() {
        hasMoved = true;
        this.startingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasMoved) {
            float time = Time.time - startingTime;
            text.text = FormatTime(time);
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        float seconds = time % 60f;
        return $"{minutes:00}:{seconds:00.00}";
    }
}
