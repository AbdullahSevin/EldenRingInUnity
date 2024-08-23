using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{

    [CreateAssetMenu(menuName = "Items/Weapons/Melee Weapon")]
    public class MeleeWeaponItem : WeaponItem
    {
        [Header("RIPOSTE MODIFIERS")]
        public float riposte_Attack_01_Modifier = 3.3f;
        public float backstab_Attack_01_Modifier = 3.3f;




        //  WEAPON "DEFLECTION" (If the weapn will bounce off another weapon when it is being guarded against)
        //  CAN BE BUFFED



    }
}

