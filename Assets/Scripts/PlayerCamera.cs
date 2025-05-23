using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public struct CameraInput
{
    public Vector2 Look;
}


public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float sensivity;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    private Vector3 _eulerAngles;
    [SerializeField] private TMP_Text sliderValue;
    private void Update()
    {
        sensivity = slider.value;
        sliderValue.text = "Sensitivity: " + slider.value;

    }

    public void Initialize(Transform target)
    {
        transform.position = target.position;
        transform.eulerAngles = _eulerAngles = target.eulerAngles;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }


    public void UpdateRotation(CameraInput input)
    {
        _eulerAngles += new Vector3(-input.Look.y, input.Look.x) * sensivity;
        _eulerAngles.x = Mathf.Clamp(_eulerAngles.x, -76f, 67f); // Clamp the x-axis rotation
        transform.eulerAngles = _eulerAngles;
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition += new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    public void AdjustableShake(float duration, float minMagnitude, float maxMagnitude)
    {
        StartCoroutine(AdjustableShakeCoroutine(duration, minMagnitude, maxMagnitude));
    }

    private IEnumerator AdjustableShakeCoroutine(float duration, float minMagnitude, float maxMagnitude)
    {
        float elapsed = 0.0f;
        Vector3 originalPosition = transform.localPosition;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float magnitude = Mathf.Lerp(minMagnitude, maxMagnitude, t);

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}