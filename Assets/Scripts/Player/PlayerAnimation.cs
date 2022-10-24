using System;
using UnityEngine;

namespace Born.Player
{
    public class PlayerAnimation : MonoBehaviour
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
            string animName = Enum.GetName(typeof(Anim), anim);
            animController.SetTrigger(animName);
        }
    }
}