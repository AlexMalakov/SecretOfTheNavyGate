using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyStatue : MonoBehaviour
{

    private bool state = false;
    [SerializeField] GameObject onModel;
    [SerializeField] GameObject offModel;


    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.toggleState();
        }
    }


    public void reset() {
        this.state = false;
        this.offModel.SetActive(true);
        this.onModel.SetActive(false);
    }

    private void toggleState() {
        this.state = !this.state;
        this.offModel.SetActive(!this.state);
        this.onModel.SetActive(this.state);
    }
}
