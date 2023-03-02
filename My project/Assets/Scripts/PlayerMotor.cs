
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
   [SerializeField]
   private Camera cam;
   
   private Vector3 veloctiy;
   private Vector3 rotation;
   private Vector3 cameraRotation;

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
   }

   private void PerformMovement()
   {
      if (veloctiy != Vector3.zero)
      {
         rb.MovePosition(rb.position + veloctiy * Time.fixedDeltaTime);
      }
   }
   
   private void PerformRotation()
   {
      //Euler converts vector3 to quaternion
      rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
      
      cam.transform.Rotate(-cameraRotation);
   }
}
