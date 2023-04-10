
using UnityEngine;

namespace Player2
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        [SerializeField]
        public float jumpPower = 3f;
<<<<<<< HEAD
        
        private PlayerMotor motor;

=======
>>>>>>> parent of 29af99ed (void)

        [SerializeField]
        public float gravity = 10f;
    
        public Camera playerCamera;

        public float walkSpeed = 3f;

        public float runSpeed = 5f;

        public float lookXLimit = 90f;
    
        public float lookSpeed = 2f;
    
        private Vector3 _moveDirection = Vector3.zero;

        private float _rotationX;
    
        public bool canMove = true;

        private CharacterController _characterController;

<<<<<<< HEAD
        
=======
>>>>>>> parent of 29af99ed (void)
    
        void Start()
        {
            //Get our component
            _characterController = GetComponent<CharacterController>();
<<<<<<< HEAD

=======
            
>>>>>>> parent of 29af99ed (void)
            //Locking the cursor and make it invisible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
<<<<<<< HEAD
            if (PauseMenu.isOn)
            {
                //active la souris dans le menu
                if (Cursor.lockState!=CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
         
                motor.Move(Vector3.zero);
                motor.Rotate(Vector3.zero);
                motor.RotateCamera(Vector3.zero);

                return;
            }
            //desactive la souris en jeu
            if (Cursor.lockState!=CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
=======
>>>>>>> parent of 29af99ed (void)
            #region  Handles Movement

            //Get values of the transform's actual position in each axis X (right) and Z (forward)
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            
            //Left Shit to walk
            bool isWalking = Input.GetKey(KeyCode.LeftShift);
            
            //Calculate movement speed in each axis
            float curSpeedZ = canMove ? (isWalking ? walkSpeed : runSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedX = canMove ? (isWalking ? walkSpeed : runSpeed) * Input.GetAxis("Horizontal") : 0;
            
            //Get the position in Y axis of the calculated movement vector (initially = 0)
            float movementDirectionY = _moveDirection.y;
            
            //update the movement vector in axis X
            _moveDirection = (forward * curSpeedZ) + (right * curSpeedX);

            #endregion

            #region Handles Jumping

            //Checks if the button for jumping is pressed, if the player can move, and if he is on the ground
            if (Input.GetButton("Jump") && canMove && _characterController.isGrounded)
            {
                // player can jump
                _moveDirection.y = jumpPower;
            }
            else
            {
                //the player isn't jumping or already not on the ground
                _moveDirection.y = movementDirectionY;
            }

            // if player is not on the ground, make him go down with gravity
            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= gravity * Time.deltaTime;
            }

            #endregion
            
            //apply calculated movement vector above
            _characterController.Move(_moveDirection * Time.deltaTime);

            #region Handles Rotation
            
            if (canMove)
            {
                //Calculate rotation around the X axis of the camera and limiting Vertical Rotation
                _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);

                //Apply the calculated rotation around the X axis to the camera. We only want the camera to move vertically. Not the entire player
                playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0,0);
                
                //Apply directly the calculated rotation around the Y axis
                //Player is only moving horizontally
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }

            #endregion

        }
    }
}
