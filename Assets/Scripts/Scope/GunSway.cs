using System;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [Header("Sway Position")]
    public float swayStep = 0.1f;
    public float maxSway = 0.06f;
    public float swayPositionSmooth = 5;

    [Header("Sway Rotation")]
    public float swayRotationStep = 8;
    public float maxRotationSway = 20;
    public float swayRotationSmooth = 5;

    private void Update()
    {
        Sway();    
    }

    /// <summary>
    /// The gun will lean (both with position and rotation) in the opposite direction the mouse is moving.
    /// </summary>
    private void Sway()
    {
        Vector2 mouseInput = new(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        // move gun in opposite direction compared to the current mouse movement
        Vector2 swayPos = mouseInput * -swayStep;
        swayPos.x = Mathf.Clamp(swayPos.x, -maxSway, maxSway);
        swayPos.y = Mathf.Clamp(swayPos.y, -maxSway, maxSway);

        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos, swayPositionSmooth * Time.deltaTime);

        Vector2 swayRotationVector = mouseInput * -swayRotationStep;
        swayRotationVector.x = Mathf.Clamp(swayRotationVector.x, -maxRotationSway, maxRotationSway);
        swayRotationVector.y = Mathf.Clamp(swayRotationVector.y, -maxRotationSway, maxRotationSway);

        // rotate gun in opposite direction compared to the current mouse movement
        // also turns the gun on frontal axis on horizontal mouse movement
        Quaternion swayRotation = Quaternion.Euler(swayRotationVector.y, swayRotationVector.x, swayRotationVector.x);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, swayRotation, swayRotationSmooth * Time.deltaTime);
    }
}
