using Fusion;
using UnityEngine;

public struct NetData : INetworkInput
{
    public NetworkButtons buttons;
    public Vector3 direction;
    public float rotation;
}