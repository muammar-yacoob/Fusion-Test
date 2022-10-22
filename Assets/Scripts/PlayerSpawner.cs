using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
  [SerializeField] private NetworkPrefabRef _playerPrefab;
  private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new ();
  public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
  {
    if (runner.IsServer)
    {
      NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.up *5, transform.rotation, player);
      _spawnedCharacters.Add(player, networkPlayerObject);
    }
  }

  public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
  {
    if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
    {
      runner.Despawn(networkObject);
      _spawnedCharacters.Remove(player);
    }
  }
  
  #region other callbacks
  public void OnConnectedToServer(NetworkRunner runner) { }
  public void OnInput(NetworkRunner runner, NetworkInput input) { }
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