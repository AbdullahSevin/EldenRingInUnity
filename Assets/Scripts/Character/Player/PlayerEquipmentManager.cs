using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace AS
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }

            }

        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }




        //  RIGHT WEAPON


        public void SwitchRightWeapon()
        {
            if (!player.IsOwner)
            {
                return;
            }

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true,true);

            WeaponItem selectedWeapon = null;

            //  DISABLE TWO HANDING IF WE ARE TWO HANDING 
            //  CHECK OUR WEAPON INDEX (WE HAVE 3 SLOTS, SO THATS 3 POSSIBLE NUMBERS)

            // ADD "1" TO OUR INDEX TO SWITCH TOTHE NEXT WEAPON
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            //  IF OUR INDEX IS OUT OF BOUNDS RESET IT TO POSITION #1 (0)
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                //  WE CHECK IF WE ARE HOLDING MORE THAN ONE WEAPON
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;   // -=1 ???
                    selectedWeapon = (WorldItemDatabase.Instance.unarmedWeapon);
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }
                return;

            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                //  IF THE NEXT POTENTIIAL WEAPON DOES NOT EQUAL THE UNARMED WEAPON
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID
                    !=
                    WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    //  ASSIGN THE NETWORK WEAPON ID SO IT SWITCHES FOR ALL CONNECTED CLIENTS
                    player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;

                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
            

        }


        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                //  REMOVE THE OLD WEAPON 
                rightHandSlot.UnloadWeapon();

                //  BRING IN THE NEW WEAPON
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
                // ASSIGN WEAPONS DAMAGE TO ITS COLLIDER
            }
        }




        //  LEFT WEAPON


        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner)
            {
                return;
            }

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            //  DISABLE TWO HANDING IF WE ARE TWO HANDING 
            //  CHECK OUR WEAPON INDEX (WE HAVE 3 SLOTS, SO THATS 3 POSSIBLE NUMBERS)

            // ADD "1" TO OUR INDEX TO SWITCH TOTHE NEXT WEAPON
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            //  IF OUR INDEX IS OUT OF BOUNDS RESET IT TO POSITION #1 (0)
            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                //  WE CHECK IF WE ARE HOLDING MORE THAN ONE WEAPON
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;   // -=1 ???
                    selectedWeapon = (WorldItemDatabase.Instance.unarmedWeapon);
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }
                return;

            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                //  IF THE NEXT POTENTIIAL WEAPON DOES NOT EQUAL THE UNARMED WEAPON
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID
                    !=
                    WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    //  ASSIGN THE NETWORK WEAPON ID SO IT SWITCHES FOR ALL CONNECTED CLIENTS
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;

                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }
        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                //  REMOVE THE OLD WEAPON 
                leftHandSlot.UnloadWeapon();

                //  BRING IN THE NEW WEAPON
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
                // ASSIGN WEAPONS DAMAGE TO ITS COLLIDER
            }
        }


        //  DAMAGE COLLIDERS

        public void OpenDamageCollider()
        {

            //  OPEN RIGHT WEAPON DAMAGE COLLIDER
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
            }
            //  OPEN LEFT WEAPON DAMAGE COLLIDER
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));
            }

            //  PLAY WHOOS SFX


        }

        public void CloseDamageCollider()
        {

            //  OPEN RIGHT WEAPON DAMAGE COLLIDER
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            //  OPEN LEFT WEAPON DAMAGE COLLIDER
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }


        }


    }
}

