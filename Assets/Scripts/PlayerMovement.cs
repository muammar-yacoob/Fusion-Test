using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterControllerPrototype))]
public class PlayerMovement : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _cc;

    private void Awake()=> _cc = GetComponent<NetworkCharacterControllerPrototype>();

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5*data.direction*Runner.DeltaTime);
        }
    }
}