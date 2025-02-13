using CustomInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirMovement : MonoBehaviour
{
    [SelfFill][SerializeField] Rigidbody rb;

    [HorizontalLine("Movement Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float rollSteeringSpeed = 5f;
    [SerializeField] float yawSteeringSpeed = 2f;
    [SerializeField] float rollStabilizationSpeed = 5f;

    [HorizontalLine("Debug Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float horizontalXInput;

    [SerializeField] Quaternion targetRotation;
    [SerializeField] Quaternion slerpedRotation;
    [SerializeField] float dot;

    Vector3 initialForward;

    private void Start()
    {
        initialForward = transform.forward;
    }

    void FixedUpdate()
    {
        horizontalXInput = Input.GetAxisRaw("Horizontal");

        HandleSteeringAndStabilization();
        HandleForwardMovement();

        // Debug Rays - let's keep these to visualize what's happening
        Debug.DrawRay(transform.position, transform.forward * forwardSpeed, Color.green);
        Debug.DrawRay(transform.position, transform.right * forwardSpeed, Color.red);
        Debug.DrawRay(transform.position, transform.up * forwardSpeed, Color.blue);
    }

    void HandleSteeringAndStabilization()
    {
        Quaternion currentRotation = rb.rotation;
        Quaternion rollRotation = Quaternion.identity;
        Quaternion yawRotation = Quaternion.identity;

        if (Mathf.Abs(horizontalXInput) >= 0.1f)
        {
            // Steering based on input
           

            dot =Vector3.Dot(transform.forward, initialForward) ;// Check if we are oriented "backward-ish" to invert roll
            Vector3 currentRolldirection = transform.forward;
            float lerpSpeed = 2f; // Adjustable speed for the Lerp - you can tweak this

            if (dot < 0)
            {
                currentRolldirection = Vector3.Lerp(transform.forward, -transform.forward, Time.fixedDeltaTime * lerpSpeed);
            }
            else if (dot > 0)
            {
                // Let's simplify this for now and focus on the negative dot case
                // If you need special behavior for positive dot, we can add it back later, tell me what you want to achieve.
                currentRolldirection = transform.forward; // For positive dot, just use forward direction for now.
                // If you *do* want to Lerp for positive dot, let me know what you want to Lerp *to* and *why*.

                // Original code for positive dot (if you need to revisit it, but let's simplify for now):
                // Vector3 absoluteForward = new Vector3(Mathf.Abs(transform.forward.x), Mathf.Abs(transform.forward.y), Mathf.Abs(transform.forward.z));
                // currentRolldirection = Vector3.Lerp(transform.forward,absoluteForward, Time.fixedDeltaTime * lerpSpeed);
            }

            float rollAngle = -horizontalXInput * rollSteeringSpeed * Time.fixedDeltaTime; // Negate for natural control
            rollRotation = Quaternion.AngleAxis(rollAngle,currentRolldirection); // Roll around the forward axis

            float yawAngle = horizontalXInput * yawSteeringSpeed * Time.fixedDeltaTime;
            yawRotation = Quaternion.AngleAxis(yawAngle,Vector3.up); // Yaw around world up axis, not local up
            slerpedRotation = currentRotation *rollRotation* yawRotation;
        }
        else
        {
            // Roll Stabilization when no horizontal input
            Vector3 currentEuler = currentRotation.eulerAngles;
            Vector3 targetEuler = new Vector3(currentEuler.x, currentEuler.y, 0f); // Keep X and Y, zero out Z (roll)
            targetRotation = Quaternion.Euler(targetEuler);
            float slerpT = rollStabilizationSpeed * Time.fixedDeltaTime;
            slerpedRotation = Quaternion.Slerp(currentRotation, targetRotation, slerpT);
        }
        rb.MoveRotation(slerpedRotation); // Apply the rotation
    }


    void HandleForwardMovement()
    {
        // Move forward always in the plane's forward direction
        Vector3 moveDirection = (transform.forward);
        rb.MovePosition(rb.position + moveDirection * forwardSpeed); // Use MovePosition for kinematic movement
    }
}