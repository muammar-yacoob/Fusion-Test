using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Born.Core
{
    public class InputProvider : MonoBehaviour, INetworkRunnerCallbacks
    {
        private SimpleControls _playerActionMap;

        private void OnEnable()
        {
            _playerActionMap = new();
            _playerActionMap.gameplay.Enable();
        }

        private void OnDisable()
        {
            _playerActionMap.gameplay.Disable();
        }

        public void OnInput(NetworkRunner runner, NetworkInput netInput)
        {
            //Collecting local input
            var localX = _playerActionMap.gameplay.move.ReadValue<Vector2>().x;
            var localZ = _playerActionMap.gameplay.move.ReadValue<Vector2>().y;

            var tmpInput = new NetworkInputData();
            
            //Mapping buttons
            tmpInput.Direction.Set(localX, 0, localZ);
            tmpInput.Buttons.Set(MyButtons.Color, _playerActionMap.gameplay.color.IsPressed());
            tmpInput.Buttons.Set(MyButtons.Jump, _playerActionMap.gameplay.jump.IsPressed());
            tmpInput.Buttons.Set(MyButtons.NextChapter, _playerActionMap.gameplay.nextChapter.IsPressed());
            tmpInput.Buttons.Set(MyButtons.PreviousChapter, _playerActionMap.gameplay.previousChapter.IsPressed());
        
            //Sending input over network
            netInput.Set(tmpInput);
        }

        #region other callbacks
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        #endregion
    }
}