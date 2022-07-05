using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

[DisallowMultipleComponent]
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody rb;
    private CapsuleCollider cc;
    private Vector2 move;
    private Vector3 mainCam;

    private Quaternion rot;

    private Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        cam = GetComponentInChildren<Camera>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            //Debug.Log("Jump");

            IsJumping("Jump");
        }
        else
            Debug.Log("No Jump");
    }

    [ServerRpc (RunLocally = true)]
    private void IsJumping(string msg)
    {
        Debug.Log($"{msg}");
    }

    /// <summary>
    /// Determines whether the player is touching the ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        float rayBuffer = 0.5f;

        bool isHit = Physics.Raycast(cc.bounds.center, Vector3.down, cc.bounds.extents.y + rayBuffer, groundLayer);

        // debug stuff
        Color rayC;
        if (isHit)
        {
            rayC = Color.green;
        }
        else
        {
            rayC = Color.red;
        }

        Debug.DrawRay(cc.bounds.center, Vector2.down * (cc.bounds.extents.y + rayBuffer), rayC);

        return isHit;
    }

    /// <summary>
    /// Debug purposes, hold down 'Z' to unlock the cursor
    /// </summary>
    /// <param name="context"></param>
    public void OnUnlockCamera(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        bool clicked = context.performed;

        if (clicked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        move = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (!base.IsOwner) return;

        //Vector3 lookCam = cam.transform.localEulerAngles;
        //lookCam.y = 0f;

        //mainCam = cam.transform.TransformDirection(lookCam);
        //mainCam.y = 0f;
        //mainCam.Normalize();

        //Vector3 newMove = Vector3.MoveTowards(this.transform.forward, mainCam, speed * Time.deltaTime);

        //rot = cam.transform.rotation;
        //rot.x = 0f;
        //rot.z = 0f;
        //transform.rotation = rot;

        //rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.y * speed);

        transform.Translate(new Vector3(move.x, 0, move.y) * (speed * Time.deltaTime));

        //Debug.Log(mainCam);
    }
}
