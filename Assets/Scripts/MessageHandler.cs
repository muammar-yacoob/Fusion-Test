using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    private string screenMessage;
    private Queue<MessageData> msgQueue = new ();
    private int bufferSize = 3;
    private void OnEnable()
    {
        PlayerNetworkSetup.OnPlayerJoined += PrintGUI;
    }

    private void OnDestroy()
    {
        PlayerNetworkSetup.OnPlayerJoined -= PrintGUI;
    }

    private void PrintGUI(MessageData messageData) => RPC_PrintGUI(messageData);
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_PrintGUI(MessageData messageData, RpcInfo info = default)
    {
        if (msgQueue.Count > 3)
            msgQueue.Dequeue();
        
        msgQueue.Enqueue(messageData);
        screenMessage = messageData.Message;
        Debug.Log($"{info.Source}:{messageData.Message}");
    }

    private void OnGUI()
    {
        
         if (msgQueue.Count == 0) return;   
        int i = 0;
        foreach (var msg in msgQueue)
        {
            GUI.color = msg.Color;
            GUI.Label(new Rect(10, 50*i++, Screen.width / 3, 20), msg.Message);
        }
    }
}

public class MessageData
{
    public string Message { get; private set; }
    public Color Color { get; private set; }

    public MessageData(string message, Color color)
    {
        Message = message;
        Color = color;
    }
}