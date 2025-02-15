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

    public Color happyColor;
    public Color mediumColor;
    public Color angryColor;

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

    public void updateScore(Car car, ScoreAction action) {
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
            deathAnimation.die();
        }
        else if (score >= maxScore)
        {
            score = maxScore;
        }

        updateUI();
    }

    private void updateUI()
    {
        float normalizedScore = (minScore + score) / (maxScore - minScore);
        slider.value = normalizedScore;

        if (normalizedScore < 1 / 3)
        {
            if (watcher.sprite != watcherAngry) {
                // watcher changed to angry sprite
                watcher.sprite = watcherAngry;
                sliderImage.color = Hex
            }
        }
        else if (normalizedScore < 2 / 3)
        {
            if (watcher.sprite != watcherMedium)
            {
                // watcher changed to angry sprite
                watcher.sprite = watcherMedium;
            }
        }
        else {
            if (watcher.sprite != watcherHappy) {
                // watcher changed to happy sprite
                watcher.sprite = watcherHappy;
            }
        }
    }
}
