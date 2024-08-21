using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
  
}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT
}

public enum CharacterGroup
{
    Team01,
    Team02
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHandWeaponSlot,
    LeftHandShieldSlot,
    BackSlot
    //Right Hips,
    //Left Hips,
}

public enum WeaponModelType
{
    Weapon,
    Shield
}

public enum EquipmentModelType
{
    FullHelmet,     // WOULD ALWAYS HIDE FACE HAIR ETC
    Hat,     // WOULD ALWAYS HAIR
    Hood,            // WOULD ALWAYS HAIR
    HelmetAcessorie,
    FaceCover,
    Torso,
    Back,
    RightShoulder,
    RightUpperArm,
    RightElbow,
    RightLowerArm,
    RightHand,
    LeftShoulder,
    LeftUpperArm,
    LeftElbow,
    LeftLowerArm,
    LeftHand,
    Hips,
    HipsAttachment,
    RightLeg,
    RightKnee,
    LeftLeg,
    LeftKnee

}

public enum EquipmentType
{
    RightWeapon01,
    RightWeapon02,
    RightWeapon03,
    LeftWeapon01,
    LeftWeapon02,
    LeftWeapon03
}

public enum HeadEquipmentType
{
    FullHelmet,   // HIDE ENTIRE HEAD + FEATURES
    Hat,          // DOESNT HIDE ANYTHING
    Hood,         // HIDES HAIR
    FaceCover     // HIDES BEARD
}

public enum WeaponClass
{
    StraightSword,
    Spear,
    MediumShield,
    Fist
}


//  THIS IS USED TO CALCULATE DAMAGE BASED ON ATTACK TYPE
public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    HeavyAttack02,
    ChargedAttack01,
    ChargedAttack02,
    RunningAttack01,
    RollingAttack01,
    BackstepAttack01
}

public enum DamageIntensity
{
    Ping,
    Light,
    Medium,
    Heavy,
    Colossal
}


// USED TO DETERMINE ITEM PICUP TYPE
public enum ItemPickupType
{
    WorldSpawn,
    EnemyDrop
}