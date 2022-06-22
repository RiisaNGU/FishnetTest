using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObj : NetworkBehaviour
{
    enum Type
    {
        Cube,
        Sphere,
        Cylinder
    }

    [SyncVar]
    private Type objType;

    [SyncVar]
    private GameObject obj;

    [SyncVar]
    [SerializeField]
    private Vector3 objPos;

    [SyncVar]
    [SerializeField]
    bool selected = false;

    private MeshRenderer mesh;

    private Color defaultCol;

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
    /// <param name="oldState"></param>
    /// <param name="newState"></param>
    /// <param name="asServer"></param>
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
