using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

// used for UI

public class Player : NetworkBehaviour
{
    /// SyncVar is used to synchronize values along the client and server

    [SyncVar]
    [SerializeField]
    string playerName;

    [SerializeField]
    Vector3 currentPos;                         // current position of player

    [SerializeField]
    private TMP_Text playerDisplay;             // TMPro UI- player's name, serialized so we can drag and drop the actual textbox that will be linked to this field

    [SerializeField]
    private TMP_Text playerPos;                 // current player position- local

    [SerializeField]
    private TMP_Text timeElapsed;               // time elapsed since client connected to server- local

    // selected object info
    [SerializeField]
    private TMP_Text objType;

    [SerializeField]
    private TMP_Text objCoord;

    [SerializeField]
    private TMP_Text objSelectedCount;

    private Canvas canvas;

    /// <summary>
    ///     Instantiates a player object every time a client starts up and connects to the server
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();        // ALWAYS call the base function- prevents bugs later on

        if (!base.IsOwner) return;   // if the object is owned by the client then the rest of the code will run, if not, the function will exit

        canvas = GetComponentInChildren<Canvas>();

        setUser();                   // set username based on the client connect number
        SetUI();               // activates the client's UI, prevents overlap
    }

    private void SetUI()
    {
        canvas.enabled = true;
    }

    /// <summary>
    /// Create's player name based on client connection number
    /// </summary>
    private void setUser()
    {
        playerName = $"Player {OwnerId}";
    }

    /// <summary>
    /// Formats the milliseconds elapsed in game to [Minutes:Seconds:Milliseconds]
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private string timeFormat(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    private void Update()
    {
        // local to each player
        currentPos = GameObject.FindGameObjectWithTag("Player").transform.position;     // Tracks the current position of the player
        playerPos.text = currentPos.ToString();

        playerDisplay.text = playerName;                                                // sets the UI text for player name

        timeElapsed.text = timeFormat(Time.timeSinceLevelLoad);                         // time since level has loaded

        // object info UI

        // if object is selected
        // set these all as active

            //objType.text =

            //objCoord.text =

            //objSelectedCount = 

        // else do nothing
    }

}
