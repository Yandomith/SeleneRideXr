using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    [Tooltip("List of checkpoints in the order they must be passed.")]
    public List<Checkpoint> checkpoints;

    [Tooltip("Optional reference to the ArrowPointer to guide the player.")]
    public ArrowPointer navigationArrow;

    [Tooltip("Reference to the final Parking Spot.")]
    public ParkingSpot finalParkingSpot;

    private int _currentCheckpointIndex = 0;

    private void Start()
    {
        InitializeCheckpoints();
    }

    private void InitializeCheckpoints()
    {
        if (checkpoints == null || checkpoints.Count == 0)
        {
            Debug.LogWarning("CheckpointManager: No checkpoints assigned!");
            return;
        }

        // Deactivate all checkpoints initially except the first one
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] != null)
            {
                // Subscribe to the event
                checkpoints[i].OnCheckpointPassed.AddListener(HandleCheckpointPassed);
                
                // Activate only the first checkpoint
                checkpoints[i].gameObject.SetActive(i == 0);
            }
        }

        _currentCheckpointIndex = 0;
        
        // Point the arrow at the first checkpoint
        if (navigationArrow != null && checkpoints.Count > 0)
        {
            navigationArrow.target = checkpoints[0].transform;
        }

        // Deactivate parking spot initially
        if (finalParkingSpot != null)
        {
            finalParkingSpot.gameObject.SetActive(false);
            // Listen for win condition
            finalParkingSpot.OnSuccessfullyParked.AddListener(HandleGameWon);
        }

        Debug.Log("CheckpointManager: Sequence started.");
    }

    private void HandleCheckpointPassed(Checkpoint passedCheckpoint)
    {
        // Safety check to ensure the passed checkpoint is the current expected one
        if (passedCheckpoint == checkpoints[_currentCheckpointIndex])
        {
            // Deactivate the completed checkpoint
            passedCheckpoint.gameObject.SetActive(false);

            // Increment index
            _currentCheckpointIndex++;

            if (_currentCheckpointIndex < checkpoints.Count)
            {
                // Activate the next checkpoint
                checkpoints[_currentCheckpointIndex].gameObject.SetActive(true);
                
                // Update the arrow to point to the new checkpoint
                if (navigationArrow != null)
                {
                    navigationArrow.target = checkpoints[_currentCheckpointIndex].transform;
                }

                Debug.Log($"CheckpointManager: Checkpoint {_currentCheckpointIndex} unlocked!");
            }
            else
            {
                // All checkpoints passed
                Debug.Log("CheckpointManager: All checkpoints completed! Sequence finished.");
                HandleSequenceComplete();
            }
        }
    }

    private void HandleSequenceComplete()
    {
        Debug.Log("Ready to park!");
        
        if (finalParkingSpot != null)
        {
            // Activate the parking spot
            finalParkingSpot.gameObject.SetActive(true);
            
            // Point the arrow to the parking spot
            if (navigationArrow != null)
            {
                navigationArrow.target = finalParkingSpot.transform;
            }
        }
        else
        {
            // If there's no parking spot, just clear the arrow
            if (navigationArrow != null) navigationArrow.target = null; 
        }
    }

    private void HandleGameWon()
    {
        Debug.Log("Game Over - Player Won!");
        if (navigationArrow != null) navigationArrow.target = null; // Hide or clear the arrow
        
        // You can trigger further UI changes or level transitions here
    }
}
