using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
   [SerializeField]
   private Behaviour[] componentsToDisable;

   [SerializeField] private string remoteLayerName = "RemotePlayer";

   private Camera _sceneCamera;
   private void Start()
   {
      // Disable components if it is not my player so we don't have the control of other players.
      if (!isLocalPlayer)
      {
         //if is not my player, disable all components of other player and assign him the layer "RemotePlayer"
         //so it can send information when only another player is shot
         DisableComponents();
         AssignRemotePlayer();
      }
      else
      {
         _sceneCamera = Camera.main;

         if (_sceneCamera != null)
         {
            _sceneCamera.gameObject.SetActive(false);
         }
      }
      
      GetComponent<Player>().Setup();
   }
   
   public override void OnStartClient()
   {
      //starts when a player join
      base.OnStartClient();
      string netID = GetComponent<NetworkIdentity>().netId.ToString();
      Player player = GetComponent<Player>();
      GameManager.RegisterPlayer(netID, player);
   }

   private void AssignRemotePlayer()
   {
      //Assign layer "RemotePlayer"
      gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
   }

   private void DisableComponents()
   {
      foreach (var t in componentsToDisable)
      {
         t.enabled = false;
      }
   }
   private void OnDisable()
   {
      //starts when a player leave
      if (_sceneCamera != null)
      {
         _sceneCamera.gameObject.SetActive(true);
      }
      
      GameManager.UnregisterPlayer(transform.name);
   }
}
