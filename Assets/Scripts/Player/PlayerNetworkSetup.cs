using System;
using Fusion;
using UnityEngine;

public class PlayerNetworkSetup : NetworkBehaviour
{
    public static event Action<string> OnPlayerJoined =  delegate{};
    public override void Spawned()
    {
        string welcomeMessage;
        if (Object.HasInputAuthority)
        {
            //Local Player Setup
            welcomeMessage = $"Welcome to {Runner.SessionInfo.Name}";
        }
        else
        {
            //Remote Player Setup
            welcomeMessage = $"{Object.Id} joined";
        }
        OnPlayerJoined?.Invoke(welcomeMessage);
        
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerJoined?.Invoke($"{Object.Id} left");
    }
}