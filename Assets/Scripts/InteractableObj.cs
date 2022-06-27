using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObj : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    private GameObject obj;             // the object that the script is attached to

    [SyncVar]
    private string type;                // name/type of the object

    [SyncVar]
    [SerializeField]
    private Vector3 objPos;             // current position of the object

    [SyncVar]
    private int count = 0;              // number of times the object has been selected/clicked

    [SyncVar]
    [SerializeField]
    private bool selected = false;      // object is/isnt currently selected

    public bool Selected { get { return selected; } set { selected = value; } }     // protecting the selected variable with a get/set

    private MeshRenderer mesh;          // to activate the mesh??

    private Color defaultCol;           // save OG red in this

    [SyncVar(OnChange = nameof(OnColorChange))]     // saving the CURRENT colour
    Color color;

    private void Awake()
    {
        defaultCol = GetComponent<MeshRenderer>().material.color;
    }

    private void OnColorChange(Color oldC, Color newC, bool asServer)
    {
        mesh.material.color = newC;
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
        trackPos();
    }

}
