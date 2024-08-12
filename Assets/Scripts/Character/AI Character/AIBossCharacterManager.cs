using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;


namespace AS
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;
        [SerializeField] bool hasBeenDefeated = false;

        //  GIVE THIS AI A UNIQUE ID
        //  WHEN THE AI IS SPAWNED, CHECK OUR SAVE FILE 
        //  IF THE SAVE FILE DOES NOT CONTAIN A BOSS MONSTER WITH THIS I.D. ADD IT
        //  IF IT IS PRESEN, CHECK IF THE BOS HAS BEEN DEFEATED 
        //  IF THE BOSS HAS BEEN DEFEATED, DISABLE THIS GAMEOBJECT
        //  IF THE BOSS HAS NOT BEEN DEFEATED, ALLOW THIS OBJECT TO CONTINUE BEING ACTIVE

        [Header("TEST")]
        [SerializeField] bool defeatedBossDebug = false;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // IF THIS OSTHE HOST'S WORLD
            if (IsServer)
            {
                //  IF OUR  SAVE DATA  DOES NOT CONTAIN INFO ON THIS BOSS, ADD IT NOW
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                //  OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
                else
                {
                    hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];

                    if (hasBeenDefeated)
                    {
                        aiCharacterNetworkManager.isActive.Value = false;
                    }
                }
            }
        }


        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                //  RESET ANY FLAGS HERE THAT NEED TO BE RESET
                //  NOTHING YET

                //  IF WE ARE NOT GROUNDED, PLAY AN AERIAL DEATH ANIMATION

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }

                //  IF OUR  SAVE DATA  DOES NOT CONTAIN INFO ON THIS BOSS, ADD IT NOW
                if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                //  OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
                else
                {
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                    WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                WorldSaveGameManager.instance.SaveGame();

            }



            // PLAY SOME DEATH SFX



            //  AWARD PLAYERS WITH RUNES

            //  DISABLE CHARACTER
            yield return new WaitForSeconds(5);





        }
    }
}

