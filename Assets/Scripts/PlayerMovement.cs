using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Transform camTranform;
    private float moveSpeed = 5;
    void Awake() => camTranform = Camera.main.transform;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetData netData))
        {
            //print($"Receiving: {netData.direction}");
            transform.position += (moveSpeed * netData.direction.normalized * Runner.DeltaTime);
        }
    }
}