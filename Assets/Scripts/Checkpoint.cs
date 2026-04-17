using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("Event fired when the player successfully passes through the checkpoint in the correct direction.")]
    public UnityEvent<Checkpoint> OnCheckpointPassed;

    [Tooltip("Tag of the player vehicle.")]
    public string playerTag = "Player";

    // Track if the player entered from the correct side
    private bool _enteredCorrectly = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) || other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(playerTag))
        {
            // Calculate local position of the player relative to the checkpoint
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

            // Assuming the checkpoint faces forward along the Z axis, 
            // entering from behind means the Z position should be negative.
            if (localPos.z < 0f)
            {
                _enteredCorrectly = true;
            }
            else
            {
                _enteredCorrectly = false; // Entered from the wrong side (the front)
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) || other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(playerTag))
        {
            // Calculate local position of the player upon exiting
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

            // If they entered correctly (from behind) and are now exiting from the front (Z > 0)
            if (_enteredCorrectly && localPos.z > 0f)
            {
                // Successfully passed through the checkpoint!
                Debug.Log($"Checkpoint '{gameObject.name}' passed correctly!");
                OnCheckpointPassed?.Invoke(this);
                
                // Reset state so it can't be triggered multiple times if they reverse back into it immediately
                _enteredCorrectly = false; 
            }
            else
            {
                // Reset state if they backed out or failed
                _enteredCorrectly = false;
            }
        }
    }
}
