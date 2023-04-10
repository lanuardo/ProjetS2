
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PalyerController : MonoBehaviour
{
   [SerializeField]
   private float speed = 3f;
   
   [SerializeField]
   private float mouseSensitivityX = 10f;
   
   [SerializeField]
   private float mouseSensitivityY = 8f;
   
   [SerializeField]
   private float jumpForce = 1f;

   private bool isGrounded;


   private PlayerMotor motor;

   private void Start()
   {
      motor = GetComponent<PlayerMotor>();
   }

   private void Update()
   {
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
      
      // Calculate the speed of the player
      float xMov = Input.GetAxis("Horizontal");
      float zMov = Input.GetAxis("Vertical");

      Vector3 moveHorizontal = transform.right * xMov;
      Vector3 moveVertical = transform.forward * zMov;

      Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

      motor.Move(velocity);
      
      //Calculate the rotation of the player with vector3

      float yRot = Input.GetAxisRaw("Mouse X");

      Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

      motor.Rotate(rotation);
      
      //Calculate the rotation of the camera with vector3

      float xRot = Input.GetAxisRaw("Mouse Y");

      Vector3 cameraRotation = new Vector3(xRot, 0, 0) * mouseSensitivityY;

      motor.RotateCamera(cameraRotation);

      //Jump button 

      Vector3 jumpVelocity = Vector3.zero;
      
      if (Input.GetButton("Jump") && isGrounded)
      {
         jumpVelocity = Vector3.up * jumpForce;
      }
      motor.ApplyJump(jumpVelocity);
      CheckGround();
   }

   void CheckGround()
   {
      Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f),
         transform.position.z);
      Vector3 direction = transform.TransformDirection(Vector3.down);
      float distance = .75f;

      if (Physics.Raycast(origin, direction, distance))
      {
         isGrounded = true;
      }
      else
      {
         isGrounded = false;
      }
   }
}
