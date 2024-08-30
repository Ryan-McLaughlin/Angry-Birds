using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    private InputAction _mousePositionAction;
    private InputAction _mouseAction;

    public static Vector2 MousePosition;

    public static bool WasPrimaryPressed;
    public static bool WasPrimaryReleased;
    public static bool IsPrimaryPressed;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _mousePositionAction = PlayerInput.actions["MousePosition"];
        _mouseAction = PlayerInput.actions["Mouse"];
    }

    private void Update()
    {
        MousePosition = _mousePositionAction.ReadValue<Vector2>();

        WasPrimaryPressed = _mouseAction.WasPressedThisFrame();
        WasPrimaryReleased = _mouseAction.WasReleasedThisFrame();
        IsPrimaryPressed = _mouseAction.IsPressed();
    }
}
