using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class ProtoMenu : MonoBehaviour
{
    private NetworkRunner _runner;
    private string _roomName = "Default";

    private void OnGUI()
    {
        DrawHud();
        DrawInstructions();
    }

    private void DrawHud()
    {
        if (_runner != null) return;
        
        _roomName = GUI.TextField(new Rect(10, 10, 120, 20), _roomName, 10);
        
        if(_roomName.IsNullOrEmpty()) return;
        if (GUI.Button(new Rect(10,33,120,20), $"Start/Join {_roomName}") )
        {
            StartGame(GameMode.AutoHostOrClient);
        }
    }

    private void DrawInstructions()
    {
        if (_runner == null) return;
        GUI.contentColor = Color.blue;
        GUI.Label(new Rect(10, Screen.height - 50, Screen.width-10, 20),"CTRL: Color, SpaceBar: Jump");
    }

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = _roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });}
}