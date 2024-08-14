using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.PlasticSCM.Editor.WebApi;


namespace AS
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;

        [Header("Music")]
        [SerializeField] AudioClip bossIntroClip;
        [SerializeField] AudioClip bossBattleLoopClip;

        [Header("Status")]
        public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] List<FogWallInteractable> fogwalls;
        [SerializeField] string sleepAnimation;
        [SerializeField] string awakenAnimation;

        [Header("Phase Shift")]
        public float minimumHealthPercentageToShift = 50;
        [SerializeField] string phaseShiftAnimation = "Phase_Change_01";
        [SerializeField] CombatStanceState phase02CombatStanceState;

        [Header("States")]
        [SerializeField] BossSleepState sleepState;

        //  GIVE THIS AI A UNIQUE ID
        //  WHEN THE AI IS SPAWNED, CHECK OUR SAVE FILE 
        //  IF THE SAVE FILE DOES NOT CONTAIN A BOSS MONSTER WITH THIS I.D. ADD IT
        //  IF IT IS PRESEN, CHECK IF THE BOS HAS BEEN DEFEATED 
        //  IF THE BOSS HAS BEEN DEFEATED, DISABLE THIS GAMEOBJECT
        //  IF THE BOSS HAS NOT BEEN DEFEATED, ALLOW THIS OBJECT TO CONTINUE BEING ACTIVE

        protected override void Awake()
        {
            base.Awake();
            
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
            OnBossFightIsActiveChanged(false, bossFightIsActive.Value); // SO IF YOU JOIN WHEN THE FIGHT IS ALREADY ACTIVE, YOU WILL GET A HP BAR.

            if (IsOwner)
            {
                sleepState = Instantiate(sleepState);
                currentState = sleepState;
            }

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
                    hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];

                }

                //  YOU CAN EITHER SHARE THE SAME ID FOR THE BOSS AND THE FOG WALL, OR SIMPLY PLACE A FOGWALL ID VARIABLE HERE ON LOOK FOR USING THAT
                //  LOCATE FOG WALL
                StartCoroutine(GetFogWallsFromWorldObjectManager());

                //  IF THE BOSS HAS BEEN AWAKENED, ENABLE THE FOG WALLS
                if (hasBeenAwakened.Value)
                {
                    for (int i = 0; i < fogwalls.Count; i++)
                    {
                        fogwalls[i].isActive.Value = true;
                    }
                }

                //  IF THE BOSS HAS BEEN DEFEATED DISABLE THE FOG WALLS
                if (hasBeenDefeated.Value)
                {
                    for (int i = 0; i < fogwalls.Count; i++)
                    {
                        fogwalls[i].isActive.Value = false;
                    }
                    aiCharacterNetworkManager.isActive.Value = false;
                }

            }

            if (!hasBeenAwakened.Value)
            {
                characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
            }


        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
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
            PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("GREAT FOE FELLED");
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                bossFightIsActive.Value = false;

                foreach (var fogwall in fogwalls)
                {
                    fogwall.isActive.Value = false;
                }

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
            if (IsOwner)
            {

                if (!hasBeenAwakened.Value)
                {
                    characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
                }
                bossFightIsActive.Value = true;
                hasBeenAwakened.Value = true;
                currentState = idle;
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

        private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (bossFightIsActive.Value)
            {
                WorldSoundFXManager.instance.PlayBossTrack(bossIntroClip, bossBattleLoopClip);

                GameObject bossHealthbar = Instantiate(PlayerUIManager.instance.playerUIHudManager.bossHealthBarObject,
                PlayerUIManager.instance.playerUIHudManager.bossHealthBarParent);

                UI_Boss_HP_Bar bossHPBar = bossHealthbar.GetComponentInChildren<UI_Boss_HP_Bar>();
                bossHPBar.EnableBossHPBar(this);
            }
            else
            {
                WorldSoundFXManager.instance.StopBossMusic();
            }

        }

        public void PhaseShift()
        {
            characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAnimation, true);
            combatStance = Instantiate(phase02CombatStanceState);
            currentState = combatStance;
        }

    }
}

