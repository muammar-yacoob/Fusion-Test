using System;
using Fusion;
using UnityEngine;

public class PlayerNetworkSetup : NetworkBehaviour
{
    public static event Action<MessageData> OnPlayerJoined =  delegate{};
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
        
        var msgData = new MessageData(welcomeMessage, Color.blue);
        OnPlayerJoined?.Invoke(msgData);
        
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        var msgData = new MessageData($"{Object.Id} left", Color.red);
        OnPlayerJoined?.Invoke(msgData);
    }
}