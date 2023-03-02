
using System;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
   [SerializeField]
   private Behaviour[] componentsToDisable;

   private Camera sceneCamera;
   private void Start()
   {
      // Disable components if it is not our player so we don't have the control of other players.
      // Size of array will be 4 : player controller, player motor, audio listener, camera
      if (!isLocalPlayer)
      {
         for (int i = 0; i < componentsToDisable.Length; i++)
         {
            componentsToDisable[i].enabled = false;
         }
      }
      else
      {
         sceneCamera = Camera.main;

         if (sceneCamera != null)
         {
            sceneCamera.gameObject.SetActive(false);
         }
      }
   }

   private void OnDisable()
   {
      if (sceneCamera != null)
      {
         sceneCamera.gameObject.SetActive(true);
      }
   }
}
