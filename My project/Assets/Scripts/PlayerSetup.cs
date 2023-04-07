using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
   [SerializeField]
   private Behaviour[] componentsToDisable;

   [SerializeField] private string remoteLayerName = "RemotePlayer";

   [SerializeField] private string dontDrawLayerName= "DontDraw";
   [SerializeField] private GameObject playerGraphics;

   [SerializeField] private GameObject playerUIPrefab;
   
   [HideInInspector]
   public GameObject playerUIInstance;
   
   
   
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
         
         
         // désactiver la partie graphique du joueur local "DontDraw"
         Utilitaire.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
         
         // definition d'un player ui propre à celui-ci (qui n'est pas accessible par d'autre joueur
         playerUIInstance = Instantiate(playerUIPrefab);

         GetComponent<Player>().Setup();

      }
      
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
      Destroy(playerUIInstance);
      if (isLocalPlayer)
      {
         GameManager.Instance.SetCameraActive(true);
      }

      GameManager.UnregisterPlayer(transform.name);
   }
}
