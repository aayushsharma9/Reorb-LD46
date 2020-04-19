using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel, instructionsPanel;

    void Start()
    {

    }

    void Update()
    {

    }

    public void ToggleInstructions(bool on)
    {
        menuPanel.SetActive(!on);
        menuPanel.SetActive(on);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}