using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class CameraController : NetworkBehaviour
{
    private Camera mainCam;
    Vector2 camPos;
    Transform camTransform;

    private float xRot = 0f;            // rotating camera up and down
    private float yRot = 0f;            // rotating camera left and right

    [SerializeField]
    private float sensitivity = 50f;
    float grabDist = 6f;
    RaycastHit hit;

    private PlayerInput playerInput;
    InteractableObj inObj;              // access to publics in the InteractableObj file

    [SerializeField]
    private Transform playerBd;

    private bool clicked;
    private float lastClicked = 0f;     // time since the last click was registered


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;       // prevents cursor from moving out of the game
        
        mainCam = GetComponentInChildren<Camera>();
        playerInput = GetComponent<PlayerInput>();
        playerBd = GetComponent<Transform>();
    }

    private void Start()
    {
        camTransform = mainCam.transform;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        float mouseX = playerInput.actions["Look"].ReadValue<Vector2>().x * sensitivity * Time.deltaTime;
        float mouseY = playerInput.actions["Look"].ReadValue<Vector2>().y * sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        yRot += mouseX;

        mainCam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);
        playerBd.rotation = Quaternion.Euler(0f, yRot, 0f);
    }

    void Update()
    {
        if (!base.IsOwner) return;

        // when player selects object, trigger a bool attached to said object
        clicked = playerInput.actions["Select"].IsPressed();
        

        if(clicked && (Time.time - lastClicked) > 0.2)      // prevent multiple clicks
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

                if (inObj.Selected)
                {
                    inObj.Selected = false;
                }
                else
                {
                    inObj.Selected = true;
                }
            }
        }
        else
        {
            Debug.DrawRay(camTransform.position, camTransform.forward * grabDist, Color.red);
        }
    }

}
