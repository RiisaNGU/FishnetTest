using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class InteractableObj : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    private GameObject obj;             // the object that the script is attached to

    [SyncVar]
    private string type;                // name/type of the object

    [SerializeField]
    [SyncVar]
    private Vector3 objPos;             // current position of the object

    [SerializeField]
    [SyncVar]
    private int count = 0;              // number of times the object has been selected/clicked

    [SerializeField]
    [SyncVar (OnChange = nameof(OnSelected))]
    private bool selected = false;      // object is/isnt currently selected

    public bool Selected { get { return selected; } set { selected = value; } }     // protecting the selected variable with a get/set

    private MeshRenderer mesh;

    [SyncVar]
    private Color defaultCol;           // save OG red in this

    private void Awake()
    {
        defaultCol = GetComponent<MeshRenderer>().material.color;
        mesh = GetComponent<MeshRenderer>();
    }

    private void setColor(Color col)
    {
        mesh.material.color = col;
    }

    private void OnSelected(bool oldSel, bool newSel, bool asServer) 
    {
        if (newSel)
        {
            Debug.Log("Selected");
            count++;
            setColor(Color.green);
            this.GiveOwnership(LocalConnection);    // gives object ownership to the client that calls the method
        }
        else
        {
            Debug.Log("Unselected");
            setColor(defaultCol);
            this.RemoveOwnership();
        }
        Debug.Log(this.OwnerId);
    }

    private void trackPos()     // takes in the position of the selected object
    {
        objPos = obj.transform.position;
    }

    private void Update()       // local to 'owner'
    {
        trackPos();
    }
}
