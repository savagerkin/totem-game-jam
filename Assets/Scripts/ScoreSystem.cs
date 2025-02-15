using UnityEngine;
using UnityEngine.UI;

public enum ScoreAction { 
    Shot,
    NoShot
}

public class ScoreSystem : MonoBehaviour
{
    [Header("References")]
    public Sprite watcherHappy;
    public Sprite watcherMedium;
    public Sprite watcherAngry;

    public Color happyColor = Color.white;
    public Color mediumColor = Color.white;
    public Color angryColor = Color.white;

    public Image watcher;
    public Slider slider;
    public Image sliderImage;

    public DeathAnimation deathAnimation;

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

    public void updateScore(Car car, ScoreAction action) {
        if (dead) return;

        if (action == ScoreAction.Shot)
        {
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
            score += noShotRewardScaler * car.noShotReward;
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
        slider.value = normalizedScore;

        Debug.Log(normalizedScore);

        if (normalizedScore < 1.0f / 3.0f)
        {
            watcher.sprite = watcherAngry;
            sliderImage.color = angryColor;
        }
        else if (normalizedScore < 2.0f / 3.0f)
        {
            watcher.sprite = watcherMedium;
            sliderImage.color = mediumColor;
        }
        else {
            watcher.sprite = watcherHappy;
            sliderImage.color = happyColor;
        }
    }
}
