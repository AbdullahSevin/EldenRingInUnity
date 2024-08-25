using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class PlayerBodyManager : MonoBehaviour
    {
        PlayerManager player;


        [Header("Hair Object")]
        [SerializeField] public GameObject hair;
        [SerializeField] public GameObject facialHair;

        [Header("Male")]
        public GameObject maleObject;           // THE MASTER MALE GAMEOBJECT PARENT
        public GameObject maleHead;             // DEFAULT HEAD MODEL WHEN UNEQUIPPING ARMOR
        public GameObject[] maleBody;
        public GameObject[] maleArms;
        public GameObject[] maleLegs;
        public GameObject maleEyebrows;
        public GameObject maleFacialHair;
        

        [Header("Female")]
        public GameObject femaleObject;
        public GameObject femaleHead;
        public GameObject[] femaleBody;
        public GameObject[] femaleArms;
        public GameObject[] femaleLegs;
        public GameObject femaleEyebrows;


        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        // ENABLE BODY FEATURES
        public void EnableHead()
        {
            // Enable HEAD OBJECT
            maleHead.SetActive(true);
            femaleHead.SetActive(true);

            // ENABLE FACIAL OBJECTS (EYEBROWS LIPS NOSE ETC)
            maleEyebrows.SetActive(true);
            maleFacialHair.SetActive(true);
            femaleEyebrows.SetActive(true);
        }

        public void DisableHead()
        {
            // Disable HEAD OBJECT
            maleHead.SetActive(false);
            femaleHead.SetActive(false);

            // Disable FACIAL OBJECTS (EYEBROWS LIPS NOSE ETC)
            maleEyebrows.SetActive(false);
            maleFacialHair.SetActive(false);
            femaleEyebrows.SetActive(false);
        }



        public void EnableHair()
        {
            hair.SetActive(true);
        }

        public void DisableHair()
        {
            hair.SetActive(false);
        }

        public void EnableFacialHair()
        {
            facialHair.SetActive(true);
        }

        public void DisableFacialHair()
        {
            facialHair.SetActive(false);
        }

        public void EnableBody()
        {
            foreach (var model in maleBody)
            {
                model.SetActive(true);
            }

            foreach (var model in femaleBody)
            {
                model.SetActive(true);
            }
        }

        public void EnableLowerBody()
        {
            foreach (var model in maleLegs)
            {
                model.SetActive(true);
            }

            foreach (var model in femaleLegs)
            {
                model.SetActive(true);
            }
        }

        public void EnableArms()
        {
            foreach (var model in maleArms)
            {
                model.SetActive(true);
            }

            foreach (var model in femaleArms)
            {
                model.SetActive(true);
            }
        }

        public void DisableBody()
        {
            foreach (var model in maleBody)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleBody)
            {
                model.SetActive(false);
            }
        }

        public void DisableLowerBody()
        {
            foreach (var model in maleLegs)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleLegs)
            {
                model.SetActive(false);
            }
        }

        public void DisableArms()
        {
            foreach (var model in maleArms)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleArms)
            {
                model.SetActive(false);
            }
        }

        public void ToggleBodyType(bool isMale)
        {
            if (isMale)
            {
                maleObject.SetActive(true);
                femaleObject.SetActive(false);
            }
            else
            {
                maleObject.SetActive(false);
                femaleObject.SetActive(true);
            }

            player.playerEquipmentManager.EquipArmor();
        }



    }

}
