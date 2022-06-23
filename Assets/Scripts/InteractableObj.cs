using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObj : NetworkBehaviour
{
    [SyncVar]
    private string type;

    [SyncVar]
    private GameObject obj;

    [SyncVar]
    [SerializeField]
    private Vector3 objPos;

    [SyncVar]
    [SerializeField]
    private bool selected = false;

    public bool Selected { get { return selected; } set { selected = value; } }

    private MeshRenderer mesh;

    private Color defaultCol;       // save OG red in this

    [SyncVar(OnChange = nameof(OnColorChange))]
    Color color;

    private void Awake()
    {
        defaultCol = GetComponent<MeshRenderer>().material.color;
        obj = GetComponent<GameObject>();
    }

    private void OnColorChange(Color oldC, Color newC, bool asServer)
    {
        mesh.material.color = newC;
    }

    /// <summary>
    /// If the object gets selcted by any client connection
    /// </summary>
    private void OnClicked(bool oldState, bool newState, bool asServer)
    {
        selected = newState;

        // if newstate is true
        // ---turn all these on---
        // else turn it off
    }


    private void setColor(Color col)
    {
        color = col;
    }

    private void trackPos()     // takes in the position of the seletced object
    {
        objPos = obj.transform.position;
    }

    private void Update()
    {
        
    }

}
