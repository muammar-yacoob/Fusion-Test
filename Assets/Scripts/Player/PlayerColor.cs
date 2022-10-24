using System.Linq;
using Born.Core;
using Fusion;
using UnityEngine;

namespace Born.Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerColor : NetworkBehaviour
    {
        [SerializeField] private Color[] playerColors;
        private Material mat;
        private Color targetColor;

        [Networked(OnChanged = nameof(OnColorChanged))] int colorIndex { get; set; }

        public override void Spawned()
        {
            mat = GetComponentInChildren<Renderer>().material;
            colorIndex = Runner.ActivePlayers.Count() - 1; //Trigers Property OnChange for other players
        
            //Sets color locally
            SetColor();
        }
    
        [Networked] public NetworkButtons ButtonsPrevious { get; set; }
        public override void FixedUpdateNetwork()
        {
            if (GetInput<NetworkInputData>(out var netInput) == false) return;
        
            var pressed = netInput.Buttons.GetPressed(ButtonsPrevious);
            var released = netInput.Buttons.GetReleased(ButtonsPrevious);

            ButtonsPrevious = netInput.Buttons;

            if (pressed.IsSet(MyButtons.Color))
            {
                colorIndex++;
                SetColor();
            }
        }
    
        public override void Render()
        {
            if(mat.color == targetColor) return;
        
            if (Object.HasInputAuthority)
            {
                mat.color = targetColor;
            }
            else
            {
                mat.color = Color.Lerp(mat.color, targetColor, 2* Time.deltaTime);
            }
        }

        public static void OnColorChanged(Changed<PlayerColor> changed) => changed.Behaviour.SetColor();

        private void SetColor()
        {
            colorIndex = (colorIndex >=  playerColors.Length) ? 0 : colorIndex;
            targetColor = playerColors[colorIndex];
        }
    }
}