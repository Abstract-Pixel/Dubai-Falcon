using CustomInspector;
using EditorAttributes;
using UnityEngine;
using ReadOnly = CustomInspector.ReadOnlyAttribute;

[RequireComponent(typeof(Rigidbody))]
public class AirMovement : MonoBehaviour
{
    [SelfFill][SerializeField] Rigidbody rb;

    [HorizontalLine("Steering Stats", 3, FixedColor.DustyBlue)]
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float rollSteeringSpeed = 5f;
    [SerializeField] float yawSteeringSpeed = 2f;
    [SerializeField] float rollStabilizationSpeed = 5f;

    [VInspector.Foldout("Steering Debug")]
    [HorizontalLine("Steering debug Stats", 3, FixedColor.CherryRed)]
    [ReadOnly][SerializeField] Quaternion targetRotation;
    [ReadOnly][SerializeField] Quaternion finalRotation;
    [ReadOnly][SerializeField] float horizontalInputValue;
    [ReadOnly][SerializeField] float dot;
    [VInspector.EndFoldout]

    [HorizontalLine("Speeding/ Vertical Control Settings", 3, FixedColor.DustyBlue)]
    [SerializeField] KeyCode divingInput;
    [SerializeField] KeyCode ascendInput;
    [SerializeField] float buildUpSpeed;
    [SerializeField] float maxAccelerationBuildUp;
    [SerializeField] float divingDivider;
    [SerializeField] float deacceleration;
    [SerializeField] float pitchRotationSpeed;

    [HorizontalLine("Clamp Settings", 3, FixedColor.DustyBlue)]
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;

    [HorizontalLine("Pitch Control debug", 3, FixedColor.DustyBlue)]
    [ReadOnly][SerializeField] float currentAccelerationBuildUp;
    [ReadOnly][SerializeField] Quaternion currentPitchRotation;
    [ReadOnly] float verticalInputValue;

    Vector3 initialForward;

    private void Start()
    {
        initialForward = transform.forward;
    }

    private void Update()
    {
        if (Input.GetKey(divingInput))
        {
            verticalInputValue =1;
            currentAccelerationBuildUp = Mathf.Lerp(currentAccelerationBuildUp, maxAccelerationBuildUp, Time.deltaTime*buildUpSpeed);
        }
        else if (Input.GetKey(ascendInput))
        {
            verticalInputValue=-1;
            currentAccelerationBuildUp = Mathf.Lerp(currentAccelerationBuildUp, 0, Time.deltaTime*deacceleration);
        }
        else
        {
            currentAccelerationBuildUp = Mathf.Lerp(currentAccelerationBuildUp, 0, Time.deltaTime*deacceleration);
            verticalInputValue =0;
        }
    }

    void FixedUpdate()
    {
        horizontalInputValue = Input.GetAxisRaw("Horizontal");
        Quaternion currentRotation = rb.rotation;

        #region Horizontal Steering
        if (Mathf.Abs(horizontalInputValue) >= 0.1f)
        {
            dot =Vector3.Dot(transform.forward, initialForward);// Check if we are oriented "backward-ish" to invert roll
            Vector3 currentRollDirection = transform.forward;
            float lerpSpeed = 2f;

            if (dot < 0)
            {
                currentRollDirection = Vector3.Lerp(transform.forward, -transform.forward, Time.fixedDeltaTime * lerpSpeed);
            }
            else if (dot > 0)
            {
                currentRollDirection = transform.forward;
            }

            float rollAngle = -horizontalInputValue * rollSteeringSpeed * Time.fixedDeltaTime; // Negate for natural control
            Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, currentRollDirection); // Roll around the forward axis

            float yawAngle = horizontalInputValue * yawSteeringSpeed * Time.fixedDeltaTime;
            Quaternion yawRotation = Quaternion.AngleAxis(yawAngle, Vector3.up); // Yaw around world up axis, not local up

            finalRotation = currentRotation *rollRotation* yawRotation;
        }
        #endregion
        #region Horizontal Stabilization Steering
        else
        {
            // Roll Stabilization when no horizontal input
            Vector3 currentEuler = currentRotation.eulerAngles;
            Vector3 targetEuler = new Vector3(currentEuler.x, currentEuler.y, 0f); // Keep X and Y, zero out Z (roll)
            targetRotation = Quaternion.Euler(targetEuler);
            float slerpT = rollStabilizationSpeed * Time.fixedDeltaTime;
            finalRotation = Quaternion.Slerp(currentRotation, targetRotation, slerpT);
        }
        #endregion
        #region Pitch Rotation Control

        float pitchAngle = verticalInputValue* pitchRotationSpeed * Time.fixedDeltaTime;
        currentPitchRotation = Quaternion.AngleAxis(pitchAngle, Vector3.right);
        finalRotation*= currentPitchRotation;

        Vector3 eulerRotation = finalRotation.eulerAngles;
        eulerRotation.x =ClampAngle(eulerRotation.x, minPitch, maxPitch);
        finalRotation.eulerAngles = eulerRotation;

        #endregion

        rb.MoveRotation(finalRotation); // Apply the rotation
        float velocity = forwardSpeed;
        if (verticalInputValue ==1|| verticalInputValue == 0)
        {
            velocity = forwardSpeed + currentAccelerationBuildUp/divingDivider;
        }
        else if (verticalInputValue ==-1)
        {
            velocity = forwardSpeed + currentAccelerationBuildUp;
        }
        rb.MovePosition(rb.position + transform.forward * velocity);
    }

    // Helper function to handle angle clamping correctly (0-360 and negative angles)
    private float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360f);
        min = Mathf.Repeat(min, 360f);
        max = Mathf.Repeat(max, 360f);
        bool reverse = false;
        if (min > max)
        {
            reverse = true;
            float temp = min;
            min = max;
            max = temp;
        }
        if (((angle > max) | (angle < min)) & !reverse)
        {
            float disMin = Mathf.Abs(angle - min);
            float disMax = Mathf.Abs(angle - max);
            if (disMin < disMax)
            {
                return min;
            }
            else
            {
                return max;
            }
        }
        if (((angle < max) & (angle > min)) & reverse)
        {
            float disMin = Mathf.Abs(angle - min);
            float disMax = Mathf.Abs(angle - max);
            if (disMin < disMax)
            {
                return min;
            }
            else
            {
                return max;
            }
        }
        return angle;
    }
}