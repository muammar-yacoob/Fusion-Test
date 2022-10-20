using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerMovement : NetworkBehaviour
{
    private Transform camTranform;
    private float moveSpeed = 5;
    void Awake() => camTranform = Camera.main.transform;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetData netData))
        {
            transform.Translate(netData.direction.normalized * Runner.DeltaTime);
        }
    }
}