using UnityEngine;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour
{
    [Header("Toggle Settings")]
    [Tooltip("The GameObject whose active status you want to toggle.")]
    public GameObject targetObject;

    [Tooltip("The controller button input action to trigger the toggle.")]
    public InputActionReference toggleAction;

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnToggleAction;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed -= OnToggleAction;
            toggleAction.action.Disable();
        }
    }

    private void OnToggleAction(InputAction.CallbackContext context)
    {
        if (targetObject != null)
        {
            // Toggle the active state
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
