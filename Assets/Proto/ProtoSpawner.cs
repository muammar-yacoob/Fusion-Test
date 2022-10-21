using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtoSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
  [SerializeField] private NetworkPrefabRef _playerPrefab;
  
  private NetworkRunner _runner;
  private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new ();

  private void OnGUI()
  {
    if (_runner != null) return;
      if (GUI.Button(new Rect(0,0,200,40), "Host or Client"))
      {
        StartGame(GameMode.AutoHostOrClient);
      }
  }
  
  async void StartGame(GameMode mode)
  {
    _runner = gameObject.AddComponent<NetworkRunner>();
    _runner.ProvideInput = true;

    await _runner.StartGame(new StartGameArgs()
    {
      GameMode = mode,
      SessionName = "TestRoom",
      Scene = SceneManager.GetActiveScene().buildIndex,
      SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
    });}
  

  public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
  {
    if (runner.IsServer)
    {
      NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.up *2, Quaternion.identity, player);
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
}