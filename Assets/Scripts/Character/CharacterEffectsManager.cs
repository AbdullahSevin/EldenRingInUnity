using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        //  PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL ...)


        //  PROCESS TIMED EFFECTS (POISON, BUILD UPS ...)


        //  PROCESS STATIC EFFECTS (ADDING REMOVING BUFFS FROM TALISMANS ETC)

        CharacterManager character;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;
        [SerializeField] GameObject criticalBloodSplatterVFX;
        [SerializeField] GameObject kamehameha_Small_Charge_VFX;
        [SerializeField] GameObject kamehameha_Charge_VFX;
        [SerializeField] GameObject kamehameha_Burst_VFX;
        [SerializeField] GameObject kamehameha_Beam_VFX;

        [Header("Static Effects")]
        public List<StaticCharacterEffect> staticEffects =  new List<StaticCharacterEffect>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character); 
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            //  IF WE MANUALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            //  ELSE, USE THE GENERIC (DEFAULT VERSION) WE HAVE ELSEWHERE
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }

        }

        public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)
        {
            //  IF WE MANUALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            //  ELSE, USE THE GENERIC (DEFAULT VERSION) WE HAVE ELSEWHERE
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }

        }

        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            // IF YOU WANT TO SYNC EFFECTS ACROSS NETWORK, IF YOU ARE THE OWNER LAUNCH A SERVER RPC HERE TO PROCESS THE EFFECT ON ALL OTHER CLIENTS
            
            // 1. ADD A STATIC EFFECT TO THE CHARACTER
            staticEffects.Add(effect);

            // 2. PROCESS ITS EFFECT
            effect.ProcessStaticEffect(character);

            // 3. CHECK FOR NULL ENTRIES IN YOUR LIST AND REMOVE THEM
            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                {
                    staticEffects.RemoveAt(i);
                }
            }
        }

        public void RemoveStaticEffect(int effectID)
        {
            // IF YOU WANT TO SYNC EFFECTS ACROSS NETWORK, IF YOU ARE THE OWNER LAUNCH A SERVER RPC HERE TO PROCESS THE EFFECT ON ALL OTHER CLIENTS

            

            StaticCharacterEffect effect;

            for (int i = 0; i < staticEffects.Count; i++)
            {
                if (staticEffects[i] != null)
                {
                    if (staticEffects[i].staticEffectID == effectID)
                    {
                        effect = staticEffects[i];
                        // 1. REMOVE STATIC EFFECT FROM CHARACTER
                        effect.RemoveStaticEffect(character);
                        // 2. REMOVE STATIC EFFECT FROM LIST
                        staticEffects.Remove(effect);
                    }
                }
            }

            // 3. CHECK FOR NULL ENTRIES IN YOUR LIST AND REMOVE THEM
            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                {
                    staticEffects.RemoveAt(i);
                }
            }

        }


        public void PlayKamehamehaSmallChargeVFX()
        {
            GameObject kamehamehaHandObject = character.characterSpecialMovesManager.kamehamehaHandTransformObject;
            Debug.Log("releasepoint:   " + kamehamehaHandObject);
            //  IF WE MANUALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if (kamehameha_Charge_VFX != null)
            {
                GameObject kamehamehaWave = Instantiate(kamehameha_Small_Charge_VFX, kamehamehaHandObject.transform.position, Quaternion.identity);
                kamehamehaWave.transform.SetParent(kamehamehaHandObject.transform);

            }
            //  ELSE, USE THE GENERIC (DEFAULT VERSION) WE HAVE ELSEWHERE
            else
            {
                GameObject kamehamehaWave = Instantiate(WorldCharacterEffectsManager.instance.kamehameha_Small_Charge_VFX, kamehamehaHandObject.transform.position, Quaternion.identity);
                kamehamehaWave.transform.SetParent(kamehamehaHandObject.transform);

            }

        }


        public void PlayKamehamehaChargeVFX()
        {
            GameObject kamehamehaHandObject = character.characterSpecialMovesManager.kamehamehaHandTransformObject;
            Debug.Log("releasepoint:   " + kamehamehaHandObject);
            //  IF WE MANUALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if (kamehameha_Charge_VFX != null)
            {
                GameObject kamehamehaWave = Instantiate(kamehameha_Charge_VFX, kamehamehaHandObject.transform.position, Quaternion.identity);
                kamehamehaWave.transform.SetParent(kamehamehaHandObject.transform);

            }
            //  ELSE, USE THE GENERIC (DEFAULT VERSION) WE HAVE ELSEWHERE
            else
            {
                GameObject kamehamehaWave = Instantiate(WorldCharacterEffectsManager.instance.kamehameha_Charge_VFX, kamehamehaHandObject.transform.position, Quaternion.identity);
                kamehamehaWave.transform.SetParent(kamehamehaHandObject.transform);

            }

        }

        public void PlayKamehamehaBurstVFX()
        {
            GameObject releasePointObject = character.characterSpecialMovesManager.kamehamehaReleaseTransformObject;
            //  IF WE MANUALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if (kamehameha_Burst_VFX != null)
            {
                GameObject kamehamehaWave = Instantiate(kamehameha_Burst_VFX, releasePointObject.transform.position, Quaternion.identity);
                kamehamehaWave.transform.SetParent(releasePointObject.transform);

            }
            //  ELSE, USE THE GENERIC (DEFAULT VERSION) WE HAVE ELSEWHERE
            else
            {
                GameObject kamehamehaWave = Instantiate(WorldCharacterEffectsManager.instance.kamehameha_Burst_VFX, releasePointObject.transform.position, Quaternion.identity);
                kamehamehaWave.transform.SetParent(releasePointObject.transform);
            }

        }

        public void PlayKamehamehaBeamVFX()
        {
            GameObject releasePointObject = character.characterSpecialMovesManager.kamehamehaReleaseTransformObject;
            Vector3 kamehamehaDirection = character.characterSpecialMovesManager.kamehamehaReleaseTransformObject.transform.forward;
            //  IF WE MANUALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if (kamehameha_Beam_VFX != null)
            {
                GameObject kamehamehaWave = Instantiate(kamehameha_Beam_VFX, releasePointObject.transform.position, Quaternion.LookRotation(kamehamehaDirection));
                kamehamehaWave.transform.SetParent(releasePointObject.transform);

            }
            //  ELSE, USE THE GENERIC (DEFAULT VERSION) WE HAVE ELSEWHERE
            else
            {
                GameObject kamehamehaWave = Instantiate(WorldCharacterEffectsManager.instance.kamehameha_Beam_VFX, releasePointObject.transform.position, Quaternion.LookRotation(kamehamehaDirection));
                kamehamehaWave.transform.SetParent(releasePointObject.transform);

            }

        }


    }

}

