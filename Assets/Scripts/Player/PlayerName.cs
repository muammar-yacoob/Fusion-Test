using System;
using Fusion;
using TMPro;
using UnityEngine;

namespace Born.Player
{
    public class PlayerName : NetworkBehaviour
    {
        [SerializeField] private TMP_Text nickNameText;
        [Networked(OnChanged = nameof(OnNickNameChanged))]
        [HideInInspector] public NetworkString<_16> nickName { get; set; }

        //Set Default nickName
        public override void Spawned()
        {
            if (Object.HasInputAuthority) // My player
            {
                nickNameText.text = Environment.UserName;
            }
            else
            {
                nickNameText.text = Runner.UserId;
            }
        }

        static void OnNickNameChanged(Changed<PlayerName> changed)=> changed.Behaviour.OnNickNameChanged();
        private void OnNickNameChanged()
        {
            Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
            nickNameText.text = nickName.ToString();
        }
    }
}