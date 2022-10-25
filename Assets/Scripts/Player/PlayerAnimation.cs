using Fusion;
using Fusion.Editor;
using UnityEngine;

namespace Born.Player
{
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
    public class PlayerAnimation : NetworkBehaviour
    {
        private NetworkMecanimAnimator networkAnimator;

        private void Awake()
        {
            networkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();
        }

        private void OnEnable()
        {
            PlayerMovement.OnAnim += Handle_OnAnim;
        }
    
        private void OnDisable()
        {
            PlayerMovement.OnAnim -= Handle_OnAnim;
        }

        private void Handle_OnAnim(Anim anim)
        {
            if(!Object.HasInputAuthority) return;
            
            string animName = anim.GetDescription();
            networkAnimator.SetTrigger(animName);
        }
    }
}