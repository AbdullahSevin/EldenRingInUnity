using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class PlayerBodyManager : MonoBehaviour
    {

        [Header("Hair Object")]
        [SerializeField] public GameObject hair;
        [SerializeField] public GameObject facialHair;

        [Header("Male")]
        public GameObject maleHead;             // DEFAULT HEAD MODEL WHEN UNEQUIPPING ARMOR
        public GameObject[] maleBody;
        public GameObject[] maleArms;
        public GameObject[] maleLegs;
        public GameObject maleEyebrows;
        public GameObject maleFacialHair;
        

        [Header("Female")]
        public GameObject femaleHead;
        public GameObject[] femaleBody;
        public GameObject[] femaleArms;
        public GameObject[] femaleLegs;
        public GameObject femaleEyebrows;

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


    }

}
