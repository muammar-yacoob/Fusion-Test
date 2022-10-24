using System;
using System.Collections.Generic;
using Born.Core;
using Born.UI;
using Fusion;
using Fusion.Sockets;
using UnityEditor;
using UnityEngine;

namespace Born.Player
{
    public class SessionController : NetworkBehaviour
    {
        private int index = 0;
        private int max = 0;
    
        //[Networked(OnChanged = nameof(OnSessionPropertyChanged))] //can't use on Dictionary
        private Dictionary<string, SessionProperty> CustomProps = new();
        private void Awake() => max = Enum.GetNames(typeof(Map)).Length;

        [ContextMenu("Next Stage")]
        public void GotoNextStage()
        {
            CustomProps.Add(nameof(Stage), index);
        
            index = index >= max ? 0 : index;
            index++;
        
            Debug.Log($"Moving stage to: {((Stage)index).GetDescription()}");
            Runner.SessionInfo.UpdateCustomProperties(CustomProps);
        }

        public static void OnSessionPropertyChanged(Changed<SessionController> changed) => changed.Behaviour.LogSessionChanges();
        private void LogSessionChanges()
        {
            if (Runner.SessionInfo.Properties.TryGetValue(nameof(Stage), out var sessionStage) && sessionStage.IsInt)
            {
                var currentStage = (Stage)sessionStage.PropertyValue;
                Debug.Log($"Current Stage:{currentStage.GetDescription()}");
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SessionController))]
    public class SessionControllerCustomInspector : Editor 
    {
        public override void OnInspectorGUI()
        {
            SessionController myTarget = (SessionController)target;
            if(GUILayout.Button("Next Stage"))
                myTarget.GotoNextStage();
        
            DrawDefaultInspector();
            EditorGUILayout.HelpBox("Iterates over session stages", MessageType.Info);
        }
    }
#endif
}