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

        #region UI
        private void OnGUI()
        {
            DrawHud();
            DrawInstructions();
        }

        private void DrawHud()
        {
            if (runner != null) return;

            sessionName = GUI.TextField(new Rect(10, 10, 120, 20), sessionName, 10);

            if (sessionName.IsNullOrEmpty()) return;
            if (GUI.Button(new Rect(10, 33, 120, 20), $"Start/Join {sessionName}"))
            {
                Play(GameMode.AutoHostOrClient, Chapter.Hanger, Lesson.Intro);
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
        
        async void Play(GameMode mode, Chapter chapter, Lesson lesson)
        {
            runner = gameObject.AddComponent<NetworkRunner>();
            //Join a lobby for the machmaker
            var result = await runner.JoinSessionLobby(SessionLobby.Custom,LobbyName);

            if (!result.Ok)
            {
                Debug.LogError($"Failed to Join Lobby: {result.ShutdownReason}");
                return;
            }

            print($"Lobby {LobbyName} joined");
            runner.ProvideInput = true;
            
            var customProps = new Dictionary<string, SessionProperty>()
            {
                { nameof(Chapter), (int)Chapter.Hanger },
                { nameof(Lesson), (int)Lesson.Intro }
            };

            var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            result = await runner.StartGame(new StartGameArgs()
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
                return;
            }
            
            print($"Session {sessionName} joined");
        }
    }
}