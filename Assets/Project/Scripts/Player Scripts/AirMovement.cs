using CustomInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirMovement : MonoBehaviour
{
    [SelfFill][SerializeField] Rigidbody rb;

    [HorizontalLine("Movement Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float steeringSpeed = 5f; // General steering (pitch and roll) - might not be used directly anymore
    [SerializeField] float rollSteeringSpeed = 5f; // Existing roll steering speed
    [SerializeField] float yawSteeringSpeed = 2f;  // New yaw steering speed - start with a lower value as yaw can be quite powerful
    [SerializeField] float rollStabilizationSpeed = 5f; // Adjust as needed

    [HorizontalLine("Debug Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float horizontalXInput;
    [SerializeField] float verticalYInput;

    [SerializeField] Quaternion targetRotation; // For debugging and visualization
    [SerializeField] Quaternion slerpedRotation; // For debugging and visualization
    [SerializeField] Vector3 testDirection;

    void FixedUpdate()
    {
        horizontalXInput = Input.GetAxis("Horizontal");
        verticalYInput = Input.GetAxis("Vertical");

        float rollAngle = horizontalXInput * rollSteeringSpeed * Time.fixedDeltaTime;
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, transform.right);

        float yawAngle = horizontalXInput * yawSteeringSpeed * Time.fixedDeltaTime;
        Quaternion yawRotation = Quaternion.AngleAxis(yawAngle, transform.up);

        Quaternion currentRotation = rb.rotation; // Get current rotation

        if (Mathf.Abs(horizontalXInput) < 0.1f)
        {
            // 1. Calculate Target Rotation (Zero Roll)
            Vector3 currentEuler = currentRotation.eulerAngles;
            Vector3 targetEuler = new Vector3(0, currentEuler.y, 0f); // Keep Pitch (X), Yaw (Y), Reset Roll (Z to 0)
            targetRotation = Quaternion.Euler(targetEuler); // Convert back to Quaternion
            Debug.Log($"Target Rotation Euler: {targetEuler}"); // Debugging

            // 2. Slerp Towards Target Rotation
            float slerpT = rollStabilizationSpeed * Time.fixedDeltaTime; // 't' value for Slerp, based on stabilization speed and time
            slerpedRotation = Quaternion.Slerp(currentRotation, targetRotation, slerpT);
        }
        else
        {

            slerpedRotation = currentRotation * yawRotation * rollRotation; // Apply steering rotations
        }

        // 3. Apply Slerped Rotation to Rigidbody
        rb.MoveRotation(slerpedRotation);


        Vector3 moveDirection = transform.forward +(transform.right * horizontalXInput);
        moveDirection = moveDirection.normalized;
        rb.AddForce(moveDirection* forwardSpeed, ForceMode.Force);

        Debug.DrawRay(transform.position, transform.forward * forwardSpeed, Color.green);
        Debug.DrawRay(transform.position, transform.right * forwardSpeed, Color.red);
        Debug.DrawRay(transform.position, transform.up * forwardSpeed, Color.blue);
    }
}