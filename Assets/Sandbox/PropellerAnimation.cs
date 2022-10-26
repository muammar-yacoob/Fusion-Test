using Born.Core;
using Fusion;
using UnityEngine;

namespace Born
{
    [RequireComponent(typeof(NetworkMecanimAnimator))]
    public class PropellerAnimation : NetworkBehaviour
    {
        [Networked] public NetworkButtons ButtonsPrevious { get; set; }
        private NetworkMecanimAnimator networkAnimator;
        private static readonly int SpinHash = Animator.StringToHash("Spin");

        public override void Spawned()
        {
            base.Spawned();
            networkAnimator = GetComponent<NetworkMecanimAnimator>();
        }

        public override void FixedUpdateNetwork()
        {
                if (GetInput<NetworkInputData>(out var netInput) == false) return;

                var pressed = netInput.Buttons.GetPressed(ButtonsPrevious);
                var released = netInput.Buttons.GetReleased(ButtonsPrevious);
                ButtonsPrevious = netInput.Buttons;

                if (pressed.IsSet(MyButtons.Jump))
                {
                    networkAnimator.SetTrigger(SpinHash, true);
                }
        }
    }
}