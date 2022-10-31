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
        private NetworkRunner runner;
        private const string LobbyName = "LOB";
        private string sessionName = "Default";
        private bool lobbyJoined;
        private bool sessionStarted;

        
        private void Awake()
        {
            runner = gameObject.AddComponent<NetworkRunner>();
            StartLobby();
        }
        
        #region UI
        private void OnGUI()
        {
            DrawHud();
            DrawInstructions();
        }

        private void DrawHud()
        {

            if (!lobbyJoined)
            {
                GUI.Label(new Rect(10, 10, 120, 20), "Connecting...");
                return;
            }
            if (sessionStarted) return;

            sessionName = GUI.TextField(new Rect(10, 10, 120, 20), sessionName, 10);

            if (sessionName.IsNullOrEmpty()) return;
            if (GUI.Button(new Rect(10, 33, 120, 20), $"Start/Join {sessionName}"))
            {
                StartSession(GameMode.AutoHostOrClient, Chapter.Hangar, Lesson.Intro);
            }
        }

        private void DrawInstructions()
        {
            if (runner == null) return;

            string instructions = "CTRL: Color, SpaceBar: Jump, ]&[: to switch Chapters";
            Vector2 lableSize = new GUIStyle().CalcSize(new GUIContent(instructions));
            Rect r = new Rect(10, Screen.height - 50, lableSize.x * 1.1f, lableSize.y * 1.5f);
            GUI.contentColor = Color.white;
            GUI.Box(r, instructions);
        }
        #endregion



        async void StartLobby()
        {
            if (runner == null)
            {
                Debug.LogError("No Runner Detected");
                return;
            }
            
            //Join a lobby for the machmaker
            var result = await runner.JoinSessionLobby(SessionLobby.Custom,LobbyName);
            
            if (!result.Ok)
            {
                Debug.LogError($"Failed to Join Lobby: {result.ShutdownReason}");
                return;
            }

            lobbyJoined = true;
            print($"Lobby {LobbyName} joined");
            runner.ProvideInput = true;
        }
        async void StartSession(GameMode mode, Chapter chapter, Lesson lesson)
        {
            var customProps = new Dictionary<string, SessionProperty>()
            {
                { nameof(Chapter), (int)Chapter.Hangar },
                { nameof(Lesson), (int)Lesson.Intro }
            };

            var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            var result = await runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                CustomLobbyName = LobbyName,
                SessionName = sessionName,
                Scene = sceneIndex,
                SceneManager = sceneManager,
                PlayerCount = 4,
                SessionProperties = customProps
            });

            if (!result.Ok)
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
                sessionStarted = false;
                return;
            }

            sessionStarted = true;
            print($"Session {sessionName} joined");
        }
    }
}