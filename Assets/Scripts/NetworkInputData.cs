using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 Direction;
    public NetworkButtons Buttons;

}

public enum MyButtons {
    Jump = 0,
    Color = 1,
}
