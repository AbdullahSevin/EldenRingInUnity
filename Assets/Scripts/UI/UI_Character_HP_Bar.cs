using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AS
{
    //  PERFORMS IDENTICALLY TO THE UI_STAT BAR, EXCEPT THIS BAR APPEARS AND DISAPPEARS IN WORLD SPACE (WILL ALWAYS FACE CAMERA)
    public class UI_Character_HP_Bar : UI_StatBar
    {
        private CharacterManager character;
        private AICharacterManager aiCharacter;
        private PlayerManager playerCharacter;

        [SerializeField] bool displayCharacterNameOnDamage = false;
        [SerializeField] float defaultTimeBeforeBarHides = 3;
        [SerializeField] float hideTimer = 0;
        [SerializeField] int currentDamageTaken = 0;
        [SerializeField] TextMeshProUGUI characterName;
        [SerializeField] TextMeshProUGUI characterDamage;
        [HideInInspector] public int oldHealthValue = 0;

        protected override void Awake()
        {
            base.Awake();
            character = GetComponentInParent<CharacterManager>();

            if (character != null)
            {
                aiCharacter = character as AICharacterManager;
                playerCharacter = character as PlayerManager;
            }
        }

        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(false);
        }


        public override void SetStat(int newValue)
        {
            if (displayCharacterNameOnDamage)
            {
                characterName.enabled = true;

                if (aiCharacter != null)
                {
                    characterName.text = aiCharacter.characterName;
                }

                if (playerCharacter != null)
                {
                    characterName.text = playerCharacter.playerNetworkManager.characterName.Value.ToString();
                }
            }

            // CALL THIS HERE INCASE MAX HEALTH CHANGES FROM A CHARACTER EFFECT/BUFF ETC
            slider.maxValue = character.characterNetworkManager.maxHealth.Value;

            // TO DO: RUN SECONDARY BAR LOGIC (YELLOW BAR THAT APPEARS BEHIND HP WHEN DAMAGED)

            // TOTAL THE DAMAGE TAKEN WHILST THE BAR IS ACTIVE
            float oldDamage = currentDamageTaken;
            currentDamageTaken = Mathf.RoundToInt(currentDamageTaken + (oldHealthValue - newValue));

            if (currentDamageTaken < 0)
            {
                currentDamageTaken = Mathf.Abs(currentDamageTaken);
                characterDamage.text = "+ " + currentDamageTaken.ToString();
            }
            else
            {
                characterDamage.text = "- " + currentDamageTaken.ToString();
            }

            slider.value = newValue;

            if (character.characterNetworkManager.currentHealth.Value != character.characterNetworkManager.maxHealth.Value)
            {
                hideTimer = defaultTimeBeforeBarHides;
                gameObject.SetActive(true);

            }


        }

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);

            if (hideTimer > 0)
            {
                hideTimer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }

        }

        private void OnDisable()
        {
            currentDamageTaken = 0;
        }



    }
}

