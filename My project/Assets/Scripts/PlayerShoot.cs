using Mirror;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private PlayerWeapon currentweapon;
    private WeaponManager _weaponManager;
    
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    
    
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("None camera was selected");
            this.enabled = false;
        }
        
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (PauseMenu.isOn)
        {
            return;
        }
        
        currentweapon = _weaponManager.getcurrentWeapon();
        if (currentweapon.fireRate<=0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot",0f,1f/currentweapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal )
    {
        RpcDoHitEffects(pos , normal);
    }
    
    [ClientRpc]
    void RpcDoHitEffects(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(_weaponManager.getcurrentWeaponGraphics().hitPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2f);
    }

    // fonction apppelé lorsque le joueur tire afin de déclencher les particules (on prévient le serveur)
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffects();
    }

    [ClientRpc]
    void RpcDoShootEffects()
    {
        _weaponManager.getcurrentWeaponGraphics().muzzleFlash.Play();
    }
    
    [Client] 
    private void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdOnShoot();
        
        RaycastHit hit;
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentweapon.range, mask))
        {
            //send information if a player is shot
            if (hit.collider.CompareTag("Player"))
            {
                CmdPlayerShot(hit.collider.name, currentweapon.damage);
            }
            
            CmdOnHit(hit.point,hit.normal);
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
