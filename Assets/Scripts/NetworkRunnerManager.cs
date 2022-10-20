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
        _runner = GetComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        _netSceneManager = GetComponent<NetworkSceneManagerDefault>();
    }
    
    private void OnGUI()
    {
        if (_runner.State == NetworkRunner.States.Shutdown)
        {
            if (GUI.Button(new Rect(0, 0, 150, 30), "Start Game"))
                StartGame(0);
        }
    }

    async void StartGame(int sceneIndex)
    {
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "DefaultX",
            Scene = 0, 
            SceneManager =  _netSceneManager
        });
        print("Game started");
        _runner.SetActiveScene(sceneIndex);
    }
}

