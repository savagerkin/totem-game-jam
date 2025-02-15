using System;
using UnityEngine;

public class Options : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject tutorialPanel;
    private Player player;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject howToPlayButton;

    public void Start()
    {
        player = gameObject.GetComponent<Player>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            turnOnOptions();
        }
    }

    public void turnOnOptions()
    {
        player.StopControls();
        Cursor.lockState = CursorLockMode.None;
        optionsPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void tunrOffOptions()
    {   
        player.StartControls();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        optionsPanel.SetActive(false);
    }
    
    public void TutorialOn()
    {
        resumeButton.SetActive(false);
        backButton.SetActive(true);
        tutorialPanel.SetActive(true);
        howToPlayButton.SetActive(false);
    }
    public void TutorialOff()
    {
        howToPlayButton.SetActive(true);
        resumeButton.SetActive(true);
        backButton.SetActive(false);
        tutorialPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    
}
