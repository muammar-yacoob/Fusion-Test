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
        if (msgQueue.Count >= bufferSize)
            msgQueue.Dequeue();
        
        msgQueue.Enqueue(messageData);
        screenMessage = messageData.Message;
        Debug.Log($"{info.Source}:{messageData.ToString()}");
    }

    private void OnGUI()
    {
        
         if (msgQueue.Count == 0) return;   
        int i = 0;
        foreach (var msg in msgQueue)
        {
            GUI.color = msg.Color;
            GUI.Label(new Rect(10, 15*i++, Screen.width / 3, 20), msg.Message);
        }
    }
}

public class MessageData
{
    public string Message { get; }
    public Color Color { get; }

    public MessageData(string message, Color color)
    {
        Message = message;
        Color = color;
    }

    public override string ToString() => this.Message.ForeColor(this.Color);
}

public static class Utils
{
    public static string ForeColor(this string original, Color color)
    {
        var colorHex = ColorUtility.ToHtmlStringRGB(color);
        var coloredString = $"<color=#{colorHex}>{original}</color>";
        return original;
    }
}