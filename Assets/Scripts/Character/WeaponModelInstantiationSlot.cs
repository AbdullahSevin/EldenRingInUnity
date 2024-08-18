using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {

        public WeaponModelSlot weaponSlot;
        // WHAT SLOT IS THIS? LEFT HAND OR RÝGHT, OR HIPS OR BACK_)
        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }

        }

        public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;

        }

        public void PlaceWeaponModelInUnequippedSlot(GameObject weaponModel, WeaponClass weaponClass, PlayerManager player)
        {
            // TO DO, MOVE WEAPON ON BACK CLOSER OR MORE OTWARD DEPENDING ON THE CHEST EQUIPMENT (SO IT DOESN'T APPEAR TO FLOAT)

            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            switch (weaponClass)
            {
                case WeaponClass.StraightSword:
                    weaponModel.transform.localPosition = new Vector3(0, 0, 0);
                    weaponModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case WeaponClass.Spear:
                    weaponModel.transform.localPosition = new Vector3(0, 0, 0);
                    weaponModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case WeaponClass.MediumShield:
                    weaponModel.transform.localPosition = new Vector3(0, 0, 0);
                    weaponModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
            }






        }



    }
}

