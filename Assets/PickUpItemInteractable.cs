using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace AS
{
    public class PickUpItemInteractable : Interactable
    {

        public ItemPickupType pickupType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("World Spawn Pick Up")]
        [SerializeField] int itemID;
        [SerializeField] bool hasBeenLooted = false;

        protected override void Start()
        {
            
            base.Start();

            if (pickupType == ItemPickupType.WorldSpawn)
            {
                CheckIfWorldItemWasAlreadyLooted();
            }



            

        }


        private void CheckIfWorldItemWasAlreadyLooted()
        {
            //  IF THE PLAYER ISN'T HOST HIDE THE ITEM
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }

            // COMPARE THE DATA OF THE LOOTED ITEMS ID S WITH THIS ITEMS ID
            if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[itemID];

            // IF IT HAS BEEN LOOTED HIDE THE GAMEOBJECT
            if (hasBeenLooted)
            {
                gameObject.SetActive(false);    
            }
            else
            {
                gameObject.SetActive(true);
            }



        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // 1 play sfx
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.pickUpItemSFX);
            // 2 ad item to inventory
            player.playerInventoryManager.AddItemToInventory(item);
            // 3 show ui popup show item name n pic
            PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item, 1);


            // 4 SAVE LOOT STATUS IF IT IS A WORLD SPAWN
            if (pickupType == ItemPickupType.WorldSpawn)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey((int)itemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(itemID);

                }
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, true);

            }
            // 5 HIDE OR DESTROY THE GAME OBJECT
            Destroy(gameObject);
        }


    }
}

