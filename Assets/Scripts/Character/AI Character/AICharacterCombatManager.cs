using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        protected AICharacterManager aiCharacter;

        [Header("Action Recovery")]
        public float actionRecoveryTimer;

        [Header("Pivot")]
        public bool enablePivot = true;

        [Header("Target Information")]
        public float distanceFromTarget;
        public float viewableAngle;
        public Vector3 targetsDirection;

        [Header("Detection")]
        [SerializeField] float detectionRadius = 15;
        public float minimumFOV = -35;
        public float maximumFOV = 35;

        [Header("Attack Rotation Speed")]
        public float attackRotationSpeed = 25;

        [Header("Stance Settings")]
        public float maxStance;
        public float currentStance;
        [SerializeField] float stanceregeneratedPersecond = 15;
        [SerializeField] bool ignoreStanceBreak;

        [Header("Stance Timer")]
        [SerializeField] float stanceRegenerationTimer = 0;
        private float stanceTickTimer = 0;
        [SerializeField] float defaultTimeUntilStanceRegenerationbegins = 15;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        private void FixedUpdate()
        {
            HandleStanceBreak();
        }

        private void HandleStanceBreak()
        {
            if (!aiCharacter.IsOwner)
                return;

            if (aiCharacter.isDead.Value)
                return;

            if (stanceRegenerationTimer > 0)
            {
                stanceRegenerationTimer -= Time.deltaTime;
            }
            else
            {
                stanceRegenerationTimer = 0;

                if (currentStance < maxStance)
                {
                    // begin adding stance each tick
                    stanceTickTimer += Time.deltaTime;

                    if (stanceTickTimer >= 1)
                    {
                        stanceTickTimer = 0;
                        currentStance += stanceregeneratedPersecond;
                    }
                }
                else
                {
                    currentStance = maxStance;
                }

            }

            if (currentStance <= 0)
            {
                // TO DO (OPTIONAL) IF WE ARE IN A VERY HIGH INTENSITY DAMAGE ANIMATION (LIKE BEING LAUCHED INTO THE AIR) DO NOT PLAY THE STANCE BREAK ANIMATION
                // THIS WOULD FEEL LESS IMPACTFUL IN GAMEPLAY

                DamageIntensity previousDamageIntensity = WorldUtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(previousPoiseDamageTaken);

                if (previousDamageIntensity == DamageIntensity.Colossal)
                {
                    currentStance = 1;
                }

                // IF WE ARE BEING STABBED OR BEING RIPOSTED (CRITICALLY DAMAGED) DO NOT PAY THE STANCE BREAK ANIM, AS THIS WOULD BREAK THE STATE

                currentStance = maxStance;

                if (ignoreStanceBreak)
                    return;

                aiCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Stance_Break_01", true);

                currentStance = maxStance;



            }

        }

        public void DamageStance(int stanceDamage)
        {
            // WHEN STANCE IS DAMAGED, THE TIMER IS RESET, MEANING CONSTANT ATTACKS GIVE NO CHANCE AT RECOVERING STANCE THAT IS LOST
            stanceRegenerationTimer = defaultTimeUntilStanceRegenerationbegins;

            currentStance -= stanceDamage;
        }

        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position,
                detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null)
                {
                    continue;
                }
                if (targetCharacter == aiCharacter)
                {
                    //Debug.Log("target is this aiCharacter, continued");
                    continue;
                }
                if (targetCharacter.isDead.Value)
                {
                    continue;
                }

                //  CAN I ATTACK THIS 
                if (WorldUtilityManager.instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                {
                    //Debug.Log("AI CHARACTER COMBAT MANAGER   >>>  CAN I DAMAGE THIS TARGET TRUE");
                    //  IF A POTENTIAL TARGET IS FOUND, IT HAS TO BE IN FROT OF US
                    Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                    if (angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                    {
                        //Debug.Log("AI CHARACTER COMBAT MANAGER   >>>  " +
                        //    "viewable:  " + viewableAngle + "   " + "  min: " + minimumDetectionAngle + "  max:  " + maximumDetectionAngle);
                        // LASTLY WE CHECK FOR ENVIRO BLOCKS
                        if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position,
                            targetCharacter.characterCombatManager.lockOnTransform.position,
                            WorldUtilityManager.instance.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position,
                            targetCharacter.characterCombatManager.lockOnTransform.position);
                            //Debug.Log("Blocked");
                        }
                        else
                        {
                            //Debug.Log("Not Blocked");
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);

                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);

                            if (enablePivot)
                            {
                                PivotTowardsTarget(aiCharacter);
                            }
                            
                        }
                    }

                }
                //if (!WorldUtilityManager.instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                //{
                //    Debug.Log("attacking: " + character.name + "  target:  " + targetCharacter.name + "  aichar name:  " + aiCharacter);
                //}

            }

        }

        public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            //  PLAY A PIVOT ANIMATION  DEPENDING ON THE VIEWABLE ANGLE OF THE TARGET
            if (aiCharacter.isPerformingAction)
            {
                Debug.Log("ai char combat manager >>>      is performing action, returned");
                return;
            }

            if (viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_45", true);
            }
            else if (viewableAngle <= -20 && viewableAngle >= -60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_45", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle >= 111 && viewableAngle <= 145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_135", true);
            }
            else if (viewableAngle <= -111 && viewableAngle >= -145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_135", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }

        }

        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }

        public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
        {
            if (currentTarget == null)
            {
                return;
            }

            if (!aiCharacter.characterLocomotionManager.canRotate)
            {
                return;
            }

            if (!aiCharacter.isPerformingAction)
            {
                return;
            }

            Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();
            
            if (targetDirection == Vector3.zero)
            {
                targetDirection = aiCharacter.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);


        }

        public void HandleActionRecovery(AICharacterManager aiCharacter)
        {
            if (actionRecoveryTimer > 0)
            {
                if (!aiCharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }

    }
}

