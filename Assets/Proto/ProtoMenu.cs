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
}