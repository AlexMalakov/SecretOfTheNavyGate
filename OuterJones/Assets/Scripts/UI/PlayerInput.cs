using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PopUpManager popUpManager;
    private static PopUpManager manager;

    public void Awake() {
        manager = popUpManager;
    }

    public static bool getSpaceInput(Transform objRequesting, string message) {
        if(Input.GetKey(KeyCode.Space)) {

            manager.displaySpacePopUp(objRequesting, message);

            return true;
        }
        return false;
    }
}
