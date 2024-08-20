using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace AS
{
    public class SiteOfGraceInteractable : Interactable
    {

        [Header("Site Of Grace Info")]
        [SerializeField] int siteOfGraceID;
        public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("VFX")]
        [SerializeField] GameObject activatedParticles;

        [Header("Interaction Text")]
        [SerializeField] string unactivatedInteractionText = "Restore Site Of Grace";
        [SerializeField] string activatedInteractionText = "Rest";


        protected override void Start()
        {
            base.Start();

            if (IsOwner)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                {
                    isActivated.Value = WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID);
                }
                else
                {
                    isActivated.Value = false;
                }

            }

            if (isActivated.Value)
            {
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }


        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // IF WE JOIN WHEN THE STATUS HAS ALREADY CHANGED, WE FORCE THE ONCHANGE FUNCTION TO RUN HERE UPON JOINING
            if (!IsOwner)
            {
                OnIsActivatedChanged(false, isActivated.Value);
            }

            isActivated.OnValueChanged += OnIsActivatedChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActivated.OnValueChanged -= OnIsActivatedChanged;
        }

        private void RestoreSiteOfGrace(PlayerManager player)
        {
            isActivated.Value = true;

            //  IF OUR SAVE FILE CONTAINS INFO ON THIS SITE OF GRACE, REMOVE IT
            if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
            {
                WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);
            }
            //  THEN RE-AD IT WITH THE VALUE OF "TRUE" (IS ACTIVATED)
            WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true);

            player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);
            // HIDE WEAPON MODELS WHILS PLAYING ANIMATION IF YOU DESIRE

            PlayerUIManager.instance.playerUIPopUpManager.SendGraceRestoredPopUp("SITE OF GRACE RESTORED");

            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
        }

        private void RestAtSiteOfGrace(PlayerManager player)
        {

            Debug.Log("RESTING");

            //  TEMPORARY CODE SECTION
            interactableCollider.enabled = true;  //  TEMPORARILY RE-ANABLING THE COLLIDER HERE UNTIL WE ADD THE MENU SO YOU CAN RESPAWN MONSTERS INDEFINITELY 
            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
            player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;
            // REFILL FLASKS (TO DO)
            // UPDATE/FORCE MOVE QUEST CHARACTERS (TO DO)

            WorldAIManager.instance.ResetAllCharacters();

        }

        private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(2); // THIS SHOULD GIVE ENOUGH TIME FOR THE ANIMATION TO PLAY AND THE POP UP TO BEGIN FADING
            interactableCollider.enabled = true;
        }

        private void OnIsActivatedChanged(bool oldSatus, bool newStatus)
        {
            if (isActivated.Value)
            {
                // play some vfx particles
                activatedParticles.SetActive(true);
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            if (!isActivated.Value)
            {
                RestoreSiteOfGrace(player);
            }
            else
            {
                RestAtSiteOfGrace(player);
            }



        }











    }
}

