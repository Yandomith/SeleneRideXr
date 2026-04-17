using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    [Tooltip("The current target the arrow should point towards.")]
    public Transform target;

    [Tooltip("If true, the arrow will only rotate horizontally (Y-axis), keeping it flat.")]
    public bool lockYAxis = true;

    [Tooltip("How fast the arrow rotates towards the target.")]
    public float rotationSpeed = 10f;

    private void Update()
    {
        if (target == null) return;

        // Calculate the direction from the arrow to the target
        Vector3 direction = target.position - transform.position;

        // If lockYAxis is true, we flatten the direction vector so the arrow doesn't tilt up or down
        if (lockYAxis)
        {
            direction.y = 0;
        }

        // Make sure we have a valid direction before trying to rotate
        if (direction != Vector3.zero)
        {
            // Determine the rotation needed to look at the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
