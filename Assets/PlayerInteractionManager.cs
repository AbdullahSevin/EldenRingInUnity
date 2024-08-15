using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager player;
        private List<Interactable> currentInteractableActions; // DO NOT USE SERIALIZE FILD IF USING UNITY 2022 3 11F1 IT CAUSES BUG

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            currentInteractableActions = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            if (!player.IsOwner)
            {
                return;
            }

            if (!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
            {
                CheckForInteractable();
            }

            // IF OUR UI MENU IS NOT OPEN, AND WE DONT HAVE A POP UP(CURENT INTERACTION MESSAGE) CHECK FOR INTERACTABLES
        }

        public void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0)
            {
                return;
            }

            if (currentInteractableActions[0] == null)
            {
                currentInteractableActions.RemoveAt(0); // IF THE CURRENT INTERACTABLE ITEM AT POSIION 0 BECOMES NULL (REMOVED FROM GAME) , WE REMOVE POSITION 0 FROM THE LIST
                return;
            }

            //  IF WE HAVE AN INTERACTABLE ACTION AND HAVE NOT NOTIFIED OUR PLAYER, WE DO SO HERE
            if (currentInteractableActions[0] != null)
            {
                // Debug.Log($"{currentInteractableActions[0].interactableText}");
                // Debug.Log($"{PlayerUIManager.instance.name}");
                // Debug.Log($"{PlayerUIManager.instance.playerUIPopUpManager.name}");

                //if (PlayerUIManager.instance.playerUIPopUpManager == null)
                //{
                //    Debug.Log("why am I null"); // you are null because you are not active :D
                //    return;
                //}
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
            }

        }

        private void RefreshInteractionList()
        {
            for (int i = currentInteractableActions.Count - 1; i > -1; i--)
            {
                if (currentInteractableActions[i] == null)
                {
                    currentInteractableActions.RemoveAt(i);
                }
            }
        }

        public void AddInteractionToList(Interactable interactableObject)
        {
            RefreshInteractionList();

            if (!currentInteractableActions.Contains(interactableObject))
            {
                currentInteractableActions.Add(interactableObject);
            }
        }

        public void RemoveInteractionFromList(Interactable interactableObject)
        {
            if (currentInteractableActions.Contains(interactableObject))
            {
                currentInteractableActions.Remove(interactableObject);
            }

            RefreshInteractionList();
        }

        public void Interact()
        {
            if (currentInteractableActions.Count == 0)
            {
                return;
            }
            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractionList();
            }
        }

        







    }
}

