using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject MainUI, ControlsUI, ControlsButton;
    bool controlsActive = false;

    void Start()
    {
        MainUI = GameObject.Find("MainUI");
        ControlsUI = GameObject.Find("ControlsUI");
        ControlsButton = GameObject.Find("Controls Button");
        ControlsButton.GetComponent<Button>().onClick.AddListener(ShowControls);
        ShowMain();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && controlsActive)
        {
            ShowMain();
        }
    }

    public void ShowMain()
    {
        MainUI.SetActive(true);
        ControlsUI.SetActive(false);
        controlsActive = false;
    }

    public void ShowControls()
    {
        MainUI.SetActive(false);
        ControlsUI.SetActive(true);
        controlsActive = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
