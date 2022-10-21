using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtoMenu : MonoBehaviour
{
    private NetworkRunner _runner;
    private void OnGUI()
    {
        if (_runner != null) return;
        if (GUI.Button(new Rect(10,10,200,30), "Start Game"))
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
            SessionName = "Default",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });}
}