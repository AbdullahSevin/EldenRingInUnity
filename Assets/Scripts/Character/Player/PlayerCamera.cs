using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics.CodeAnalysis;


namespace AS
    
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        public Vector2 turn;
        // FOR MOUSE
        public bool isOnFreeViewMode = false;

        [SerializeField] Transform cameraPivotTransform;

        // CHANGE THESE TO TWEAK CAMERA PERFORMANCE
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // BIGGER VALUES MAKES THE CAMERA TAKE LONGER TO REACH TO ITS POSITION, SLOWER AT CATCHING UP THE PLAYER
        [SerializeField] float leftAndRightRotationSpeed = 700;
        [SerializeField] float upAndDownRotationSpeed = 700;
        [SerializeField] float minimumPivot = -80; // THE LOWEST POINT A PLAYER IS ABLE TO LOOK DOWN
        [SerializeField] float maximumPivot = 80; // THE HIGHEST POINT A PLAYER IS ABLE TO LOOK DOWN
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        // display camera values
        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // USED FOR CAMERA COLLISIONS (MOVES THE CAMERA OBJECT TO THIS POSITION UPON COLLIDING)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition; // VALUE USED FOR THE CAMERA COLLISIONS
        private float targetCameraZPosition; // VALUE USED FOR THE CAMERA COLLISIONS


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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        private void Update()
        {
            
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
            }

        }
            
        private void HandleFollowTarget()
        {
            Vector3 targetCameraZPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
                transform.position = targetCameraZPosition;
        }


        private void HandleRotations()
        {
            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // IF LOCKED ON, FORCE ROTATION TOWARDS TARGET
            // ELSE ROTATE REGULARLY
            if (PlayerInputManager.instance.isUsingMouseKeyboard == false)
            {
                
                // NORMAL ROTATIONS
                // ROTATE LEFT AND RIGHT BASED ON HORIZONTAL MOVEMENT ON THE RIGHT JOYSTICK
                leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
                // ROTATE UP AND DOWN BASED ON THE VERTIKCLA MOVEMENT ON THE RIGHT JOYSTICK
                upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
                // CLAMP THE UP AND DOWN LOKING ANGLE BETWEEN A MIN AND A MAX VALUE
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                

                // ROTATE THIS OBJECT LEFT AND RIGHT
                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation = targetRotation;

                // ROTATE THE PIVOT GAMEOBJECT UP AND DOWN
                cameraRotation = Vector3.zero;
                cameraRotation.x = upAndDownLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                if (isOnFreeViewMode)
                {
                    turn.x = Input.GetAxis("Mouse X");
                    turn.y = Input.GetAxis("Mouse Y");

                    leftAndRightLookAngle += (turn.x * leftAndRightRotationSpeed * Time.deltaTime);
                    upAndDownLookAngle -= (turn.y * upAndDownRotationSpeed * Time.deltaTime);
                    // CLAMP
                    upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                    // CAMERA LEFT AND RIGHT ROTATION
                    cameraRotation.y = leftAndRightLookAngle;
                    targetRotation = Quaternion.Euler(cameraRotation);
                    transform.rotation = targetRotation;

                    // CAMERA UP AND DOWN ROTATION
                    cameraRotation = Vector3.zero;
                    cameraRotation.x = upAndDownLookAngle;
                    targetRotation = Quaternion.Euler(cameraRotation);
                    cameraPivotTransform.localRotation = targetRotation;
                    //this.transform.localRotation = Quaternion.Euler(-turn.y * mouseSensitivityY, turn.x * mouseSensitivityX, 0);
                }
                else
                {
                    return;
                }
                
            }

        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            // DIRECTION FOR COLLISION CHECK
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // WE CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRED DIRECTION * (SEE ABOVE FOR DIRECTION)
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // IF THERE IS, WE GET OUR DISTANCE FROM IT
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // WE THEN EQUATE OUR TARGET Z POSITION TO THE FOLLOWING 
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }
            
            // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (MAKING IT SNAP BACK)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            // WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;

        }
    }
}





