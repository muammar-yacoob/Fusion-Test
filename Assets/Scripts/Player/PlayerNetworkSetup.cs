using System;
using System.Linq;
using Born.Messaging;
using Born.UI;
using Fusion;
using UnityEngine;

namespace Born.Player
{
    public class PlayerNetworkSetup : NetworkBehaviour
    {
        public static event Action<MessageData> OnPlayerJoined =  delegate{};

        private Chapter _chapter = Chapter.Hanger;
        private Lesson _lesson;
    
        public override void Spawned()
        {
            if (Runner.SessionInfo.Properties.TryGetValue(nameof(Chapter), out var sessionMap) && sessionMap.IsInt)
            {
                _chapter = (Chapter) sessionMap.PropertyValue;
            }
        
            if (Runner.SessionInfo.Properties.TryGetValue(nameof(Lesson), out var sessionStage) && sessionStage.IsInt)
            {
                _lesson = (Lesson)sessionStage.PropertyValue;
            }


            string welcomeMessage;
            if (Object.HasInputAuthority) // My player
            {
                //Local Player Setup
                string mapName = Enum.GetName(typeof(Chapter), _chapter);
                string stageName = Enum.GetName(typeof(Lesson), _lesson);
                int playersCount = Runner.ActivePlayers.Count();
                int playerMax = Runner.SessionInfo.MaxPlayers;
            
                welcomeMessage = $"Welcome to {Runner.SessionInfo.Name} ({playersCount}/{playerMax} Players). Map: {mapName} Stage:{stageName}.";
                
                //remove unneccessary components
                Destroy(gameObject.GetComponentInChildren<Billboard>().gameObject);
            }
            else // Other players
            {
                //Remote Player Setup
                welcomeMessage = $"{Object.Id} joined";
                
                //remove unneccessary components
                Destroy(gameObject.GetComponentInChildren<PlayerAnimation>());
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
}