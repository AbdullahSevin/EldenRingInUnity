using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class AshOfWar : Item
    {

        [Header("Ash Of War Information")]
        public WeaponClass[] usableWeaponClasses;


        [Header("Costs")]
        public int focusCost = 20;
        public int StaminaCost = 20;


        //  THE FUNCTION ATTEMPTING TO PERFORM THE ASH OF WAR
        public virtual void AttempToPerformAction(PlayerManager playerPerformingAction)
        {
            Debug.Log("Performed");
        }

        public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            return false;
        }


        protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)
        {
            playerPerformingAction.playerNetworkManager.currentStamina.Value -= StaminaCost;
        }

        protected virtual void DeductFocusPointCost(PlayerManager playerPerformingAction)
        {
            // playerPerformingAction.playerNetworkManager.currentFocus.Value -= focusCost;
        }


    }
}

