using UnityEngine;
using UnityEngine.Events;

public class ParkingSpot : MonoBehaviour
{
    [Tooltip("How long the player must stay in the parking spot to win (seconds).")]
    public float requiredParkingTime = 3f;

    [Tooltip("The maximum speed the car can be moving to be considered 'parked'.")]
    public float maxParkedVelocity = 0.5f;

    [Tooltip("Tag of the player vehicle.")]
    public string playerTag = "Player";

    [Tooltip("Event fired when the player successfully parks.")]
    public UnityEvent OnSuccessfullyParked;

    private bool _isPlayerInSpot = false;
    private float _currentParkTime = 0f;
    private Rigidbody _playerRigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) || (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(playerTag)))
        {
            _isPlayerInSpot = true;
            _currentParkTime = 0f; // Reset timer upon entry
            
            if (other.attachedRigidbody != null)
            {
                _playerRigidbody = other.attachedRigidbody;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) || (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(playerTag)))
        {
            _isPlayerInSpot = false;
            _currentParkTime = 0f; // Reset timer upon exit
            _playerRigidbody = null;
        }
    }

    private void Update()
    {
        if (_isPlayerInSpot)
        {
            // Check if the car is stopped (or moving very slowly)
            bool isParked = true;
            if (_playerRigidbody != null && _playerRigidbody.linearVelocity.magnitude > maxParkedVelocity)
            {
                isParked = false;
            }

            if (isParked)
            {
                _currentParkTime += Time.deltaTime;
                
                // Optional: You could fire an event here to update a UI parking timer!

                if (_currentParkTime >= requiredParkingTime)
                {
                    Debug.Log("Successfully Parked! You Win!");
                    OnSuccessfullyParked?.Invoke();
                    
                    // Stop checking to prevent multiple events
                    _isPlayerInSpot = false; 
                }
            }
            else
            {
                // Reset timer if they start moving around too much while inside
                _currentParkTime = 0f;
            }
        }
    }
}
