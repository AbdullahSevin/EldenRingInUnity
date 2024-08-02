using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID;


        public virtual void ProcessEffect(CharacterManager character)
        {

        }

    }
}

