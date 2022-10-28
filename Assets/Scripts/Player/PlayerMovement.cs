using System;
using Born.Core;
using Fusion;
using UnityEngine;

namespace Born.Player
{
    [RequireComponent(typeof(NetworkCharacterControllerPrototype))]
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
    public class PlayerMovement : NetworkBehaviour
    {
        private NetworkCharacterControllerPrototype _cc;
        private float jumpHight = 15;
        public static event Action<Anim> OnAnim = delegate { };
        [Networked] public NetworkButtons ButtonsPrevious { get; set; }

        private void Awake() => _cc = GetComponent<NetworkCharacterControllerPrototype>();

        public override void FixedUpdateNetwork()
        {
            
            if (GetInput<NetworkInputData>(out var netInput) == false) return;

            //netInput.Direction.Normalize();
            _cc.Move(5 * netInput.Direction.normalized * Runner.DeltaTime);

            DoJump(netInput);
            ResetIfDropped();
        }

        private void DoJump(NetworkInputData netInput)
        {
            var pressed = netInput.Buttons.GetPressed(ButtonsPrevious);
            var released = netInput.Buttons.GetReleased(ButtonsPrevious);

            ButtonsPrevious = netInput.Buttons;

            if (pressed.IsSet(MyButtons.Jump))
            {
                //Jump
                if (!_cc.IsGrounded || _cc.Velocity.y > 0) return;
                _cc.Velocity = Vector3.up * jumpHight;

                if (Object.HasInputAuthority)
                {
                    RPC_Jump((byte)Anim.Jump);
                }
            }
        }
        
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
        private void RPC_Jump(byte byteAnim,RpcInfo info = default){
            var anim = (Anim)byteAnim;
            print($"{info.Source}:{anim.GetDescription()}");
            
            OnAnim?.Invoke(anim);
        }
    
        private void ResetIfDropped()
        {
            if (_cc.transform.position.y < -50)
                _cc.transform.position = Vector3.up * 5;
        }
    }
    public enum Anim{Jump,Damage,Talk,Die}
}