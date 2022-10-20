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
        //if (!Object.HasInputAuthority) return;
        
        if (GetInput(out NetData netData))
        {
            //print($"Receiving: {netData.direction}");
            transform.Translate(netData.direction * moveSpeed * Runner.DeltaTime);
        }
    }
}