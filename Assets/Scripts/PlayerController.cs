using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            Debug.Log("Jump");
        }
        else
            Debug.Log("No Jump");
    }

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

    public void OnClick(InputAction.CallbackContext context)
    {
        // when player selects object, trigger a bool attached to said object
        float clicked = playerInput.actions["Select"].ReadValue<float>();

        if(clicked == 1)
        {
            Debug.Log("Clicked");
        }
        else
            Debug.Log("Unclicked");

        // if you are looking at an object
        // get the object's information
        // else do nothing


    }

    private void Update()
    {
        move = playerInput.actions["Move"].ReadValue<Vector2>();

        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.y * speed);
        //Debug.Log(move);
    }
}
