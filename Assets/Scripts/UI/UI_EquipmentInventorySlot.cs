using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace AS
{
    public class UI_EquipmentInventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightedIcon;
        [SerializeField] public Item currentItem;


        public void AddItem(Item item)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;

        }

        public void SelectSlot()
        {
            highlightedIcon.enabled = true;
        }

        public void DeSelectSlot()
        {
            highlightedIcon.enabled = false;
        }

        public void EquipItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item equippedItem;


            switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:

                    //Debug.Log("case EquipmentType.RightWeapon01");
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                        //Debug.Log("added item to inv");
                    }

                    // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR SELECTED WEAPON
                    player.playerInventoryManager.weaponsInRightHandSlots[0] = currentItem as WeaponItem;

                    // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE  HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;
                        //Debug.Log("changed weapon");
                    }

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    // Debug.Log("REFRESH EQUIPMENT WINDOW");

                    break;
                case EquipmentType.RightWeapon02:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR SELECTED WEAPON
                    player.playerInventoryManager.weaponsInRightHandSlots[1] = currentItem as WeaponItem;

                    // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE  HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;
                    }

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.RightWeapon03:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR SELECTED WEAPON
                    player.playerInventoryManager.weaponsInRightHandSlots[2] = currentItem as WeaponItem;

                    // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE RIGHT WEAPON 3 AND YOU ARE  HOLDING RIGHT WEAPON 1 NOTHING WOULD HAPPEN HERE)
                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;
                    }

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon01:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[0];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR SELECTED WEAPON
                    player.playerInventoryManager.weaponsInLeftHandSlots[0] = currentItem as WeaponItem;

                    // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE left WEAPON 3 AND YOU ARE  HOLDING left WEAPON 1 NOTHING WOULD HAPPEN HERE)
                    if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;
                    }

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon02:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[1];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR SELECTED WEAPON
                    player.playerInventoryManager.weaponsInLeftHandSlots[1] = currentItem as WeaponItem;

                    // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE left WEAPON 3 AND YOU ARE  HOLDING left WEAPON 1 NOTHING WOULD HAPPEN HERE)
                    if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;
                    }

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon03:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[2];

                    if (equippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR SELECTED WEAPON
                    player.playerInventoryManager.weaponsInLeftHandSlots[2] = currentItem as WeaponItem;

                    // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT (IF YOU CHANGE left WEAPON 3 AND YOU ARE  HOLDING left WEAPON 1 NOTHING WOULD HAPPEN HERE)
                    if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;
                    }

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;

                case EquipmentType.Head:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT A NULL ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.headEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN ASSIGN THE SLOT OUR NEW ITEM
                    player.playerInventoryManager.headEquipment = currentItem as HeadEquipmentItem;

                    // THEN REMOVE THE NEW ITEM FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW ITEM
                    player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;

                case EquipmentType.Body:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT A NULL ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.bodyEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN ASSIGN THE SLOT OUR NEW ITEM
                    player.playerInventoryManager.bodyEquipment = currentItem as BodyEquipmentItem;

                    // THEN REMOVE THE NEW ITEM FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW ITEM
                    player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;

                case EquipmentType.Legs:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT A NULL ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.legEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN ASSIGN THE SLOT OUR NEW ITEM
                    player.playerInventoryManager.legEquipment = currentItem as LegEquipmentItem;

                    // THEN REMOVE THE NEW ITEM FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW ITEM
                    player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;

                case EquipmentType.Hands:
                    // IF OUR CURRENT WEAPON IN THIS SLOT, IS NOT A NULL ITEM, ADD IT TO OUR INVENTORY
                    equippedItem = player.playerInventoryManager.handEquipment;

                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }

                    // THEN ASSIGN THE SLOT OUR NEW ITEM
                    player.playerInventoryManager.handEquipment = currentItem as HandEquipmentItem;

                    // THEN REMOVE THE NEW ITEM FROM OUR INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    // RE-EQUIP NEW ITEM
                    player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                    // REFRESHES EQUIPMENT WINDOW
                    PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                    break;

            }

            PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();


        }



    }
}

