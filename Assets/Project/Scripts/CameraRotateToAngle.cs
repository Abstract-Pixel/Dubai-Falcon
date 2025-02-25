using System.Collections;
using UnityEngine;

public class CameraRotateToAngle : MonoBehaviour
{
    [Tooltip("Target rotation in Euler angles (e.g., 0, 90, 0)")]
    public Vector3 targetEulerAngles = new Vector3(0, 90, 0);

    [Tooltip("Time in seconds to complete the rotation")]
    public float duration = 1.0f;

    private bool isRotating = false;

    public void ButtonClicked()
    {
        if (!isRotating)
        {
            GameManger.instance.WinState();
            StartCoroutine(RotateToTarget());
        }
    }

    IEnumerator RotateToTarget()
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        isRotating = false;
    }
}