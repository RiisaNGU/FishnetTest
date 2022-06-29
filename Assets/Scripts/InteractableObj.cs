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

    [SerializeField]
    [SyncVar]
    private int count = 0;              // number of times the object has been selected/clicked

    [SerializeField]
    [SyncVar (OnChange = nameof(OnSelected))]
    private bool selected = false;      // object is/isnt currently selected

    public bool Selected { get { return selected; } set { selected = value; } }     // protecting the selected variable with a get/set

    private MeshRenderer mesh;          

    private Color defaultCol;           // save OG red in this

    private void Awake()
    {
        defaultCol = GetComponent<MeshRenderer>().material.color;
        mesh = GetComponent<MeshRenderer>();
    }

    [ServerRpc (RequireOwnership = false)]
    private void setColor(Color col)
    {
        mesh.material.color = col;
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnSelected(bool oldSel, bool newSel, bool asServer)
    {
        Debug.Log(newSel);

        Selected = newSel;

        if (newSel)
        {
            Debug.Log("Selected");

            count++;
            setColor(Color.green);
        }
        else
        {
            Debug.Log("Unselected");
            setColor(defaultCol);
        }

    }

    [ServerRpc (RequireOwnership = false)]
    private void trackPos()     // takes in the position of the selected object
    {
        objPos = obj.transform.position;
    }

    [ServerRpc(RequireOwnership = false)]
    private void Update()       // local to 'owner'
    {
        trackPos();
    }

}
