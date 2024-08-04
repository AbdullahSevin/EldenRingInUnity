using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using static UnityEngine.GridBrushBase;
using UnityEngine.InputSystem;


namespace AS
    
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;

        [Header("Movement Settings")]
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 7;
        [SerializeField] float sprintingSpeed = 10;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Jump")]
        [SerializeField] float jumpStaminaCost = 25f;
        [SerializeField] float jumpHeight = 4;
        [SerializeField] float jumpForwardSpeed = 7;
        [SerializeField] float freeFallSpeed = 2;
        private Vector3 jumpDirection;

        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 25f;





        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();
            if (player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;
                
                // IF NOT LOCKED ON PASS MOVE AMOUNT
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

                // OF LOCKED ON PASS HORIZONTAL AND VERTICAL
            }
        }

        public void HandleAllMovement()
        {
            // GROUNDED MOVEMENT
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
           
            // CLAMP THE MOVEMENTS
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove)
            {
                return;
            }

            GetMovementValues();
            // OUR MOVEMENT DIRECTION IS BASED ON OUR CAMERA FACING PERSPECTIVE AND OUR MOVEMENT INPUTS
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.y = 0;
            moveDirection.Normalize();
            


            if (player.playerNetworkManager.isSprinting.Value == true)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);

            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    // MOVE AT RUNNING SPEED
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount >= 0.5f)
                {
                    // MOVE AT WALKING SPEED
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }

            
        }

        private void HandleJumpingMovement()
        {
            if (player.playerNetworkManager.isJumping.Value)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
                // Debug.Log("direction: " + jumpDirection);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDirection;

                //freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                //freeFallDirection = PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                //freeFallDirection.y = 0;

                Vector3 cameraFixedForward = CalculateFixedCameraForward();
                Vector3 cameraRight = PlayerCamera.instance.cameraObject.transform.right;


                freeFallDirection = (cameraFixedForward * PlayerInputManager.instance.verticalInput + cameraRight * PlayerInputManager.instance.horizontalInput).normalized;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }
        
        private void HandleRotation()
        {
            if (!player.canRotate)
            {
                return;
            }

            /*

            Vector3 targetRotationDirection = Vector3.zero;
            Vector3 cameraRotationForward = PlayerCamera.instance.cameraObject.transform.forward;
            cameraRotationForward.y = 0;
            Debug.Log("forward: " + cameraRotationForward);
            targetRotationDirection = cameraRotationForward * verticalMovement;
            Debug.Log("before: " + targetRotationDirection);
            // Debug.Log("vert: " + verticalMovement);
            Vector3 cameraRotationRight = PlayerCamera.instance.cameraObject.transform.right;
            cameraRotationRight.y = 0;
            // Debug.Log("right: " + cameraRotationRight);
            targetRotationDirection += cameraRotationRight * horizontalMovement;
            // Debug.Log("horz: " + horizontalMovement);
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;
            // Debug.Log("after: " + targetRotationDirection);

            */
            /*
            Vector3 targetRotationDirection = Vector3.zero;
            Vector3 cameraRotationForward = new Vector3 (1, 0, 1);
            targetRotationDirection = cameraRotationForward * verticalMovement;
            Vector3 cameraRotationRight = new Vector3(1, 0, 1);
            targetRotationDirection += cameraRotationRight * horizontalMovement;         
            targetRotationDirection.Normalize();
            // targetRotationDirection.y = 0;
            Debug.Log("after: " + targetRotationDirection);
            */

            Vector3 targetRotationDirection = Vector3.zero;

            //List<string> rotationDirection;
            //rotationDirection = new List<string>();
            //if (verticalMovement == 1)
            //{
            //    rotationDirection.Clear();
            //    rotationDirection.Add("Forward");
            //}
            //else if (verticalMovement ==-1)
            //{
            //    rotationDirection.Clear();
            //    rotationDirection.Add("Backward");
            //}
            //else if (verticalMovement < 0.8 && verticalMovement > 0.6)
            //{
            //    if (horizontalMovement > 0.6)
            //    {
            //        rotationDirection.Clear();
            //        rotationDirection.Add("Forward");
            //        rotationDirection.Add("Right");
            //    }
            //    else if (horizontalMovement < -0.6)
            //    {
            //        rotationDirection.Clear();
            //        rotationDirection.Add("Forward");
            //        rotationDirection.Add("Left");
            //    }

            //}
            //else if (verticalMovement == 0)
            //{
            //    if (horizontalMovement == 1)
            //    {
            //        rotationDirection.Clear();
            //        rotationDirection.Add("Right");
            //    }
            //    else if (horizontalMovement == -1)
            //    {
            //        rotationDirection.Clear();
            //        rotationDirection.Add("Left");
            //    }
            //}
            //else if (verticalMovement > -0.8 && verticalMovement < -0.6)
            //{
            //    if (horizontalMovement > 0.5)
            //    {
            //        rotationDirection.Clear();
            //        rotationDirection.Add("Backward");
            //        rotationDirection.Add("Right");
            //    }
            //    else if (horizontalMovement < -0.5)
            //    {
            //        rotationDirection.Clear();
            //        rotationDirection.Add("Backward");
            //        rotationDirection.Add("Left");
            //    }
            //}
            //Vector3 cameraForward = PlayerCamera.instance.cameraObject.transform.forward;
            //cameraForward.y = 0;
            //cameraForward.x = 0;
            //foreach (string direction in rotationDirection)
            //{
            //    switch (direction)
            //    {
            //        case "Forward":
            //            targetRotationDirection += cameraForward;
            //            break;
            //        case "Backward":
            //            targetRotationDirection -= cameraForward;
            //            break;
            //        case "Right":
            //            targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right;
            //            break;
            //        case "Left":
            //            targetRotationDirection -= PlayerCamera.instance.cameraObject.transform.right;
            //            break;
            //    }
            //}

            //targetRotationDirection.Normalize();
            //targetRotationDirection.y = 0;

            Vector3 cameraForward = PlayerCamera.instance.transform.forward;
            Vector3 cameraRight = PlayerCamera.instance.transform.right;

            cameraForward.y = 0; // Ignore y component
            cameraRight.y = 0; // Ignore y component

            cameraForward.Normalize();
            cameraRight.Normalize();

            targetRotationDirection = (cameraForward * verticalMovement + cameraRight * horizontalMovement).normalized;


            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }


            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;

            /*
            Debug.Log($"horMov: {horizontalMovement} / verMov: {verticalMovement}" +
                $" / transRight: {PlayerCamera.instance.cameraObject.transform.right} / transForward: {PlayerCamera.instance.cameraObject.transform.forward}" +
                $"targetRotDir: {targetRotationDirection} / targetRot: {targetRotation}");
            */

        }

        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            // IF WE ARE OUT OF STAMINA, SET SPRINTING TO FALSE

            // IF WE ARE MOVING, SET SPRINTING TO TRUE
            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            // IF WE ARE NOT MOVING OR MOVING SLOWLY, SET SPRINTING TO FALSE
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.isSprinting.Value == true)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
            // IF WE ARE MOVING SET SPRINTING TO TRUE
            // IF WE ARE STATIONARY SET SPRINTING TO FALSE
        }
        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction == true)
            {
                return;
            }


            // IT APPEARS YOU CAN GO TO NEGATIVE STAMINA AMOUNT, SO IF YOU HAVE 1 STAMINA, 1 - 25(DODGE STAMINA COST), YOU WILL BE HAVING -24 STAMINA
            if (player.playerNetworkManager.currentStamina.Value < 0)
            {
                return;
            }
            

            // IF WE ARE MOVING WHEN WE ATTEMPT TO DODGE: WE PERFORM A ROLL
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);

                //PERFORM A ROLL ANIMATION
            }

            // IF WE ARE STANDING STILL WHEN WE ATTEMPT TO DODGE: WE PERFORM A BACKSTEP
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
                // PERFORM A BACKSTEP ANIMATION
            }

            //if (dodgeStaminaCost == 0)
            //{
            //    dodgeStaminaCost = dodgeStaminaCost + 25;
            //    Debug.Log("dodge cost has been set to dodge cost + 25, because SOMEHOW??? it was set to 0"); // HAHA FOUND OUT IT WAS EDITED ON PLAYER PREFAB 
            //}
            
            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;

        }

        public void AttemptToPerformJump()
        {

            // IF WE ARE PERFORMING A GENERAL ACTION WE DONT WANT TO ALLOW A JUMP (WILL CHANGE WHEN COMBAT IS ADDED)
            if (player.isPerformingAction == true)
            {
                return;
            }


            // NO STAMINA NO JUMP
            if (player.playerNetworkManager.currentStamina.Value < 0)
            {
                return;
            }


            // IF WE ARE ALREADY JUMPING WE CANT JUMP ON AIR AGAIN (OR CAN WE? WE WILL SEE IN THE FUTURE)
            if (player.playerNetworkManager.isJumping.Value)
            {
                return;
            }

            // IF WE ARE NOT ON THE GROUND WE CAN'T JUMP
            if (!player.isGrounded)
            {
                return;
            }

            // IF WE ARE TWO HANDING OUR WEAPON PLAY THE TWO HANDED JUMP ANIMATION, OTHERWISE PLAY THE ONE HANDED ANIMATION (TO DO)

            player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

            player.playerNetworkManager.isJumping.Value = true;


            

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            Vector3 cameraFixedForward =  CalculateFixedCameraForward();
            Vector3 cameraRight = PlayerCamera.instance.cameraObject.transform.right;


            jumpDirection = (cameraFixedForward * PlayerInputManager.instance.verticalInput + cameraRight * PlayerInputManager.instance.horizontalInput).normalized;

            //jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            //jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            //jumpDirection.y = 0;


            if (jumpDirection != Vector3.zero)  // IF WE ARE NOT STATIONARY
            {

                //jumpDirection *= verticalMovement;
                //Debug.Log($"{verticalMovement}");

                //jumpDirection *= player.playerAnimatorManager.verticalAmount;
                //Debug.Log($"{player.playerAnimatorManager.verticalAmount}");



                // IF WE ARE SPRINTING JUMP DISTANCE IS AT FULL DISTANCE
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                // IF WE ARE RUNNING JUMP DISTANCE IS AT HAFT DISTANCE
                else if (PlayerInputManager.instance.moveAmount > 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                // IF WE ARE WALKING JUMP DISTANCE IS AT QUARTER DISTANCE
                else if (PlayerInputManager.instance.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }

                // BECAUSE WE CLAMPED THE MOVEMENT, JUMP IS NOT FLEXIBLE MUCH, UNFORTUNATELLY.
            }

            

        }

        public void ApplyJumpingVelocity()
        {
            // APPLY AN UPWARD VELOCITY DEPENDING ON FORCES IN OUR GAME
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }

        private Vector3 CalculateFixedCameraForward()
        {
            Quaternion cameraRotation = PlayerCamera.instance.cameraObject.transform.rotation;

            // Adjust the rotation to create a fixed rotation where y-component is set to zero
            Quaternion cameraFixedRotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, cameraRotation.eulerAngles.z);

            // Get the forward direction of this fixed rotation
            Vector3 cameraFixedForward = cameraFixedRotation * Vector3.forward;

            return cameraFixedForward;
        }


    }
}

