
using System;
using Mirror.Examples.NetworkRoom;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
   [SerializeField]
   private Camera cam;
   
   private Vector3 veloctiy;
   private Vector3 rotation;
   private Vector3 cameraRotation;
   private Vector3 jumpVelocity;

   private Rigidbody rb;

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
   }

   public void Move(Vector3 _velocity)
   {
      veloctiy = _velocity;
   }
   
   public void Rotate(Vector3 _rotation)
   {
      rotation = _rotation;
   }
   public void RotateCamera(Vector3 _cameraRotation)
   {
      cameraRotation = _cameraRotation;
   }

   private void FixedUpdate()
   {
      PerformMovement();
      PerformRotation();

      // Vector3 gravity = new Vector3(0, -9.81f, 0);
      // float gravityMultiplayer = 5f;
      // gravity *= gravityMultiplayer;
      //
      // rb.AddForce(gravity, ForceMode.Force);
      // if (rb.velocity.y < 0)
      // {
      //    rb.AddForce(gravity / 10f, ForceMode.Force);
      // }
   }

   private void PerformMovement()
   {
      if (veloctiy != Vector3.zero)
      {
         rb.MovePosition(rb.position + veloctiy * Time.fixedDeltaTime);
      }

      if (jumpVelocity != Vector3.zero)
      {
         rb.AddForce(jumpVelocity * Time.fixedDeltaTime, ForceMode.Impulse);
      }
   }
   
   private void PerformRotation()
   {
      //Euler converts vector3 to quaternion
      rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
      
      cam.transform.Rotate(-cameraRotation);
   }

   public void ApplyJump(Vector3 _jumpVelocity)
   {
      jumpVelocity = _jumpVelocity;
   }

   
}
