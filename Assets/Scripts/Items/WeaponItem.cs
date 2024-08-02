using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


namespace AS
{
    public class WeaponItem : Item
    {
        //  ANIMATOR CONTROLLOER OVERRIDE (Change attack animations based on weapon you are currently using)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthREQ = 0;
        public int dexREQ= 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int lightningDamage = 0;
        public int holyDamage = 0;

        //  WEAPON GUARD ABSORBTIONS (BLOCKING POWER)

        [Header("Weapon Poise")]
        public float poiseDamage = 10;
        //  OFFENSIVE POISE BONUS WHEN ATTACKING


        //  WEAPON MODIFIERS 
        //  LIGHT ATTACK MODIFIER
        //  HEAVY ATTACK MODIFIER
        //  CRITICAL DAMAGE MODIFIER ETC

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        //  RUNNNG ATTACK STAMINA COST MODIFIER
        //  LIGHT ATTACK STAMINA COST MOD.
        //  HEAVY ATT STAMINA COST MOD. ETC ...


        //  ITEM BASED ACTIONS (RB RT LB LT GAME-PAD)

        //  ASH OF WAR 

        //  BLOCKING SOUNDS


      






    }
}

