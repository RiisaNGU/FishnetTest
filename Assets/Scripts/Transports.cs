using UnityEngine;
using FishNet;
using FishNet.Transporting.Multipass;
using FishNet.Managing.Transporting;
using FishNet.Transporting.Tugboat;
using FishNet.Transporting.Bayou;

public class Transports : MonoBehaviour
{
    [SerializeField] Multipass multipass;
    private void Start()
    {
        ClientTransport();   
    }

    // determines if the player in playing on web, editor, or pc
    private void ClientTransport()
    {
#if UNITY_WEBGL && !UNITY_EDITOR 
        multipass.SetClientTransport<Bayou>();             // WebGL transport
#else
        multipass.SetClientTransport<Tugboat>();           // Editor and pc transport
#endif
    }
}
