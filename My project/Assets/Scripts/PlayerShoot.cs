using Mirror;
using UnityEngine;


public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("None camera was selected");
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client] 
    private void Shoot()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            //send information if a player is shot
            if (hit.collider.CompareTag("Player"))
            {
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
        }
    }

    [Command] //client to server
    private void CmdPlayerShot(string playerId, float damage)
    {
        //print info if a player is shot whether its my player or not
        Debug.Log(playerId + "has been shot");

        //Give damage to the player who got shot
        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);
    }
}
