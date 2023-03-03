
using UnityEngine;

namespace Player2
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        [SerializeField]
        public float jumpPower = 3f;

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

    
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            #region  Handles Movement
            
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
        
            //Left Shit to walk
            bool isWalking = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isWalking ? walkSpeed : runSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isWalking ? walkSpeed : runSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = _moveDirection.y;
            _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            #endregion

            #region Handles Jumping

            if (Input.GetButton("Jump") && canMove && _characterController.isGrounded)
            {
                _moveDirection.y = jumpPower;
            }
            else
            {
                _moveDirection.y = movementDirectionY;
            }

            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= gravity * Time.deltaTime;
            }

            #endregion

            #region Handles Rotation

            _characterController.Move(_moveDirection * Time.deltaTime);

            if (canMove)
            {
                _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                //Limiting Rotate Axe X
                _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0,0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }

            #endregion
        }
    }
}
