using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;


namespace AS
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int horizontal;
        int vertical;

        [Header("Damage Animations")]
        public string lastDamageAnimationPlayed;

        [SerializeField] string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
        [SerializeField] string hit_Forward_Medium_02 = "hit_Forward_Medium_02";

        [SerializeField] string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
        [SerializeField] string hit_Backward_Medium_02 = "hit_Backward_Medium_02";

        [SerializeField] string hit_Left_Medium_01 = "hit_Left_Medium_01";
        [SerializeField] string hit_Left_Medium_02 = "hit_Left_Medium_02";

        [SerializeField] string hit_Right_Medium_01 = "hit_Right_Medium_01";
        [SerializeField] string hit_Right_Medium_02 = "hit_Right_Medium_02";

        public List<string> forward_Medium_Damage = new List<string>();
        public List<string> backward_Medium_Damage = new List<string>();
        public List<string> left_Medium_Damage = new List<string>();
        public List<string> right_Medium_Damage = new List<string>();

        private float snappedHorizontal;
        public float snappedVertical;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

        }

        protected virtual void Start()
        {
            forward_Medium_Damage.Add(hit_Forward_Medium_01);
            forward_Medium_Damage.Add(hit_Forward_Medium_02);

            backward_Medium_Damage.Add(hit_Backward_Medium_01);
            backward_Medium_Damage.Add(hit_Backward_Medium_02);

            left_Medium_Damage.Add(hit_Left_Medium_01);
            left_Medium_Damage.Add(hit_Left_Medium_02);

            right_Medium_Damage.Add(hit_Right_Medium_01);
            right_Medium_Damage.Add(hit_Right_Medium_02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();

            foreach (var item in animationList)
            {
                finalList.Add(item);
            }

            //  CHECK IF WE PLAYED THIS ANIMATON AND REMVE IF WE DID, SO IT DOESNT REPEAT
            finalList.Remove(lastDamageAnimationPlayed);

            //  CHECK THE LIST AND REMOVE THE NULL ENTRIES IF THERE ARE ANY
            for (int i = finalList.Count - 1; i > -1; i--)
            {
                if (finalList[i] == null)
                {
                    finalList.RemoveAt(i);
                }
            }

            int randomValue = Random.Range(0, finalList.Count);

            return finalList[randomValue];
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float snappedHorizontal;
            float snappedVertical;

            if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.5f && horizontalMovement <= 1f)
            {
                snappedHorizontal = 1f;
            }
            else if (horizontalMovement < 0 && horizontalMovement >= -0.5)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.5f && horizontalMovement >= -1f)
            {
                
                snappedHorizontal = -1f;
            }
            else
            {
                snappedHorizontal = 0;
            }

            if (verticalMovement > 0 && verticalMovement <= 0.5f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.5f && verticalMovement <= 1f)
            {
                snappedVertical = 1f;
            }
            else if (verticalMovement < 0 && verticalMovement >= -0.5)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalMovement < -0.5f && verticalMovement >= -1f)
            {
                
                snappedVertical = -1f;
            }
            else
            {
                snappedVertical = 0;
            }

            if (isSprinting)
            {
                snappedVertical = 2;
            }

            character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(string targetAnimation, 
            bool isPorformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            //  Debug.Log("PLAYING ANIMATION: " + targetAnimation);

            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            // CAN BE USED TO STOP CHARACTER FORM ATTEMPTING NEW ACTION
            // FOR EXAMPLE IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
            // THIS FLAG WILL TURN IF YOU ARE STUNNED
            // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
            character.isPerformingAction = isPorformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType, string targetAnimation,
            bool isPorformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {

            //  KEEP TRACK OF LAST ATTACK PERFORMED (FOR COMBOS)
            //  KEEP TRACK OF CURRENT ATTACK TYPE (LIGHT, HEAVY, ETC)
            //  UPDATE ANIMATION SET TO CURRENT WEAPON ANIMATIONS
            //  DECIDE IF OUR ATTACK CAN BE PARRIED
            //  TELL THE NETWORK OUR "ISATTACKING" FLAG IS ACTIVE (For counter damage etc)

            character.characterCombatManager.currentAttackType = attackType;
            character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation; 
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPorformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }

        public virtual void EnableCanDoCombo()
        {
            
        }


        public virtual void DisableCanDoCombo()
        {
           

        }

    }
}

