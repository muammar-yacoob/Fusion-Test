using System.Linq;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerColor : NetworkBehaviour
{
    [SerializeField] private Color[] playerColors;
    private Material mat;

    //[Networked(OnChanged = nameof(OnColorChanged))]
    public int colorIndex { get; set; }

    private void Start()
    {
        mat = GetComponentInChildren<Renderer>().material;
        colorIndex = Runner.ActivePlayers.Count() - 1;
        mat.color = playerColors[colorIndex];
    }

    private void OnColorChanged()
    {
        mat.color = playerColors[colorIndex];
    }
}