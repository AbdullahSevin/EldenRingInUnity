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

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("PLAYER ACTIONS")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;





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
        }

        private void OnSceneChange (Scene oldScene, Scene newScene)
        {
            // IF WE ARE LOADING TO OUR WORLD SCENE, ENABLE OUR PLAYER CONTROLS
            // use a list and "in", later if you end up having multiple worlds ? 
                // or make it false in case of index == main menu, and use "else" to make it true on all other scenes 
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE OUR PLAYER CONTROLS
            // THIS IS TO PREVENT PLAYER MOVEMENT WHILE NOT BEING IN WORLD SCENE (MAIN MENU SETTINGS MENU ETC)
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls ==null)
            {
                playerControls = new PlayerControls();


                // COMMON INPUTS: THESE CODES DOES THE SAME THINGS FOR BOTH JOYSTICK AND KEYBOARD
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;



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
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandeSprintInput();
            HandleJumpInput();
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
            // IF WE ARE NOT LOCKED ON, ONLY USE THE MOVEAMOUNT
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);


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
        
    }
}

