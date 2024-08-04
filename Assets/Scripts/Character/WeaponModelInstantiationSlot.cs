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

        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;

        }





    }
}

