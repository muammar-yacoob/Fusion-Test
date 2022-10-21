using Fusion;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    private string screenMessage;

    private void OnEnable()
    {
        PlayerNetworkSetup.OnPlayerJoined += PrintGUI;
    }

    private void OnDestroy()
    {
        PlayerNetworkSetup.OnPlayerJoined -= PrintGUI;
    }

    private void PrintGUI(string message) => RPC_PrintGUI(message);
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_PrintGUI(string message, RpcInfo info = default)
    {
        if (info.IsInvokeLocal)
        {
            screenMessage = "Welcome aboard!";
        }
        else
        {
            screenMessage = message;
        }
        Debug.Log($"{info.Source}:{message}");
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.white;
        GUI.Label(new Rect(10, 10, Screen.width-10, 20),screenMessage);
    }
}