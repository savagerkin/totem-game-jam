using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    public void turnOnEndGamePanel()
    {
        
        endGamePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void RestartingGame()
    {
        Time.timeScale = 1; // Ensure the time scale is reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}