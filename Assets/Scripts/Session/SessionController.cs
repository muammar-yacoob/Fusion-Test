using System.Collections.Generic;
using Born.Core;
using Fusion;
using UnityEditor;
using UnityEngine;

namespace Born.Session
{
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
    public class SessionController : NetworkBehaviour
    {
        [Networked] public NetworkButtons ButtonsPrevious { get; set; }

        private Chapter currentChapter = Chapter.Hangar;

        

        public override void FixedUpdateNetwork()
        {
            if (GetInput<NetworkInputData>(out var netInput) == false) return;

            var pressed = netInput.Buttons.GetPressed(ButtonsPrevious);
            var released = netInput.Buttons.GetReleased(ButtonsPrevious);

            ButtonsPrevious = netInput.Buttons;

            if (!(pressed.IsSet(MyButtons.NextChapter) || pressed.IsSet(MyButtons.PreviousChapter))) return;

            if (pressed.IsSet(MyButtons.NextChapter))
            {
                currentChapter = currentChapter.GetNext();
            }

            if (pressed.IsSet(MyButtons.PreviousChapter))
            {
                currentChapter = currentChapter.GetPrevious();
            }

            print($"Setting Chapter to: {currentChapter.GetName()}");

            var customProps = new Dictionary<string, SessionProperty>()
            {
                { nameof(Chapter), (int)currentChapter },
                { nameof(Lesson), (int)Lesson.Intro }
            };

            Runner.SessionInfo.UpdateCustomProperties(customProps);
        }

        #region Editor

        [ContextMenu("Goto Next Chapter")]
        private void GotoNextChapter()
        {
            currentChapter = currentChapter.GetNext();

            var customProps = new Dictionary<string, SessionProperty>()
            {
                { nameof(Chapter), (int)currentChapter },
                { nameof(Lesson), (int)Lesson.Intro },
                { "SessionName", Runner.SessionInfo.Name },
            };

            Runner.SessionInfo.UpdateCustomProperties(customProps);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(SessionController))]
        public class SessionControllerCustomInspector : Editor
        {
            public override void OnInspectorGUI()
            {
                var myTarget = (SessionController)target;
                if (GUILayout.Button("Next Chapter"))
                    myTarget.GotoNextChapter();

                DrawDefaultInspector();
                EditorGUILayout.HelpBox("Iterates over session stages", MessageType.Info);
            }
        }
#endif
        #endregion
    }
}