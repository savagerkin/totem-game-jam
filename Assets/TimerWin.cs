using TMPro;
using UnityEngine;

public class TimerWin : MonoBehaviour
{
    [SerializeField] private float timerDuration = 240f; // Set the timer duration in seconds
    private float timer;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject winPanel;
    
    void Start()
    {
        timer = timerDuration;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                win();
            }
        }

        timerText.text = timer.ToString("F2"); // Update the timer text with the current timer value
    }

    private void win()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }
}