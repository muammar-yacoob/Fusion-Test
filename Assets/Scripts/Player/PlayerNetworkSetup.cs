using System;
using System.Linq;
using Fusion;
using UnityEngine;

public class PlayerNetworkSetup : NetworkBehaviour
{
    public static event Action<MessageData> OnPlayerJoined =  delegate{};

    private Map map = Map.Airport;
    private Stage stage;
    
    public override void Spawned()
    {
        if (Runner.SessionInfo.Properties.TryGetValue(nameof(Map), out var sessionMap) && sessionMap.IsInt)
        {
            map = (Map) sessionMap.PropertyValue;
        }
        
        if (Runner.SessionInfo.Properties.TryGetValue(nameof(Stage), out var sessionStage) && sessionStage.IsInt)
        {
            stage = (Stage)sessionStage.PropertyValue;
        }


        string welcomeMessage;
        if (Object.HasInputAuthority)
        {
            //Local Player Setup
            string mapName = Enum.GetName(typeof(Map), map);
            string stageName = Enum.GetName(typeof(Stage), stage);
            int playersCount = Runner.ActivePlayers.Count();
            int playerMax = Runner.SessionInfo.MaxPlayers;
            
            welcomeMessage = $"Welcome to {Runner.SessionInfo.Name} ({playersCount}/{playerMax} Players). Map: {mapName} Stage:{stageName}.";
        }
        else
        {
            //Remote Player Setup
            welcomeMessage = $"{Object.Id} joined";
            //Destroy(gameObject.GetComponentInChildren<AnimationController>());
        }
        
        var msgData = new MessageData(welcomeMessage, Color.white);
        OnPlayerJoined?.Invoke(msgData);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        var msgData = new MessageData($"{Object.Id} left", Color.red);
        OnPlayerJoined?.Invoke(msgData);
    }
}