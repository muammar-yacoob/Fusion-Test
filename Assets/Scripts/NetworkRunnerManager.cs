using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkRunner))]
[RequireComponent(typeof(NetworkSceneManagerDefault))]
public class NetworkRunnerManager : MonoBehaviour
{
    private NetworkRunner _runner;
    private NetworkSceneManagerDefault _netSceneManager;

    private void Awake()
    {
        _runner = GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        _netSceneManager = GetComponent<NetworkSceneManagerDefault>();
    }
    
    private void OnGUI()
    {
        if (_runner.State == NetworkRunner.States.Shutdown)
        {
            if (GUI.Button(new Rect(10, 10, 150, 30), "Start Game"))
                StartGame(GameMode.AutoHostOrClient,0);
        }
    }

    async void StartGame(GameMode gameMode, int sceneIndex)
    {
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = "BornDefaultRoom",
            Scene = 0, 
            SceneManager =  _netSceneManager
        });
        int playerCount = _runner.SessionInfo.PlayerCount;
        string roomName = _runner.SessionInfo.Name;
        print($"Game started. {playerCount} Players in {roomName}");
        //_runner.SetActiveScene(sceneIndex);
    }
}

