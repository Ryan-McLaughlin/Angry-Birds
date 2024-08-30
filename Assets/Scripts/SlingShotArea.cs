using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask _slingShotAreaMask;

    public bool IsWithinSlingShotArea()
    {
        //Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        // overlap point from the physics system
        // if mouse is actually touching a collider in the Sling Shot Area mask (layer)
        if (Physics2D.OverlapPoint(worldPosition, _slingShotAreaMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
