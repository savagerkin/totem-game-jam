using UnityEngine;
using UnityEngine.UI;

public enum ScoreAction { 
    Shot,
    NoShot
}

public class ScoreSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pigOkay;
    [SerializeField] private GameObject pigQuestioning;
    [SerializeField] private GameObject pigAngry;

    public Color happyColor = Color.white;
    public Color mediumColor = Color.white;
    public Color angryColor = Color.white;

    public Image watcher;
    public CircularLoadingBar circularBar;

    public DeathAnimation deathAnimation;
    public SpeedLimitSign speedLimitSign;

    [Header("Settings")]
    public int[] speedLimits = new int[4];
    public float minScore;
    public float maxScore;
    public float correctShotReward;
    public float incorrectShotReward;
    public float noShotRewardScaler;

    [Header("Data")]
    public int correctlyShot;
    public int incorrectlyShot;
    public int notShot;
    public float score;

    private bool dead = false;

    private void Start()
    {
        updateUI();
    }

    public void Update()
    {
        for (int i = 0; i < speedLimits.Length; i++) {
            speedLimitSign.speedLimitTexts[i].text = speedLimits[i].ToString();
        }
    }

    public void updateScore(Car car, ScoreAction action) {
        if (dead) return;

        if (action == ScoreAction.Shot)
        {
            Debug.Log("stuff");
            Debug.Log(car.lane);
            Debug.Log(car.velocity);
            Debug.Log(speedLimits[car.lane]);
 

            if (speedLimits[car.lane] <= car.velocity)
            {
                correctlyShot += 1;
                score += correctShotReward;
            }
            else {
                incorrectlyShot += 1;
                score += incorrectShotReward;
            }
        } else if (action == ScoreAction.NoShot) {
            notShot += 1;
            score += noShotRewardScaler;
        }

        if (score <= minScore) {
            StartCoroutine(deathAnimation.die());
            dead = true;
        }
        else if (score >= maxScore)
        {
            score = maxScore;
        }

        updateUI();
    }

    private void updateUI()
    {
        float normalizedScore = (score - minScore) / (maxScore - minScore);
        circularBar.SetValue(normalizedScore);

        pigAngry.SetActive(false);
        pigQuestioning.SetActive(false);
        pigOkay.SetActive(false);

        if (normalizedScore < 1.0f / 3.0f)
        {
            pigAngry.SetActive(true);
            circularBar.SetFillColor(angryColor);
        }
        else if (normalizedScore < 2.0f / 3.0f)
        {
            pigQuestioning.SetActive(true);
            circularBar.SetFillColor(mediumColor);
        }
        else {
            pigOkay.SetActive(true);
            circularBar.SetFillColor(happyColor);
        }
    }
}
