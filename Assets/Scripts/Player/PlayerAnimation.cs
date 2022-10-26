using Fusion;
using Fusion.Editor;
using UnityEngine;

namespace Born.Player
{
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
    [RequireComponent(typeof(NetworkMecanimAnimator))]
    public class PlayerAnimation : NetworkBehaviour
    {
        private NetworkMecanimAnimator networkAnimator;
        private Animator localAnimator;

        public override void Spawned()
        {
            base.Spawned();
            networkAnimator = GetComponent<NetworkMecanimAnimator>();
            localAnimator = GetComponentInChildren<Animator>();
            PlayerMovement.OnAnim += Handle_OnAnim;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            PlayerMovement.OnAnim -= Handle_OnAnim;
        }

        private void Handle_OnAnim(Anim anim)
        {
            string animName = anim.GetDescription();
            if (Object.HasInputAuthority)
            {
                localAnimator.SetTrigger(animName);
            }
            else
            {
                networkAnimator.SetTrigger(animName, true);
            }
        }
    }
}