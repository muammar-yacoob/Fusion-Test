using Fusion;
using UnityEngine;

namespace Born.Core
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 Direction;
        public NetworkButtons Buttons;

    }

    public enum MyButtons {
        Jump = 0,
        Color = 1,
        NextChapter = 2,
        PreviousChapter = 3,
    }
}