using UnityEngine;
using UHFPS.Tools;

namespace UHFPS.Runtime
{
    public class ItemsContainer : InventoryContainer, IInteractStart
    {
        public Animator Animator;
        public string OpenParameter = "Open";

        public SoundClip OpenSound;
        public SoundClip CloseSound;
        public bool CloseWithAnimation;

        public void InteractStart()
        {
            inventory.OpenContainer(this);

            GameTools.PlayOneShot3D(transform.position, OpenSound);
            if(Animator != null) Animator.SetBool(OpenParameter, true);
        }

        public override void OnStorageClose()
        {
            if(CloseWithAnimation) GameTools.PlayOneShot3D(transform.position, CloseSound);
            if (Animator != null) Animator.SetBool(OpenParameter, false);
        }

        public void PlayCloseSound()
        {
            GameTools.PlayOneShot3D(transform.position, CloseSound);
        }
    }
}