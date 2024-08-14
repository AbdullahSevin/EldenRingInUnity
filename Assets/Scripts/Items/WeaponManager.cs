using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;


        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.holyDamage = weapon.holyDamage;

            meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
            meleeDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;

            meleeDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
            meleeDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;

            meleeDamageCollider.charge_Attack_01_Modifier = weapon.charge_Attack_01_Modifier;
            meleeDamageCollider.charge_Attack_02_Modifier = weapon.charge_Attack_02_Modifier;

            meleeDamageCollider.running_Attack_01_Modifier = weapon.running_Attack_01_Modifier;
            meleeDamageCollider.rolling_Attack_01_Modifier = weapon.rolling_Attack_01_Modifier;
            meleeDamageCollider.backstep_Attack_01_Modifier = weapon.backstep_Attack_01_Modifier;

        }




    }

    
}
  
