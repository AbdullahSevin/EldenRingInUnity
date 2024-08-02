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

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character); 
        }



    }

}

