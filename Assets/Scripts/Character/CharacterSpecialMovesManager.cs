using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AS
{
    public class CharacterSpecialMovesManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Special Move")]
        public GameObject kamehamehaHandTransformObject;
        public GameObject kamehamehaReleaseTransformObject;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }



        public virtual void AttemptToPerformSpecialMove(SpecialMove specialMove)
        {

        }


    }
}

