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
        [SerializeField] float cameraSmoothSpeed = 1; // BIGGER VALUES MAKES THE CAMERA TAKE LONGER TO REACH TO ITS POSITION, SLOWER AT CATCHING UP THE PLAYER
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

        [Header("Lock On")]
        [SerializeField] float lockOnRadius = 20;
        [SerializeField] float minimumViewableAngle = -50;
        [SerializeField] float maximumViewableAngle = 50;
        [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
        [SerializeField] float setCameraHeightSpeed = 0.05f;
        [SerializeField] float unlockedCameraHeight= 1.65f;
        [SerializeField] float lockedCameraHeight = 2.0f;
        [SerializeField] List<CharacterManager> availableTargets = new List<CharacterManager>();
        private Coroutine cameraLockOnHeightCoroutine;
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockOnTarget;
        public CharacterManager rightLockOnTarget;
        
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
            if (player.playerNetworkManager.isLockedOn.Value)
            {

                //  THIS ROTATES THIS GAMEOBJECT
                Vector3 rotationDirection =
                    player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetLockOnRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetLockOnRotation, lockOnTargetFollowSpeed);

                //  THIS ROTATES THE PIVOT OBJECT
                rotationDirection =
                    player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetLockOnRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetLockOnRotation, lockOnTargetFollowSpeed);


                //  SAVE OUR ROTATIONS TO OUR LOOK ANGLES, SO WHEN WE UNLOCK IT DOESNT SNAP TOO FAR AWAY
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            // ELSE ROTATE REGULARLY
            else
            {
                
                if (PlayerInputManager.instance.isUsingMouseKeyboard == false)
                {

                    // NORMAL ROTATIONS
                    // ROTATE LEFT AND RIGHT BASED ON HORIZONTAL MOVEMENT ON THE RIGHT JOYSTICK
                    leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
                    leftAndRightLookAngle = Mathf.Repeat(leftAndRightLookAngle, 360);
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
                        leftAndRightLookAngle = Mathf.Repeat(leftAndRightLookAngle, 360);
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

        public void HandleLocatingLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity;                   //  WILL BE USED TO DETERMINE THE TARGET CLOSEST TO US
            float shortestDistanceOfRightTarget = Mathf.Infinity;      //  WILL BE USED TO DETERMINE SHORTEST DISTANCE ON ONE AXIS TO THE RIGHT OF CURRENT TARGET (+) (Closest target to the right of current target)
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;      //  WILL BE USED TO DETERMINE SHORTEST DISTANCE ON ONE AXIS  TO THE LEFT OF CURRENT TARGET (-) (Closest target to the left of current target)

            //  TO DO: USE A LAYERMASK
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if (lockOnTarget != null)
                {
                    //  CHECK IF THEY ARE WITHIN OUR FIELD OF VIEW
                    Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);


                    //  IF TARGET IS DEAD CHECK THE NEXT POTENTIAL TARGET
                    if (lockOnTarget.isDead.Value)
                    {
                       // Debug.Log("PLAYER CAMERA       >>> DEAD -  CONTINUE");
                        continue;
                    }
                    //  IF TARGET IS US CHECK FOR NEXT POTENTIAL TARGET
                    if (lockOnTarget.transform.root == player.transform.root)
                    {
                       // Debug.Log("PLAYER CAMERA       >>> PLAYER ROOT -  CONTINUE");
                        continue;
                    }

                    //  LASTLY IF THE TARGET IS OUTSIDE FIELD OF VIEW OR IS BLOCKED BY ENVIRO, SKIP AND CONTINUE SEARCHING FOR NEXT POTENTIAL TARGET
                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                       // Debug.Log("PLAYER CAMERA       >>> IF VIEWABLE ANGLE ?");
                        RaycastHit hit;

                        //  TODO ADD LAYER MASK, MAKE THIS ONLY CHECK THE ENVIRONMENT LAYER
                        if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position,
                            lockOnTarget.characterCombatManager.lockOnTransform.position,
                            out hit, WorldUtilityManager.instance.GetEnviroLayers()))
                        {
                            // Debug.Log("PLAYER CAMERA       >>> LINECAST OBSTACLE -  CONTINUE");
                            // WE HIT SOMETHING, THIS CANNOT BE OUR LOCK ON TARGET BECAUSE THERE IS/ARE OBSTACLE AMONG US, SKIP
                            continue;

                        }
                        else
                        {
                            //  OTHERWISE ADD TO THE TARGET LIST
                            availableTargets.Add(lockOnTarget);
                            // Debug.Log("PLAYER CAMERA     >>>   Found a valid Lock On Target / TARGET LIST:  " + availableTargets.Count + " + " + availableTargets[0]);
                        }

                    }

                }

            }
            // WE NOW SORT THROUGH OUR POTENTIAL TARGETS TO SEE WHICH ONE WE LOCK ONTO FIRST
            for (int k = 0; k < availableTargets.Count; k++)
            {
                if (availableTargets[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k];
                        // Debug.Log("PLAYER CAMERA     >>>    NEAREST LOCK ON TARGET: + " + nearestLockOnTarget);
                    }

                    //  IF WE ARE ALREADY LOCKED ON WHEN SEARCHING FOR OUR NEAREST LEFT/RIGHT TARGETS
                    if (player.playerNetworkManager.isLockedOn.Value)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if (availableTargets[k] != player.playerCombatManager.currentTarget)
                        {
                            continue;
                        }

                        //  CHECK FOR THE LEFT SIDE FOR TARGETS
                        if (relativeEnemyPosition.x < 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = availableTargets[k];
                        }
                        //  
                        else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = availableTargets[k];
                        }

                    }


                }
                else
                {
                    ClearLockOnTargets();
                    player.playerNetworkManager.isLockedOn.Value = false;
                }


            }
        }

        public void SetLockCameraHeight()
        {
            if (cameraLockOnHeightCoroutine != null)
            {
                StopCoroutine(cameraLockOnHeightCoroutine);
            }

            cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());

        }

        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            availableTargets.Clear();
        }

        public IEnumerator WaitThenFindNewTarget()
        {
            while (player.isPerformingAction)
            {
                yield return null;
            }

            ClearLockOnTargets();
            HandleLocatingLockOnTargets();

            if (nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }

            yield return null;  
        }

        private IEnumerator SetCameraHeight()
        {
            float duration = 1;
            float timer = 0;

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
            Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                if (player != null)
                {
                    if (player.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.transform.localPosition = 
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                        cameraPivotTransform.transform.localRotation =
                            Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                    }
                    else
                    {
                        cameraPivotTransform.transform.localPosition = 
                            Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    }
                }
                yield return null;
            }

            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                    cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
                }

                yield return null;
            }

        }


    }
}





