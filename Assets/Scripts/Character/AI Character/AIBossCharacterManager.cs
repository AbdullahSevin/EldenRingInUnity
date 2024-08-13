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
        [SerializeField] bool hasBeenAwakened = false;
        [SerializeField] List<FogWallInteractable> fogwalls;

        //  GIVE THIS AI A UNIQUE ID
        //  WHEN THE AI IS SPAWNED, CHECK OUR SAVE FILE 
        //  IF THE SAVE FILE DOES NOT CONTAIN A BOSS MONSTER WITH THIS I.D. ADD IT
        //  IF IT IS PRESEN, CHECK IF THE BOS HAS BEEN DEFEATED 
        //  IF THE BOSS HAS BEEN DEFEATED, DISABLE THIS GAMEOBJECT
        //  IF THE BOSS HAS NOT BEEN DEFEATED, ALLOW THIS OBJECT TO CONTINUE BEING ACTIVE

        [Header("DEBUG")]
        [SerializeField] bool defeatedBossDebug = false;
        [SerializeField] bool wakeBossUp = false;

        protected override void Update()
        {
            base.Update();

            if (wakeBossUp)
            {
                wakeBossUp = false;
                WakeBoss();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            // IF THIS IS THE HOST'S WORLD
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
                    hasBeenAwakened = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];

                }

                //  YOU CAN EITHER SHARE THE SAME ID FOR THE BOSS AND THE FOG WALL, OR SIMPLY PLACE A FOGWALL ID VARIABLE HERE ON LOOK FOR USING THAT
                //  LOCATE FOG WALL
                StartCoroutine(GetFogWallsFromWorldObjectManager());

                //  IF THE BOSS HAS BEEN AWAKENED, ENABLE THE FOG WALLS
                if (hasBeenAwakened)
                {
                    for (int i = 0; i < fogwalls.Count; i++)
                    {
                        fogwalls[i].isActive.Value = true;
                    }
                }

                //  IF THE BOSS HAS BEEN DEFEATED DISABLE THE FOG WALLS
                if (hasBeenDefeated)
                {
                    for (int i = 0; i < fogwalls.Count; i++)
                    {
                        fogwalls[i].isActive.Value = false;
                    }
                    aiCharacterNetworkManager.isActive.Value = false;
                }

            }
        }


        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.instance.fogWalls.Count == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            fogwalls = new List<FogWallInteractable>();

            foreach (var fogwall in WorldObjectManager.instance.fogWalls)
            {
                if (fogwall.fogWallID == bossID)
                {
                    fogwalls.Add(fogwall);
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

        public void WakeBoss()
        {
            hasBeenAwakened = true;
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }
            
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                
            }

            for (int i = 0; i < fogwalls.Count; i++)
            {
                fogwalls[i].isActive.Value = true;
            }

        }



    }
}

