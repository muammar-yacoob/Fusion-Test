using System;
using Fusion;
using TMPro;
using UnityEngine;

namespace Born.Player
{
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
    public class PlayerName : NetworkBehaviour
    {
        [SerializeField] private TMP_Text nickNameText;
        [Networked(OnChanged = nameof(OnNickNameChanged))]
        //[HideInInspector]
        public NetworkString<_16> nickName { get; set; }

        //Set Default nickName
        public override void Spawned()
        {
            if (Object.HasInputAuthority) // My player
            {
                nickName = Environment.UserName;
            }
            else
            {
                nickName = Runner.UserId;
            }
        }

        static void OnNickNameChanged(Changed<PlayerName> changed)=> changed.Behaviour.OnNickNameChanged();
        private void OnNickNameChanged()
        {
            //Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
            if(nickNameText != null)
                nickNameText.text = nickName.ToString();
        }
    }
}