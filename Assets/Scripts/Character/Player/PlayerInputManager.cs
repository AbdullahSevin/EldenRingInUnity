using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;


namespace AS
    // just trying my new wireless keyboard, ignore this line.
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;

        // get controller type from settings (later), for now use bool => true: mouse - false: joystick
        public bool isUsingMouseKeyboard = true;

        // 1 - read input values
        // 2 - move according to the input values
        PlayerControls playerControls;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] Vector2 cameraInput;
        // FOR JOYSTICK
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("LOCK ON INPUT")]
        [SerializeField] bool lockOn_Input;
        [SerializeField] bool lockOn_Left_Input;
        [SerializeField] bool lockOn_Right_Input;
        private Coroutine lockOnCoroutine;

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("PLAYER ACTION INPUT")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool interaction_Input = false;


        [Header("EQUIPMENT INPUTS")]
        [SerializeField] bool switch_Right_Weapon_Input = false;
        [SerializeField] bool switch_Left_Weapon_Input = false;

        [Header("BUMPER INPUTS")]
        [SerializeField] bool RB_Input = false;
        [SerializeField] bool LB_Input = false;

        [Header("TRIGGER INPUTS")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;

        [Header("BUMPER INPUTS")]
        [SerializeField] bool two_Hand_Input = false;
        [SerializeField] bool two_Hand_Right_Weapon_Input = false;
        [SerializeField] bool two_Hand_Left_Weapon_Input = false;


        [Header("QUED INPUTS")]
        [SerializeField] bool input_Que_Is_Active = false;
        [SerializeField] float default_Que_Input_Time = 0.35f;
        [SerializeField] float que_Input_Timer = 0;
        [SerializeField] bool que_RB_Input = false;
        [SerializeField] bool que_RT_Input = false;
        



        private void Awake()
        {

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // WHEN THE SCENE CHANGES RUN THIS LOGIC
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
            
        }

        private void OnSceneChange (Scene oldScene, Scene newScene)
        {
            // IF WE ARE LOADING TO OUR WORLD SCENE, ENABLE OUR PLAYER CONTROLS
            // use a list and "in", later if you end up having multiple worlds ? 
                // or make it false in case of index == main menu, and use "else" to make it true on all other scenes 
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE OUR PLAYER CONTROLS
            // THIS IS TO PREVENT PLAYER MOVEMENT WHILE NOT BEING IN WORLD SCENE (MAIN MENU SETTINGS MENU ETC)
            else
            {
                instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls ==null)
            {
                playerControls = new PlayerControls();


                // COMMON INPUTS: THESE CODES DOES THE SAME THINGS FOR BOTH JOYSTICK AND KEYBOARD
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();


                //  ACTIONS 
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;

                // EQUIPMENT INPUTS
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;


                //  BUMPERS
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                playerControls.PlayerActions.LB.performed += i => LB_Input = true;
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isBlocking.Value = false;

                //  TRIGGERS
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;

                //  TWO HAND
                playerControls.PlayerActions.TwoHandWeapon.performed += i => two_Hand_Input = true;
                playerControls.PlayerActions.TwoHandWeapon.canceled += i => two_Hand_Input = false;
                playerControls.PlayerActions.TwoHandRightWeapon.performed += i => two_Hand_Right_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => two_Hand_Right_Weapon_Input = false;
                playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => two_Hand_Left_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => two_Hand_Left_Weapon_Input = false;

                //  LOCK ON 
                playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;



                // HOLD INPUTS: ACTIVATES / DE-ACTIVATES ACTIONS (SETS BOOLS TRUE or FALSE)
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

                

                // if using joystick do these:
                if (!isUsingMouseKeyboard)
                {
                    playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

                }
                else
                // IF USING MOUSE DO THESE:
                {
                    
                    playerControls.PlayerCamera.MovementMouse.performed += i => PlayerCamera.instance.isOnFreeViewMode = true;
                    playerControls.PlayerCamera.MovementMouse.canceled += i => PlayerCamera.instance.isOnFreeViewMode = false;

                }

                //  QUED INPUTS
                playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);
                playerControls.PlayerActions.QueRT.performed += i => QueInput(ref que_RT_Input);


            }
            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        // IF WE MINIMIZE OR LOWER THE WINDOW, STOP ADJUSTING INPUTS
        private void OnApplicationFocus(bool Focus)
        {
            if (enabled)
            {
                if (Focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandeSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleLBInput();
            HandleRTInput();
            HandleChargeRTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
            HandleQuedInputs();
            HandleInteractionInput();
        }

        // TWO HAND
        
        private void HandleTwoHandInput()
        {
            if (!two_Hand_Input)
            {
                return;
            }

            if (two_Hand_Right_Weapon_Input)
            {
                // IF WE ARE USING THE TWO HAND INPUT AND PRESSING THE RIGHT TWO HAND BUTTON WE WAN TO TOP THE REGULAR RB INPUT (OR ELSE W WOULD ATACK)
                RB_Input = false;
                two_Hand_Right_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    // IF WE ARE TWO HANDING WEAPON ALREADY, CHANGE THE IS TWOHANDNG BOOL TO FALSE, WHICH TRIGGERS AN "ONVALUECHANGED" FUNCTION, WHICH UN-TWOHANDS CURRENT WEAPON
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    // IF WE ARE NOT ALREADY TWO HANDING CHANGE THE RIGHT HAND TWO HAND BOOL TO TRUE WHICH TRIGGERS AN "ONVALUECHANGED"
                    // THIS FNCTION TWO HANDS THE RIGHT WEAPON
                    player.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                    return;
                }
            }
            else if (two_Hand_Left_Weapon_Input)
            {
                // IF WE ARE USING THE TWO HAND INPUT AND PRESSING THE RIGHT TWO HAND BUTTON WE WAN TO TOP THE REGULAR RB INPUT (OR ELSE W WOULD ATACK)
                LB_Input = false;
                two_Hand_Left_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    // IF WE ARE TWO HANDING WEAPON ALREADY, CHANGE THE IS TWOHANDNG BOOL TO FALSE, WHICH TRIGGERS AN "ONVALUECHANGED" FUNCTION, WHICH UN-TWOHANDS CURRENT WEAPON
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    // IF WE ARE NOT ALREADY TWO HANDING CHANGE THE RIGHT HAND TWO HAND BOOL TO TRUE WHICH TRIGGERS AN "ONVALUECHANGED"
                    // THIS FNCTION TWO HANDS THE RIGHT WEAPON
                    player.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                    return;
                }
            }



        }


        //  LOCK ON

        private void HandleLockOnInput()
        {
            //  CHECK FOR DEAD TARGET
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if (player.playerCombatManager.currentTarget == null)
                {
                    // Debug.Log("PLAYER INPUT MANAGER     >>>     NO CUR TARGET RETURNED ");
                    return;
                }
                if (player.playerCombatManager.currentTarget.isDead.Value)
                {
                    player.playerNetworkManager.isLockedOn.Value = false;
                }

                //  ATTEMPT TO FIND NEW TARGET 

                //  THIS ASSURES US THAT THE COROUTINE NEVER RUNS MULTIPLE TIMES OVERLAPPING ITSELF 
                if (lockOnCoroutine != null)
                {
                    StopCoroutine(lockOnCoroutine);
                }

                lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
            }

            if (lockOn_Input && player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;
                //  DISABLE LOCK ON
                return;
            }

            if (lockOn_Input && !player.playerNetworkManager.isLockedOn.Value)
            {
                lockOn_Input = false;
                PlayerCamera.instance.ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;

                //  IF WE ARE AIMING USING RANGED WEPON RETURN (DO NOT ALLOW LOCK ON WHILST AIMING)


                // Debug.Log("PLAYER INPUT MANAGER     >>>  CALLED HANDLE LOCATING LOCK ON TARGETS");
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.nearestLockOnTarget != null)
                {
                    //  SET THE TARGET AS OUR CURRENT TARGET 
                    // Debug.Log("PLAYER INPUT MANAGER     >>>     HANDLE LOCATING LOCKON TRAGETS >>> NEAREST TARGET HAS SET");
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                    player.playerNetworkManager.isLockedOn.Value = true;
                }
            }


        }

        private void HandleLockOnSwitchTargetInput()
        {
           if (lockOn_Left_Input)
            {
                lockOn_Left_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                    }
                }
            }

            if (lockOn_Right_Input)
            {
                lockOn_Right_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.instance.HandleLocatingLockOnTargets();

                    if (PlayerCamera.instance.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                    }
                }
            }

        }

        // MOVEMENT
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // RETURNS THE ABSOULETE NUMBER, DISTANCE (REMOVING THE MINUS SIGN, SO ITS ALWAYS POSITIVE)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // CLAMP THE moveAmount VALUES 0, 0.5, 1
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ONLY WANT NON-STRAFING MOVEMENT
            // WE USE THE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON

            if (player == null)
            {
                return;
            }

            if (moveAmount != 0)
            {
                player.playerNetworkManager.isMoving.Value = true;
            }
            else
            {
                player.playerNetworkManager.isMoving.Value = false;
            }

            // IF WE ARE NOT LOCKED ON, ONLY USE THE MOVEAMOUNT
            if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
            }


            //IF WE ARE LOCKED ON PASS THE HORIZONTAL MOVEMENT AS WELL
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

       
        // ACTION
        private void HandleDodgeInput()
        {
            if (dodgeInput == true)
            {
                dodgeInput = false;

                //      FUTURE NOTE: RETURN (DO NOTHING) IF MENU OR AN UI WINDOW IS OPEN 
                // PERFORM A DODGE

                player.playerLocomotionManager.AttemptToPerformDodge();

            }
        }

        private void HandeSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();

            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                // IF UI WİNDOW IS OPEN RETURN

                // ATTEMPT TO PERFORM JUMP
                player.playerLocomotionManager.AttemptToPerformJump();
            }

            
        }

        private void HandleRBInput()
        {
            if (two_Hand_Input)
            {
                return;
            }
            if (RB_Input)
            {
                RB_Input = false;

                //  TO DO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerNetworkManager.SetCharacterActionHand(true);

                //  TO DO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action,
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleLBInput()
        {
            if (two_Hand_Input)
            {
                return;
            }

            if (LB_Input)
            {
                LB_Input = false;

                //  TO DO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerNetworkManager.SetCharacterActionHand(false);

                //  TO DO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentLeftHandWeapon.oh_LB_Action,
                    player.playerInventoryManager.currentLeftHandWeapon);
            }
        }

        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;

                //  TO DO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerNetworkManager.SetCharacterActionHand(true);

                //  TO DO: IF WE ARE TWO HANDING THE WEAPON, USE THE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(
                    player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action,
                    player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleChargeRTInput()
        {
            //  WE ONLY WANT TO CHECK FOR A CHARGE IF WE ARE IN AN ACTION THAT REQUIRES IT (Attacking)
            if (player.isPerformingAction)
            {
                // Debug.Log($"using right hand?: {player.playerNetworkManager.isUsingRightHand.Value}");
                if (player.playerNetworkManager.isUsingRightHand.Value)
                {
                    // Debug.Log("Hold_RT_Input:" + Hold_RT_Input);
                    player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
                }
            }
            
        }

        private void HandleSwitchRightWeaponInput()
        {
            if (switch_Right_Weapon_Input)
            {
                switch_Right_Weapon_Input = false;
                if (player.isDead.Value)
                {
                    return;
                }
                player.playerEquipmentManager.SwitchRightWeapon();
            }
            
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switch_Left_Weapon_Input)
            {
                switch_Left_Weapon_Input = false;
                if (player.isDead.Value)
                {
                    return;
                }
                player.playerEquipmentManager.SwitchLeftWeapon();
            }

        }

        private void HandleInteractionInput()
        {
            if (interaction_Input)
            {
                interaction_Input = false;
                player.playerInteractionManager.Interact();
                //player.playerInteractionManager.CheckForInteractable();
            }
        }

        private void QueInput(ref bool quedInput)
        {
            //  RESET ALL QUED INPUTS SO ONLY ONE CAN QUE AT A TIME
            que_RB_Input = false;
            que_RT_Input = false;
            // que_LB_Input = false;
            // que_LT_Input = false;

            //  CHECK FOR UI WINDOW BEING OPEN, IF ITS OPEN RETURN

            if (player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
            {
                quedInput = true;
                que_Input_Timer = default_Que_Input_Time;
                input_Que_Is_Active = true;
            }

        }

        private void ProcessQuedInput()
        {
            if (player.isDead.Value)
            {
                return;
            }

            if (que_RB_Input)
            {
                RB_Input = true;
            }

            if (que_RT_Input)
            {
                RT_Input = true;
            }
        }

        private void HandleQuedInputs()
        {
            if (input_Que_Is_Active)
            {
                //  WHILE THE TIMER IS ABOVE 0, KEEP ATTEMPTING TO PRESS THE INPUT
                if (que_Input_Timer > 0)
                {
                    que_Input_Timer -= Time.deltaTime;
                    ProcessQuedInput();
                }
                else
                {
                    //  RESET ALL QUED INPUTS
                    que_RB_Input = false;
                    que_RT_Input = false;
                    // que_LB_Input = false;
                    // que_LT_Input = false;

                    input_Que_Is_Active = false;
                    que_Input_Timer = 0;
                }


            }
        }




    }
}

