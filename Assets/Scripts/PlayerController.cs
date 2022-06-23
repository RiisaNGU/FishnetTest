using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpHeight;

    private PlayerInput playerInput;

    private Rigidbody rb;
    private CapsuleCollider cc;

    [SerializeField]
    private LayerMask groundLayer;

    private Vector2 move;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            Debug.Log("Jump");
        }
        else
            Debug.Log("No Jump");
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
    /// Debug purposes, hold down 'Z' to unlcok the cursor
    /// </summary>
    /// <param name="context"></param>
    public void OnUnlockCamera(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;

        float clicked = playerInput.actions["Unlock Camera"].ReadValue<float>();

        if (clicked == 1)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!base.IsOwner) return;

        move = playerInput.actions["Move"].ReadValue<Vector2>();

        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.y * speed);
        //Debug.Log(move);
    }
}
