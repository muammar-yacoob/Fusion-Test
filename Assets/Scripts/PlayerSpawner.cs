using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }
}