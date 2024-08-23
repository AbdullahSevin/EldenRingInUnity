using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager instance;

        [Header("Layers")]
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask enviroLayers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviroLayers()
        {
            return enviroLayers;
        }

        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if (attackingCharacter == CharacterGroup.Team01)
            {
                // Debug.Log("UTILITY T1 attacking:" + attackingCharacter + "   target:  " + targetCharacter);
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: /* Debug.Log("UTILITY CAN I ATTACK RETURNED FALSE T1"); */ return false;
                    case CharacterGroup.Team02: /* Debug.Log("UTILITY CAN I ATTACK RETURNED TRUE T1"); */ return true;
                    default:
                        break;
                }
            }
            else if (attackingCharacter == CharacterGroup.Team02)
            {
                // Debug.Log("UTILITY T2 attacking:" + attackingCharacter + "   target:  " + targetCharacter);
                switch (targetCharacter)
                {
                    // sanki kendini de target olarak gönderiyor buraya, öyle gibi
                    case CharacterGroup.Team01: /* Debug.Log("UTILITY CAN I ATTACK RETURNED TRUE T2"); */ return true;  
                    case CharacterGroup.Team02: /* Debug.Log("UTILITY CAN I ATTACK RETURNED FALSE T2"); */ return false; 
                    default:
                        break;
                }
            }
            return false;
        }

        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

            if (cross.y < 0)
            {
                viewableAngle = -viewableAngle;
            }

            return viewableAngle;
        }

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
        {
            // THROWING DAGGERS, KITCHEN KNIFE, CURSING WORDS MAYBE
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            // DAGGERS / LIGHT ATTACKS
            if (poiseDamage >= 10)
                damageIntensity = DamageIntensity.Light;

            // STANDARD WEAPONS / MEDIUM ATTACKS
            if (poiseDamage >= 30)
                damageIntensity = DamageIntensity.Medium;

            // GREAT WEAPONS / HEAVY ATTACKS / EMOTIONAL DAMAGE maybe
            if (poiseDamage >= 70)
                damageIntensity = DamageIntensity.Heavy;

            // ULTRA WEAPONS / COLOSSAL ATTACKS
            if (poiseDamage >= 120)
                damageIntensity = DamageIntensity.Colossal;

            return damageIntensity;


        }

        public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.11f, 0, 0.7f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword: // CHANGE POSITION HERE IF YOU DESIRE TO DO SO
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    break;
                case WeaponClass.Fist:
                    break;
            }

            return position;

        }

        public Vector3 GetBackstabPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.12f, 0, 0.74f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword: // CHANGE POSITION HERE IF YOU DESIRE TO DO SO
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    break;
                case WeaponClass.Fist:
                    break;
            }

            return position;

        }
    }
}