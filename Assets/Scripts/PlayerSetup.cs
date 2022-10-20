using System.Linq;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Color[] playerColors;
    private void Start()
    {
        if (Runner == null) return;
        
        //ColorPlayer
        if (playerColors.Length == 0) return;
        int colorIndex = Runner.ActivePlayers.Count() - 1;
        GetComponentInChildren<Renderer>().material.color = playerColors[colorIndex];

        if (Object.HasInputAuthority)
        {
            gameObject.name += " - Mine";
        }
        else
        {
            gameObject.name += " - Other";
        }
    }
}