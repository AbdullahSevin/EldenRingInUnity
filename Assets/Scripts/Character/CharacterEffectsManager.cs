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

    }

}

