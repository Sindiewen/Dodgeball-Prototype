using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManager : MonoBehaviour
{

    private PlayerInputControls controls;

    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;

    

    private void awake()
    {
        controls = new PlayerInputControls();
        // controls.Player.Move.performed += ctx => defineMovement(ctx.ReadValue<Vector2>());
        
        
    }
    private void OnEnable() 
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = controls.Player.Move.ReadValue<float>();
        Debug.Log("Horizontal value: " + horizontal.ToString());
    }

    public Vector2 defineMovement(Vector2 direction)
    {
        return direction;
    }
}
