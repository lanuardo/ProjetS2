
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


   private PlayerMotor motor;

   private void Start()
   {
      motor = GetComponent<PlayerMotor>();
   }

   private void Update()
   {
      // Calculate the speed of the player
      float xMov = Input.GetAxisRaw("Horizontal");
      float zMov = Input.GetAxisRaw("Vertical");

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
   }
}
