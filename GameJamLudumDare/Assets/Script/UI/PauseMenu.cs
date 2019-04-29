using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject ui;
    public GameObject toolTip;

    public void Start()
    {
        EventManager.StartListening("trigger_pause", Toggle);
    }

    public void Toggle()
    {
        Toggle(new object[] { });
    }

    public void Toggle(object[] args )
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
        Toggle(new object[] { });
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
