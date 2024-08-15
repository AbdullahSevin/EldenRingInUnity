using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ParrelSync.NonCore;


namespace AS
{
    public class Interactable : MonoBehaviour
    {
        public string interactableText; // TEXT PROMPT WHEN ENTERING THE INTERACTION COLLIDER (PICKUP ITEM, PULL LEVERL ETC)
        [SerializeField] protected Collider interactableCollider; // COLLIDER THAT CHECKS FOR PLAYERINTERACTION
        [SerializeField] protected bool hostOnlyInteractable = true;  //  WHEN ENABLED OBJECT CANT BE INTERACTED WITH BY CO-OP PLAYERS

        protected virtual void Awake()
        {
            //  CHECK IF ITS NULL, IN SOME CASES YOU MAY WANT TO MANUALLY AIGN A COLLIDER AS A CHILD OBJECT (DEPENDING ON INTEERACTABLE)
            if (interactableCollider == null)
            {
                interactableCollider = GetComponent<Collider>();
            }


        }

        protected virtual void Start()
        {

        }

        public virtual void Interact(PlayerManager player)
        {
            Debug.Log("YOU HAVE INTERACTED!");

            if (player.IsOwner)
            {
                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                {
                    return;
                }

                if (!player.IsOwner)
                {
                    return;
                }
                // PASS THE INTERACTION TO THE PLAYER
                player.playerInteractionManager.AddInteractionToList(this);
            }
        }

        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                {
                    return;
                }

                if (!player.IsOwner)
                {
                    return;
                }

                // REMOVE THE INTERACTION FROM PLAYER
                player.playerInteractionManager.RemoveInteractionFromList(this);

                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            }
        }





    }
}

