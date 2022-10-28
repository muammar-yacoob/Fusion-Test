using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Born.Session;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

namespace Born.UI
{
    public class ProtoMenu : MonoBehaviour
    {
        private NetworkRunner _runner;
        private const string LobbyName = "LOB";
        private string _roomName = "Default";

        private void Start()
        {
        }

        #region UI
        private void OnGUI()
        {
            DrawHud();
            DrawInstructions();
        }

        private void DrawHud()
        {
            if (_runner != null) return;

            _roomName = GUI.TextField(new Rect(10, 10, 120, 20), _roomName, 10);

            if (_roomName.IsNullOrEmpty()) return;
            if (GUI.Button(new Rect(10, 33, 120, 20), $"Start/Join {_roomName}"))
            {
                Play(GameMode.AutoHostOrClient, Chapter.Hanger, Lesson.Intro);
            }
        }

        private void DrawInstructions()
        {
            if (_runner == null) return;

            string instructions = "CTRL: Color, SpaceBar: Jump";
            Vector2 lableSize = new GUIStyle().CalcSize(new GUIContent(instructions));
            Rect r = new Rect(10, Screen.height - 50, lableSize.x * 1.1f, lableSize.y * 1.5f);
            GUI.contentColor = Color.white;
            GUI.Box(r, instructions);
        }
        #endregion
        
        public async Task JoinLobby(NetworkRunner runner) {

            var result = await runner.JoinSessionLobby(SessionLobby.ClientServer);

            if (!result.Ok)
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
                return;
            }
        }

        async void Play(GameMode mode, Chapter chapter, Lesson lesson)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            //Join a lobby for the machmaker
            await _runner.JoinSessionLobby(SessionLobby.Custom,LobbyName);

            _runner.ProvideInput = true;
            
            var customProps = new Dictionary<string, SessionProperty>()
            {
                { nameof(Chapter), (int)Chapter.Hanger },
                { nameof(Lesson), (int)Lesson.Intro }
            };

            var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            var result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                CustomLobbyName = LobbyName,
                SessionName = _roomName,
                Scene = sceneIndex,
                SceneManager = sceneManager,
                PlayerCount = 4,
                SessionProperties = customProps
            });

            if (!result.Ok)
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            }
        }
    }
}