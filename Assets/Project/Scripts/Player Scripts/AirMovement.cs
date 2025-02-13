using CustomInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirMovement : MonoBehaviour
{
    [SelfFill][SerializeField] Rigidbody rb;

    [HorizontalLine("Movement Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float steeringSpeed = 5f;
    [SerializeField] float rollSteeringSpeed = 5f; // Existing roll steering speed
    [SerializeField] float yawSteeringSpeed = 2f;  // New yaw steering speed - start with a lower value as yaw can be quite powerful

    [HorizontalLine("Debug Stats", 3, FixedColor.CherryRed)]
    [SerializeField] float horizontalXInput;
    [SerializeField] float verticalYInput;

    private Vector3 currentMoveDirection = Vector3.forward; // Initial forward direction

    void FixedUpdate()
    {

        horizontalXInput = Input.GetAxis("Horizontal");
        verticalYInput = Input.GetAxis("Vertical");

        float rollAngle = horizontalXInput * rollSteeringSpeed * Time.fixedDeltaTime; 
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, transform.right); 

        float yawAngle =  horizontalXInput * yawSteeringSpeed * Time.fixedDeltaTime;
        Quaternion yawRotation = Quaternion.AngleAxis(yawAngle, transform.up);

        // 3. Apply Roll Rotation - Rotate the *Rigidbody* itself for physics-based roll
        rb.MoveRotation(rb.rotation * yawRotation *rollRotation); 

        rb.AddForce(transform.forward * forwardSpeed, ForceMode.Force);

        Debug.DrawRay(transform.position, transform.forward * forwardSpeed, Color.green); // Current forward
        Debug.DrawRay(transform.position, transform.right * forwardSpeed, Color.red);    
        Debug.DrawRay(transform.position, transform.up * forwardSpeed, Color.blue);   
    }
}
