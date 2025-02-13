using CustomInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirMovement : MonoBehaviour
{
    [SelfFill][SerializeField] Rigidbody rb;

    [HorizontalLine("Movement Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float speed = 10f;
    [SerializeField] float steeringSpeed = 5f;
    [SerializeField] float rollSteeringSpeed = 5f; // Existing roll steering speed
    [SerializeField] float yawSteeringSpeed = 2f;  // New yaw steering speed - start with a lower value as yaw can be quite powerful

    [HorizontalLine("Debug Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float HorizontalXInput;
    [SerializeField] float VerticalYInput;

    private Vector3 currentMoveDirection = Vector3.forward; // Initial forward direction

    void FixedUpdate()
    {
        // 1. Get Input
        HorizontalXInput = Input.GetAxis("Horizontal");
        VerticalYInput = Input.GetAxis("Vertical");

        // 2. Calculate Roll Rotation - Rotate around the *local right axis* (transform.right) [Rolling]
        float rollAngle = HorizontalXInput * rollSteeringSpeed * Time.fixedDeltaTime; 
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, transform.right); 

        // 3. Apply Roll Rotation - Rotate the *Rigidbody* itself for physics-based roll
        rb.MoveRotation(rb.rotation * rollRotation); // Apply rotation to Rigidbody's rotation

        // 4. Apply Force - Move in the current *forward direction* (now influenced by roll)
        rb.AddForce(transform.forward * speed, ForceMode.Force);

        // Debugging
        Debug.DrawRay(transform.position, transform.forward * speed, Color.green); // Current forward
        Debug.DrawRay(transform.position, transform.right * speed, Color.red);    // Current right
        Debug.DrawRay(transform.position, transform.up * speed, Color.blue);     // Current up
    }
}
