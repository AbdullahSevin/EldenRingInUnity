using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace AS
{
    public class PlayerUIEquipmentManager : MonoBehaviour
    {

        [Header("Menu")]
        [SerializeField] GameObject menu;

        [Header("Weapon Slots")]
        [SerializeField] Image rightHandSlot01;
        private Button rightHandSlot01Button;
        [SerializeField] Image rightHandSlot02;
        private Button rightHandSlot02Button;
        [SerializeField] Image rightHandSlot03;
        private Button rightHandSlot03Button;
        [SerializeField] Image leftHandSlot01;
        private Button leftHandSlot01Button;
        [SerializeField] Image leftHandSlot02;
        private Button leftHandSlot02Button;
        [SerializeField] Image leftHandSlot03;
        private Button leftHandSlot03Button;
        [SerializeField] Image headEquipmentSlot;
        private Button headEquipmentSlotButton;
        [SerializeField] Image bodyEquipmentSlot;
        private Button bodyEquipmentSlotButton;
        [SerializeField] Image legEquipmentSlot;
        private Button legEquipmentSlotButton;
        [SerializeField] Image handEquipmentSlot;
        private Button handEquipmentSlotButton;

        // THIS INVENTORY POPULATES WITH RELATED ITEMS WHEN CHANGING EQUIPMENT
        [Header("Equipment Inventory")]
        public EquipmentType currentSelectedEquipmentSlot;
        [SerializeField] GameObject equipmentInventoryWindow;
        [SerializeField] GameObject equipmentInventorySlotPrefab;
        [SerializeField] Transform equipmentInventoryContentWindow;
        [SerializeField] Item currentSelectedItem;


        private void Awake()
        {
            rightHandSlot01Button      = rightHandSlot01.GetComponentInParent<Button>(true);
            rightHandSlot02Button      = rightHandSlot02.GetComponentInParent<Button>(true);
            rightHandSlot03Button      = rightHandSlot03.GetComponentInParent<Button>(true);
 
            leftHandSlot01Button      = leftHandSlot01.GetComponentInParent<Button>(true);
            leftHandSlot02Button      = leftHandSlot02.GetComponentInParent<Button>(true);
            leftHandSlot03Button      = leftHandSlot03.GetComponentInParent<Button>(true);
 
            headEquipmentSlotButton  = headEquipmentSlot.GetComponentInParent<Button>(true);
            bodyEquipmentSlotButton  = bodyEquipmentSlot.GetComponentInParent<Button>(true);
            legEquipmentSlotButton  = legEquipmentSlot.GetComponentInParent<Button>(true);
            handEquipmentSlotButton  = handEquipmentSlot.GetComponentInParent<Button>(true);

        }


        public void OpenEquipmentManagerMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            ToggleEquipmentButtons(true);
            menu.SetActive(true);
            equipmentInventoryWindow.SetActive(false);
            ClearEquipmentInventroy();
            RefreshEquipmentSlotIcons();
        }

        public void RefreshMenu()
        {
            ClearEquipmentInventroy();
            RefreshEquipmentSlotIcons();
        }

        private void ToggleEquipmentButtons(bool isEnabled)
        {
            rightHandSlot01Button.enabled = isEnabled;
            rightHandSlot02Button.enabled = isEnabled;
            rightHandSlot03Button.enabled = isEnabled;

            leftHandSlot01Button.enabled = isEnabled;
            leftHandSlot02Button.enabled = isEnabled;
            leftHandSlot03Button.enabled = isEnabled;

            headEquipmentSlotButton.enabled = isEnabled;
            bodyEquipmentSlotButton.enabled = isEnabled;
            legEquipmentSlotButton.enabled = isEnabled;
            handEquipmentSlotButton.enabled = isEnabled;
        }

        public void SelectLastSelectedEquipmentSlot()
        {
            Button lastSelectedButton = null;

            ToggleEquipmentButtons(true);

            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    lastSelectedButton = rightHandSlot01Button;
                    break;
                case EquipmentType.RightWeapon02:
                    lastSelectedButton = rightHandSlot02Button;
                    break;
                case EquipmentType.RightWeapon03:
                    lastSelectedButton = rightHandSlot03Button;
                    break;
                case EquipmentType.LeftWeapon01:
                    lastSelectedButton = leftHandSlot01Button;
                    break;
                case EquipmentType.LeftWeapon02:
                    lastSelectedButton = leftHandSlot02Button;
                    break;
                case EquipmentType.LeftWeapon03:
                    lastSelectedButton = leftHandSlot03Button;
                    break;
                case EquipmentType.Head:
                    lastSelectedButton = headEquipmentSlotButton;
                    break;
                case EquipmentType.Body:
                    lastSelectedButton = bodyEquipmentSlotButton;
                    break;
                case EquipmentType.Legs:
                    lastSelectedButton = legEquipmentSlotButton;
                    break;
                case EquipmentType.Hands:
                    lastSelectedButton = handEquipmentSlotButton;
                    break;
            }

            if (lastSelectedButton != null)
            {
                lastSelectedButton.Select();
                lastSelectedButton.OnSelect(null);
            }

            equipmentInventoryWindow.SetActive(false);

        }
        
        
        //  THIS ISFINE BUT IF YOU R USING THE A BUTTON TO COSE MENUS YOU WILL JUMP AS YOU CLOSE THE MENU   
        public void CloseEquipmentManagerMenu()
        {

            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        private void RefreshEquipmentSlotIcons()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();


            // RIGHT WEAPON 01
            WeaponItem rightHandWeapon01 = player.playerInventoryManager.weaponsInRightHandSlots[0];

            if (rightHandWeapon01.itemIcon != null)
            {
                rightHandSlot01.enabled = true;
                rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
            }
            else
            {
                rightHandSlot01.enabled = false;
            }

            // RIGHT WEAPON 02
            WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponsInRightHandSlots[1];

            if (rightHandWeapon02.itemIcon != null)
            {
                rightHandSlot02.enabled = true;
                rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
            }
            else
            {
                rightHandSlot02.enabled = false;
            }

            // RIGHT WEAPON 03
            WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponsInRightHandSlots[2];

            if (rightHandWeapon03.itemIcon != null)
            {
                rightHandSlot03.enabled = true;
                rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
            }
            else
            {
                rightHandSlot03.enabled = false;
            }

            // left WEAPON 01
            WeaponItem leftHandWeapon01 = player.playerInventoryManager.weaponsInLeftHandSlots[0];

            if (leftHandWeapon01.itemIcon != null)
            {
                leftHandSlot01.enabled = true;
                leftHandSlot01.sprite = leftHandWeapon01.itemIcon;
            }
            else
            {
                leftHandSlot01.enabled = false;
            }

            // left WEAPON 02
            WeaponItem leftHandWeapon02 = player.playerInventoryManager.weaponsInLeftHandSlots[1];

            if (leftHandWeapon02.itemIcon != null)
            {
                leftHandSlot02.enabled = true;
                leftHandSlot02.sprite = leftHandWeapon02.itemIcon;
            }
            else
            {
                leftHandSlot02.enabled = false;
            }

            // left WEAPON 03
            WeaponItem leftHandWeapon03 = player.playerInventoryManager.weaponsInLeftHandSlots[2];

            if (leftHandWeapon03.itemIcon != null)
            {
                leftHandSlot03.enabled = true;
                leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
            }
            else
            {
                leftHandSlot03.enabled = false;
            }

            // HEAD EQUIPMENT

            HeadEquipmentItem headEquipment = player.playerInventoryManager.headEquipment;

            if (headEquipment != null)
            {
                headEquipmentSlot.enabled = true;
                headEquipmentSlot.sprite = headEquipment.itemIcon;
            }
            else
            {
                headEquipmentSlot.enabled = false;
            }

            // BODY EQUIPMENT

            BodyEquipmentItem bodyEquipment = player.playerInventoryManager.bodyEquipment;

            if (bodyEquipment != null)
            {
                bodyEquipmentSlot.enabled = true;
                bodyEquipmentSlot.sprite = bodyEquipment.itemIcon;
            }
            else
            {
                bodyEquipmentSlot.enabled = false;
            }

            // LEG EQUIPMENT

            LegEquipmentItem legEquipment = player.playerInventoryManager.legEquipment;

            if (legEquipment != null)
            {
                legEquipmentSlot.enabled = true;
                legEquipmentSlot.sprite = legEquipment.itemIcon;
            }
            else
            {
                legEquipmentSlot.enabled = false;
            }

            // HAND EQUIPMENT

            HandEquipmentItem handEquipment = player.playerInventoryManager.handEquipment;

            if (handEquipment != null)
            {
                handEquipmentSlot.enabled = true;
                handEquipmentSlot.sprite = handEquipment.itemIcon;
            }
            else
            {
                handEquipmentSlot.enabled = false;
            }















        }

        private void ClearEquipmentInventroy()
        {
            foreach (Transform item in equipmentInventoryContentWindow)
            {
                Destroy(item.gameObject);
            }
        }

        public void LoadEquipmentInventory()
        {
            ToggleEquipmentButtons(false);
            equipmentInventoryWindow.SetActive(true);
            Debug.Log("LoadEquipmentInventory worked: inv window set active true");

            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon03:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon03:
                    LoadWeaponInventory();
                    break;
                case EquipmentType.Head:
                    LoadHeadEquipmentInventory();
                    break;
                case EquipmentType.Body:
                    LoadBodyEquipmentInventory();
                    break;
                case EquipmentType.Legs:
                    LoadLegEquipmentInventory();
                    break;
                case EquipmentType.Hands:
                    LoadHandEquipmentInventory();
                    break;
                default:
                    break;
            }

        }

        private void LoadWeaponInventory()
        {
            List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // SEARCH OUR ENTIRE INVENTORY, AND OUT OF ALL OF THE ITEMS IN OUR INVENTORY, IF THE ITEM IS A WEAPON ADD IT TO OUR WEAPON LIST

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;

                if (weapon != null)
                {
                    weaponsInInventory.Add(weapon);
                }
            }

            if (weaponsInInventory.Count <= 0)
            {
                // TODO SEND THE PLAYER A POP UP MESSAGE THAT HE HAS NONE OF ITEM TYPE IN INVENTORY
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < weaponsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(weaponsInInventory[i]);

                // THIS WILL SELECT THE FIRST BUTTON IN THE LIST
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }

            }



        }

        private void LoadHeadEquipmentInventory()
        {
            List<HeadEquipmentItem> headEquipmentInInventory = new List<HeadEquipmentItem>();

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // SEARCH OUR ENTIRE INVENTORY, AND OUT OF ALL OF THE ITEMS IN OUR INVENTORY, IF THE ITEM IS A head ADD IT TO OUR head LIST

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                HeadEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;

                if (equipment != null)
                {
                    headEquipmentInInventory.Add(equipment);
                }
            }

            if (headEquipmentInInventory.Count <= 0)
            {
                // TODO SEND THE PLAYER A POP UP MESSAGE THAT HE HAS NONE OF ITEM TYPE IN INVENTORY
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < headEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(headEquipmentInInventory[i]);

                // THIS WILL SELECT THE FIRST BUTTON IN THE LIST
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }

            }


        }

        private void LoadBodyEquipmentInventory()
        {
            List<BodyEquipmentItem> bodyEquipmentInInventory = new List<BodyEquipmentItem>();

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // SEARCH OUR ENTIRE INVENTORY, AND OUT OF ALL OF THE ITEMS IN OUR INVENTORY, IF THE ITEM IS A body ADD IT TO OUR body LIST

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                BodyEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;

                if (equipment != null)
                {
                    bodyEquipmentInInventory.Add(equipment);
                }
            }

            if (bodyEquipmentInInventory.Count <= 0)
            {
                // TODO SEND THE PLAYER A POP UP MESSAGE THAT HE HAS NONE OF ITEM TYPE IN INVENTORY
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < bodyEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(bodyEquipmentInInventory[i]);

                // THIS WILL SELECT THE FIRST BUTTON IN THE LIST
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }

            }
        }

        private void LoadLegEquipmentInventory()
        {
            List<LegEquipmentItem> legEquipmentInInventory = new List<LegEquipmentItem>();

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // SEARCH OUR ENTIRE INVENTORY, AND OUT OF ALL OF THE ITEMS IN OUR INVENTORY, IF THE ITEM IS A leg ADD IT TO OUR leg LIST

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                LegEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;

                if (equipment != null)
                {
                    legEquipmentInInventory.Add(equipment);
                }
            }

            if (legEquipmentInInventory.Count <= 0)
            {
                // TODO SEND THE PLAYER A POP UP MESSAGE THAT HE HAS NONE OF ITEM TYPE IN INVENTORY
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < legEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(legEquipmentInInventory[i]);

                // THIS WILL SELECT THE FIRST BUTTON IN THE LIST
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }

            }
        }

        private void LoadHandEquipmentInventory()
        {
            List<HandEquipmentItem> handEquipmentInInventory = new List<HandEquipmentItem>();

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            // SEARCH OUR ENTIRE INVENTORY, AND OUT OF ALL OF THE ITEMS IN OUR INVENTORY, IF THE ITEM IS A hand ADD IT TO OUR hand LIST

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                HandEquipmentItem equipment = player.playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;

                if (equipment != null)
                {
                    handEquipmentInInventory.Add(equipment);
                }
            }

            if (handEquipmentInInventory.Count <= 0)
            {
                // TODO SEND THE PLAYER A POP UP MESSAGE THAT HE HAS NONE OF ITEM TYPE IN INVENTORY
                equipmentInventoryWindow.SetActive(false);
                ToggleEquipmentButtons(true);
                RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < handEquipmentInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                equipmentInventorySlot.AddItem(handEquipmentInInventory[i]);

                // THIS WILL SELECT THE FIRST BUTTON IN THE LIST
                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);
                }

            }
        }

        public void SelectEquipmentSlot(int equipmentSlot)
        {
            currentSelectedEquipmentSlot = (EquipmentType)equipmentSlot;
        }

        public void UnequipSelectedItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item unequippedItem;

            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];

                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[0] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }

                    break;
                case EquipmentType.RightWeapon02:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];

                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }

                    break;
                case EquipmentType.RightWeapon03:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];

                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[2] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }

                    break;
                case EquipmentType.LeftWeapon01:
                    unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[0];

                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInLeftHandSlots[0] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }

                    if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }

                    break;
                case EquipmentType.LeftWeapon02:
                    unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[1];

                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInLeftHandSlots[1] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }

                    if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }

                    break;
                case EquipmentType.LeftWeapon03:
                    unequippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[2];

                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInLeftHandSlots[2] = Instantiate(WorldItemDatabase.Instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                        }
                    }

                    if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                    {
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Instance.unarmedWeapon.itemID;
                    }

                    break;
                case EquipmentType.Head:
                    unequippedItem = player.playerInventoryManager.headEquipment;

                    
                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.headEquipment = null;
                    player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                    break;
                case EquipmentType.Body:
                    unequippedItem = player.playerInventoryManager.bodyEquipment;


                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.bodyEquipment = null;
                    player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                    break;
                case EquipmentType.Legs:
                    unequippedItem = player.playerInventoryManager.legEquipment;


                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.legEquipment = null;
                    player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                    break;
                case EquipmentType.Hands:
                    unequippedItem = player.playerInventoryManager.handEquipment;


                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);

                    player.playerInventoryManager.handEquipment = null;
                    player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                    break;
            }

            // REFRESHES MENU (ICONS ETC)
            RefreshMenu();


        }
        
    }
}