using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Player : NetworkBehaviour
{
    /// SyncVar is used to synchronize values along the client and server

    [SyncVar]
    [SerializeField]
    string playerName;

    [SyncVar]
    [SerializeField]
    int playerCount;            // player number (determined by number of clients connected)

    [SyncVar]
    [SerializeField]
    Vector3 currentPos;         // current position of player

    [SyncVar]
    private Time timeElapsed;   // time elapsed since game started


    //////////////////////////////////////

    /// <summary>
    ///     Instantiates a player object every time a client starts up and connects to the server
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();   // always call the base function- prevents bugs later on

        if (!IsOwner) return;   // if the object is owned by the client then the rest of the code will run, if not, the function will exit

        setUser($"Player " + (++playerCount).ToString());   // set username based on the client connect number
    }

    [ServerRpc]
    private void setUser(string name)
    {
        playerName = name;
    }

    [ServerRpc]
    public void Update()
    {
        currentPos = GameObject.FindGameObjectWithTag("Player").transform.position;     // Tracks the current position of the player
    }

}
