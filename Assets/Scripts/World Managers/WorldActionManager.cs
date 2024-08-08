using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AS
{
    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager instance;

        [Header("Weapon Item Actions")]
        public WeaponItemAction[] weaponItemActions;

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

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            for (int i = 0; i < weaponItemActions.Length; i++)
            {
                weaponItemActions[i].actionID = i;
            }
        }

        public WeaponItemAction GetWeaponItemActionByID(int ID)
        {
            return weaponItemActions.FirstOrDefault(action => action.actionID == ID);
        }

    }

}
