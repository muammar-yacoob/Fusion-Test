using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    [SerializeField] private TMP_Text nickNameText;
    [Networked(OnChanged = nameof(OnNickNameChanged))]
    [HideInInspector] public NetworkString<_16> nickName { get; set; }

    //Set Default nickName
    public override void Spawned() => nickNameText.text = Environment.UserName;

    static void OnNickNameChanged(Changed<PlayerName> changed)=> changed.Behaviour.OnNickNameChanged();
    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
        nickNameText.text = nickName.ToString();
    }
}