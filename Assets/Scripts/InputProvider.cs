using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(NetworkObject))]
public class InputProvider : NetworkBehaviour, INetworkRunnerCallbacks
{
    //Action Map type/asset is Hardcoded for now.
    private SimpleControls _playerActionMap;
    private float localX,localZ;

    public void Start() //this has to be Start, you can't do it on OnEnable :(
    {
        Runner?.AddCallbacks(this);
        _playerActionMap = new SimpleControls();
        _playerActionMap.gameplay.Enable();
    }
    
    public void OnDisable()
    {
        Runner?.RemoveCallbacks(this);
        _playerActionMap.gameplay.Disable();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        localX = Input.GetAxis("Horizontal");
        localX = Input.GetAxis("Vertical");
        
        localX = _playerActionMap.gameplay.move.ReadValue<Vector2>().x;
        localZ = _playerActionMap.gameplay.move.ReadValue<Vector2>().y;
        
        var netData = new NetData();
        netData.direction.Set(localX, 0, localZ);
        input.Set(netData);
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetData netData))
        {
            transform.Translate(netData.direction.normalized * 5 * Runner.DeltaTime);
        }
    }
    
    #region other callbacks
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
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