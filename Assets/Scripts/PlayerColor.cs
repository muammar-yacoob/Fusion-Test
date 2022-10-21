using System;
using System.Linq;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerColor : NetworkBehaviour
{
    [SerializeField] private Color[] playerColors;
    private Material mat;

    [Networked(OnChanged = nameof(OnColorChanged))]
    int colorIndex { get; set; }

    private void Awake()=> mat = GetComponentInChildren<Renderer>().material;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //data.colorButton;
        }
    }

    private void Start()
    {
        colorIndex = Runner.ActivePlayers.Count() - 1; //Trigers Property OnChange
        //Sets color locally
        SetColor();
    }

    public static void OnColorChanged(Changed<PlayerColor> changed)
    {
        changed.LoadNew();
        changed.Behaviour.SetColor();
    }

    private void SetColor()
    {
        mat.color = playerColors[colorIndex];
    }
}