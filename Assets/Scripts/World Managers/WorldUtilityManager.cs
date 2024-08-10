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


    }
}

