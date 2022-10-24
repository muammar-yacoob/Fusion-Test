using Fusion;
using Fusion.Editor;
using UnityEngine;

namespace Born.Player
{
    public class PlayerAnimation : NetworkBehaviour
    {
        private Animator animController;

        private void Awake()
        {
            animController = GetComponentInChildren<Animator>();
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
            animController.SetTrigger(animName);
        }
    }
}