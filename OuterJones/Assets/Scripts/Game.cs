using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] RoomsLayout layout;
    [SerializeField] Player player;

    public void resetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


public interface Effectable {
    void onEffect();
    void onEffectOver();
}