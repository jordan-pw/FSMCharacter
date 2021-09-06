using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    bool fixedRotation = false;

    [SerializeField]
    Transform focus = default;

    [SerializeField, Range(20f, 200f)]
    float distance = 5f;

    [SerializeField, Min(0f)]
    float focusRadius = 1f;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;

    [SerializeField, Range(1f, 360f)]
    float rotationSpeed = 90f;

    [SerializeField, Range(1f, 90f)]
    float rotationAngle = 45f;

    private Vector3 focusPoint;

    private Vector2 orbitAngles = new Vector2(45f, 0f);

    private bool goalReached = true;

    float goalAngle = 0f;

    private void Awake()
    {
        focusPoint = focus.position;
    }

    private void LateUpdate()
    {
        UpdateFocusPosition();
        ManualRotation();
        Quaternion lookRotation = Quaternion.Euler(orbitAngles);
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private void UpdateFocusPosition()
    {
        Vector3 targetPoint = focus.position;
        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
        {
            focusPoint = targetPoint;
        }
    }

    private void ManualRotation()
    {
        float input = InputHandler.rotateAxis;
        const float e = 0.001f;
        if (input < e || input > e)
        {
            float storedInput = input;
            if (fixedRotation)
            {
                if (goalReached)
                {
                    goalAngle = orbitAngles.y + rotationAngle * storedInput;
                    if (goalAngle != orbitAngles.y) goalReached = false;
                }
                if (goalAngle != orbitAngles.y)
                {
                    orbitAngles.y = Mathf.SmoothStep(orbitAngles.y, goalAngle, rotationSpeed * Time.unscaledDeltaTime);
                    if (goalAngle == orbitAngles.y) goalReached = true;
                }
                
            }
            else
            {
                orbitAngles.y += rotationSpeed * Time.unscaledDeltaTime * input;
            }
        }
    }
}
