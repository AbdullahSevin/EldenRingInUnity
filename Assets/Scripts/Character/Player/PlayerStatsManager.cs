using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class PlayerStatsManager : CharacterStatsManager
    {

        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            //  WHY CALCULATE THESE HERE?
            //  WHEN WE MAKE A NEW CHARACTER CREATION MENU, AND SET THE STATS DEPENDING ON THE CLASS, THIS WILL BE CALCULATED THERE
            // UNTIL THEN HOWEVER, STATS ARE NEVER CALCULATED, SO WE DO IT HERE ON START, IF A SAVE FILE EXISTS THEY WILL BE OVER WRITTEN WHEN LOADING INTO A SCENE
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value); 
        }

        public void CalculateTotalArmorAbsorption()
        {
            // RESET ALL VALUES TO 0
            armorPhysicalDamageAbsorpion = 0;
            armorMagicDamageAbsorpion = 0;
            armorFireDamageAbsorpion = 0;
            armorLightningDamageAbsorpion = 0;
            armorHolyDamageAbsorpion = 0;

            armorImmunity = 0;
            armorRobutsness = 0;
            armorFocus = 0;
            armorVitality = 0;

            basePoiseDefense = 0;


            //   EQUIPMENT
            if (player.playerInventoryManager.headEquipment != null)
            {
                // DAMAGE RESISTANCE
                armorPhysicalDamageAbsorpion += player.playerInventoryManager.headEquipment.physicalDamageAbsorpion;
                armorMagicDamageAbsorpion += player.playerInventoryManager.headEquipment.magicDamageAbsorpion;
                armorFireDamageAbsorpion += player.playerInventoryManager.headEquipment.fireDamageAbsorpion;
                armorLightningDamageAbsorpion += player.playerInventoryManager.headEquipment.lightningDamageAbsorpion;
                armorHolyDamageAbsorpion += player.playerInventoryManager.headEquipment.holyDamageAbsorpion;

                // STATUS EFFECT RESISTANCE
                armorRobutsness += player.playerInventoryManager.headEquipment.robutsness;
                armorVitality += player.playerInventoryManager.headEquipment.vitality;
                armorImmunity += player.playerInventoryManager.headEquipment.immunity;
                armorFocus += player.playerInventoryManager.headEquipment.focus;

                // POISE
                basePoiseDefense += player.playerInventoryManager.headEquipment.poise;
            }

            // BODY EQUIPMENT
            if (player.playerInventoryManager.bodyEquipment != null)
            {
                // DAMAGE RESISTANCE
                armorPhysicalDamageAbsorpion += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorpion;
                armorMagicDamageAbsorpion += player.playerInventoryManager.bodyEquipment.magicDamageAbsorpion;
                armorFireDamageAbsorpion += player.playerInventoryManager.bodyEquipment.fireDamageAbsorpion;
                armorLightningDamageAbsorpion += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorpion;
                armorHolyDamageAbsorpion += player.playerInventoryManager.bodyEquipment.holyDamageAbsorpion;

                // STATUS EFFECT RESISTANCE
                armorRobutsness += player.playerInventoryManager.bodyEquipment.robutsness;
                armorVitality += player.playerInventoryManager.bodyEquipment.vitality;
                armorImmunity += player.playerInventoryManager.bodyEquipment.immunity;
                armorFocus += player.playerInventoryManager.bodyEquipment.focus;

                // POISE
                basePoiseDefense += player.playerInventoryManager.bodyEquipment.poise;
            }

            // HAND EQUIPMENT
            if (player.playerInventoryManager.handEquipment != null)
            {
                // DAMAGE RESISTANCE
                armorPhysicalDamageAbsorpion += player.playerInventoryManager.handEquipment.physicalDamageAbsorpion;
                armorMagicDamageAbsorpion += player.playerInventoryManager.handEquipment.magicDamageAbsorpion;
                armorFireDamageAbsorpion += player.playerInventoryManager.handEquipment.fireDamageAbsorpion;
                armorLightningDamageAbsorpion += player.playerInventoryManager.handEquipment.lightningDamageAbsorpion;
                armorHolyDamageAbsorpion += player.playerInventoryManager.handEquipment.holyDamageAbsorpion;

                // STATUS EFFECT RESISTANCE
                armorRobutsness += player.playerInventoryManager.handEquipment.robutsness;
                armorVitality += player.playerInventoryManager.handEquipment.vitality;
                armorImmunity += player.playerInventoryManager.handEquipment.immunity;
                armorFocus += player.playerInventoryManager.handEquipment.focus;

                // POISE
                basePoiseDefense += player.playerInventoryManager.handEquipment.poise;
            }

            // LEG EQUIPMENT
            if (player.playerInventoryManager.legEquipment != null)
            {
                // DAMAGE RESISTANCE
                armorPhysicalDamageAbsorpion += player.playerInventoryManager.legEquipment.physicalDamageAbsorpion;
                armorMagicDamageAbsorpion += player.playerInventoryManager.legEquipment.magicDamageAbsorpion;
                armorFireDamageAbsorpion += player.playerInventoryManager.legEquipment.fireDamageAbsorpion;
                armorLightningDamageAbsorpion += player.playerInventoryManager.legEquipment.lightningDamageAbsorpion;
                armorHolyDamageAbsorpion += player.playerInventoryManager.legEquipment.holyDamageAbsorpion;

                // STATUS EFFECT RESISTANCE
                armorRobutsness += player.playerInventoryManager.legEquipment.robutsness;
                armorVitality += player.playerInventoryManager.legEquipment.vitality;
                armorImmunity += player.playerInventoryManager.legEquipment.immunity;
                armorFocus += player.playerInventoryManager.legEquipment.focus;

                // POISE
                basePoiseDefense += player.playerInventoryManager.legEquipment.poise;
            }

        }
    }

}


