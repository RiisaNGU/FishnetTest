using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class CameraController : NetworkBehaviour
{
    private Camera mainCam;
    private Transform camTransform;

    private float xRot = 0f;            // rotating camera up and down
    private float yRot = 0f;            // rotating camera left and right

    [SerializeField]
    private float sensitivity = 0.5f;
    private float grabDist = 7f;
    private RaycastHit hit;

    InteractableObj inObj;              // access to publics in the InteractableObj file
    private Transform playerBd;

    private float lastClicked = 0f;     // time since the last click was registered

    private void Start()
    {
        mainCam = GetComponentInChildren<Camera>();
        playerBd = GetComponent<Transform>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        Cursor.lockState = CursorLockMode.Locked;       // prevents cursor from moving out of the game
        mainCam.enabled = true;
        camTransform = mainCam.transform;

        float mouseX = context.ReadValue<Vector2>().x * sensitivity * Time.deltaTime;
        float mouseY = context.ReadValue<Vector2>().y * sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);             // locks the max rotation that the player can look up and down

        yRot += mouseX;

        mainCam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);

        playerBd.rotation = Quaternion.Euler(0f, yRot, 0f);
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        if (Time.time - lastClicked > 0.2)                // prevent multiple clicks
        {
            lastClicked = Time.time;
            checkRaycast();
        }
    }

    private void checkRaycast()
    {
        // raycast from the camera to select objects
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, grabDist))
        {
            if (hit.collider.gameObject.GetComponent<InteractableObj>() != null)        // if the object hit is an interactive object (contains InteractableObj.cs)
            {
                Debug.DrawRay(camTransform.position, camTransform.forward * grabDist, Color.yellow);
                Debug.Log("Clicked");

                inObj = hit.collider.gameObject.GetComponent<InteractableObj>();        // set inObj to the object that is currently colliding w/ the raycast

                if (inObj.Selected && inObj.CompareOwner(base.Owner))                   // object owner check
                {
                    inObj.Selected = false;
                }
                else if (!inObj.Selected && !inObj.CompareOwner(base.Owner))
                {
                    inObj.Selected = true;
                }
            }
        }
        else
            Debug.DrawRay(camTransform.position, camTransform.forward * grabDist, Color.red);
    }
}
