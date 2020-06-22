using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    // Mouse input Attributes
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 100f;

    // character controller, parent object to rotate
    public Transform playerBody;

    private Vector2 mousePositionVector;

    // To rotate the player around the x axis for looking up and down
    private float xRotation = 0f;

    public void mouseVectorInputPassthrough(InputAction.CallbackContext context)
    {
        mousePositionVector = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // At the moment, this using Unity's old Input system. This will work for now but consider
        // move over to Unity's new system soon.
        float mouseX = mousePositionVector.x * mouseSensitivityX * Time.deltaTime;
        float mouseY = mousePositionVector.y * mouseSensitivityY * Time.deltaTime;

        // player look

        // Creates clamp for rotating around x axis. 
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
