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
            StartGame(GameMode.AutoHostOrClient, Map.Airport, Stage.Intro );
        }
    }

    private void DrawInstructions()
    {
        if (_runner == null) return;

        string instructions = "CTRL: Color, SpaceBar: Jump";
        Vector2 lableSize = new GUIStyle().CalcSize(new GUIContent(instructions));
        Rect r = new Rect(10, Screen.height - 50, lableSize.x*1.1f, lableSize.y*1.5f);
        GUI.contentColor = Color.white;
        GUI.Box (r, instructions);
    }

    async void StartGame(GameMode mode, Map map, Stage stage)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        
        var customProps = new Dictionary<string, SessionProperty>() {
            { nameof(Map), (int)Map.Airport },
            { nameof(Stage), (int)Stage.Intro }
        };  

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = _roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 4,
            SessionProperties =  customProps
        });}
}
public enum Map{Airport, EngineRoom, Garage }
public enum Stage{Intro, IDG, APU, DC, GPU }