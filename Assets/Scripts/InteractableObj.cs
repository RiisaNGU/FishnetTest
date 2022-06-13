using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class InteractableObj : NetworkBehaviour
{
    enum Type
    {
        Cube,
        Sphere,
        Cylinder
    }

    [SyncVar]
    [SerializeField]
    Type objType;

    [SyncVar]
    [SerializeField]
    Vector3 objPos;



}
