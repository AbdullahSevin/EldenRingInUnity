using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class AIDurkCharacterManager : AIBossCharacterManager
    {
        // HWY GIVE DURK HIS OWN CHARACTER MANAGER?
        // OUR CHARACTER MANAGERS ACT AS A HUB TO WHERE WE CAN REFERENCE ALL COMPONENTS OF A CHARACTER
        // A "PLAYER" MANAGER FOR EXAMPLE WILL HAVE ALL THE COMPONENTS OF A PLAYER CHARACTER
        // AN UNDEAD WILL HAVE ALL THE UNIQUE COMPONENTS OF AN UNDEAD
        // SINCE DURK HAS HIS OWN SFX (CLUB, STOMP) THAT ARE UNIQUE TO HIS CHARACTER ONLY, WE CREATED A DURK SFX MANAGER
        // AND TO REFERENCE THIS NEW SFX MANAGER, AND TO KEEP OUR DESIGN PATTERN THE SAME, WE NEED A DURK CHARACTER MANAGER TO REFERENCE IT FROM

        [HideInInspector] public AIDurkSoundFXManager durkSoundFXManager;
        [HideInInspector] public AIDurkCombatManager durkCombatManager;

        protected override void Awake()
        {
            base.Awake();

            durkSoundFXManager = GetComponent<AIDurkSoundFXManager>();
            durkCombatManager = GetComponent<AIDurkCombatManager>();
        }

    }
}

