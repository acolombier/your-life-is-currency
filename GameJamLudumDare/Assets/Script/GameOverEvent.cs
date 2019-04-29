using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverEvent : MonoBehaviour
{

    public GameObject gameOverScreen;
    public GameObject RestartButton;

    public void Start()
    {
        EventManager.StartListening("game_over", Toggle);
        gameOverScreen.SetActive(false);
        RestartButton.SetActive(false);
    }

    public void Toggle()
    {
        Toggle(new object[] { });
    }

    public void Toggle(object[] args)
    {
        gameOverScreen.SetActive(true);
        RestartButton.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
