using System;
using System.Collections.Generic;
using Born.Core;
using Born.UI;
using Fusion;
using UnityEditor;
using UnityEngine;

namespace Born.Player
{
    public class SessionData : NetworkBehaviour
    {
        private int index = 0;
        private int max = 0;
    
        //Allowed Types: https://doc.photonengine.com/en-us/fusion/current/manual/network-object/network-behaviour#allowed_types
        [Networked(OnChanged = nameof(OnSessionPropertyChanged))]
        private NetworkDictionary<string, SessionProperty> CustomProps{ get; set; }
        private void Awake() => max = Enum.GetNames(typeof(Chapter)).Length;

        [ContextMenu("Next Stage")]
        public void GotoNextStage()
        {
            CustomProps.Add(nameof(Lesson), index);
        
            index = index >= max ? 0 : index;
            index++;
        
            Debug.Log($"Moving stage to: {((Lesson)index).GetDescription()}");
            Runner.SessionInfo.UpdateCustomProperties(CustomProps);
        }

        public static void OnSessionPropertyChanged(Changed<SessionData> changed) => changed.Behaviour.LogSessionChanges();
        private void LogSessionChanges()
        {
            if (Runner.SessionInfo.Properties.TryGetValue(nameof(Lesson), out var chapter) && chapter.IsInt)
            {
                var currentChapter = (Lesson)chapter.PropertyValue;
                Debug.Log($"Current Stage:{currentChapter.GetDescription()}");
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SessionData))]
    public class SessionControllerCustomInspector : Editor 
    {
        public override void OnInspectorGUI()
        {
            SessionData myTarget = (SessionData)target;
            if(GUILayout.Button("Next Stage"))
                myTarget.GotoNextStage();
        
            DrawDefaultInspector();
            EditorGUILayout.HelpBox("Iterates over session stages", MessageType.Info);
        }
    }
#endif
}