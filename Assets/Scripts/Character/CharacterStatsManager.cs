using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace AS
{
    public class CharacterStatsManager : MonoBehaviour
    {

        CharacterManager character;

        [Header("Health Regeneration")]
        [SerializeField] int healthRegenerationAmount = 1;
        private float healthRegenerationTimer = 1;
        private float healthTickTimer = 0;
        [SerializeField] float healthRegenerationDelay = 0;

        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenerationAmount = 2;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 1;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalAbsorption;
        public float blockingMagicAbsorption;
        public float blockingFireAbsorption;    
        public float blockingLightningAbsorption;
        public float blockingHolyAbsorption;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }


        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            // MAKE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED

            health = vitality * 100;

            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            // MAKE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            // ONLY OWNERS CAN EDIT THEIR NETWORK VARIABLES
            if (!character.IsOwner)
            {
                return;
            }

            // WE  DONT WANT TO REGENERATE STAMINA WHILE USING IT
            if (character.characterNetworkManager.isSprinting.Value == true)
            {
                return;
            }
            if (character.isPerformingAction)
            {
                return;
            }

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }

        }

        public virtual void RegenerateHealth()
        {
            // ONLY OWNERS CAN EDIT THEIR NETWORK VARIABLES
            if (!character.IsOwner)
            {
                return;
            }


            healthRegenerationTimer += Time.deltaTime;

            if (healthRegenerationTimer >= healthRegenerationDelay)
            {
                if (character.characterNetworkManager.currentHealth.Value < character.characterNetworkManager.maxHealth.Value)
                {
                    healthTickTimer += Time.deltaTime;

                    if (healthTickTimer >= 0.1)
                    {
                        healthTickTimer = 0;
                        character.characterNetworkManager.currentHealth.Value += healthRegenerationAmount;
                    }
                }
            }

        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            // WE ONLY WANT TO RESET THE REGENERATION IF THE ACTION USED STAMINA
            // WE DONT WANT TO RESET THE REGENERATION IF WE ARE ALREADY REGENERATING STAMINA
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
            
        }


    }

}

