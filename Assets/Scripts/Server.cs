using FishNet;
using FishNet.Object;
using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject spherePrefab;

    [ServerRpc]
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkObject obj = Instantiate(spherePrefab);
        InstanceFinder.ServerManager.Spawn(obj, null);
    }
}
