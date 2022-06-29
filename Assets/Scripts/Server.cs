using FishNet;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class Server : NetworkBehaviour
{
    // list of dynamic network objects that appear in the scene
    [SerializeField]
    private List<NetworkObject> NetworkObjects = new List<NetworkObject>();

    private NetworkObject obj;

    [Server]
    public override void OnStartServer()        // runs when server starts up
    {
        base.OnStartServer();

        // instantiate every network object in the list
        for(int i = 0; i < NetworkObjects.Count; i++)
        {
            obj = Instantiate(NetworkObjects[i]);
            InstanceFinder.ServerManager.Spawn(obj, null);
        }
    }
}
