using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Camera mainCam;
    Vector2 camPos;
    Transform camTransform;

    private float xRot = 0f;            // rotating camera up and down
    private float yRot = 0f;            // rotating camera left and right

    [SerializeField]
    private float sensitivity = 100f;
    float grabDist = 6f;
    RaycastHit hit;

    private PlayerInput playerInput;

    [SerializeField]
    private Transform playerBd;

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

    void Update()
    {
        float mouseX = playerInput.actions["Look"].ReadValue<Vector2>().x * sensitivity * Time.deltaTime;
        float mouseY = playerInput.actions["Look"].ReadValue<Vector2>().y * sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        yRot += mouseX;


        mainCam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);
        playerBd.rotation = Quaternion.Euler(0f, yRot, 0f);

        //Debug.Log(Vector3.up * mouseX);

        // raycast from the camera to select objects
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, grabDist))
        {
            //Transform objectHit = hit.transform;
            Debug.DrawRay(camTransform.position, camTransform.forward * grabDist, Color.yellow);

            // if colliding with interactable
            // and if clicked
            // "select" the object and set it's bool to "true" aka "selected"
            // be able to see the object's:
                // name
                // coords
                // amount of times it's been selected
        }
        else
        {
            Debug.DrawRay(camTransform.position, camTransform.forward * grabDist, Color.red);
        }
    }
}
