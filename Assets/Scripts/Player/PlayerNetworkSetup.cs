using System;
using System.Collections.Generic;
using System.Linq;
using Born.Messaging;
using Born.Session;
using Born.UI;
using Fusion;
using UnityEngine;

namespace Born.Player
{
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
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
            var typesToDestroy = new List<Type>();
            
            if (Object.HasInputAuthority) 
            {
                //Setting up [Local Player]
                string chapter = Enum.GetName(typeof(Chapter), _chapter);
                string lesson = Enum.GetName(typeof(Lesson), _lesson);
                int playersCount = Runner.ActivePlayers.Count();
                int playersMax =  Runner.SessionInfo.MaxPlayers;
            
                welcomeMessage = $"Welcome to {Runner.SessionInfo.Name} ({playersCount}/{playersMax} Players). Chapter: {chapter}.";
                
                //Removing unnecessary components
                typesToDestroy.Add(typeof(Billboard));
                //Add other types here
                
                if (typesToDestroy.Count > 0)
                {
                    DestroyComponents(typesToDestroy);
                }
            }
            else
            {
                //Setting up [Remote Player]
                welcomeMessage = $"{Object.Id} joined";
                
                //Removing unnecessary components
                typesToDestroy.Add(typeof(PlayerAnimation));
                //Add other types here
                
                if (typesToDestroy.Count > 0)
                {
                    DestroyComponents(typesToDestroy);
                }
            }
        
            var msgData = new MessageData(welcomeMessage, Color.white);
            OnPlayerJoined?.Invoke(msgData);
        }

        private void DestroyComponents(List<Type> typesToRemove)
        {
            var comps = GetComponents<Component>();
            foreach (var comp in comps)
            {
                if(typesToRemove.Contains(comp.GetType()))
                {
                    Destroy(comp);
                }
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            var msgData = new MessageData($"{Object.Id} left", Color.red);
            OnPlayerJoined?.Invoke(msgData);
        }
    }
}