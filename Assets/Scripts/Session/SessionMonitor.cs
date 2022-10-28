using System;
using System.Collections.Generic;
using Born.Core;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Born.Session
{
    public class SessionMonitor: NetworkBehaviour, INetworkRunnerCallbacks
    {
        public override void Spawned() => Runner.AddCallbacks(this);
        public override void Despawned(NetworkRunner runner, bool hasState) => runner.RemoveCallbacks(this);
    
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            Debug.Log($"Session List Updated: Count:{sessionList.Count}");
            if (sessionList.Count == 0) return;
            
            foreach (var session in sessionList)
            {
                //if (!session.IsValid) return;
                
                Chapter currentChapter = Chapter.Hanger;
                Lesson currentLesson = Lesson.Intro;

                if(session.Properties.TryGetValue(nameof(Chapter),out var tmpProperty) && tmpProperty.IsType<Chapter>())
                {
                    currentChapter = (Chapter)tmpProperty.PropertyValue;
                }
                
                if(session.Properties.TryGetValue(nameof(Lesson),out tmpProperty) && tmpProperty.IsType<Lesson>())
                {
                    currentLesson = (Lesson)tmpProperty.PropertyValue;
                }
                
                Debug.Log($"{session.Name}: Players({session.PlayerCount},{session.MaxPlayers}), Stage:{currentChapter.GetName()}/{currentLesson.GetName()}");
            }
        }

        #region Other Callbacks
        public void OnConnectedToServer(NetworkRunner runner){}
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){}
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){}
        public void OnInput(NetworkRunner runner, NetworkInput input){}
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
        public void OnDisconnectedFromServer(NetworkRunner runner){}
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){}
        public void OnSceneLoadDone(NetworkRunner runner){}
        public void OnSceneLoadStart(NetworkRunner runner){}
        #endregion
    }
}