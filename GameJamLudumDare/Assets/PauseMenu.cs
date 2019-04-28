using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject ui;
    public GameObject toolTip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }        
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            toolTip.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            toolTip.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        Toggle();
        EditorSceneManager.LoadScene(EditorSceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {

    }
}
