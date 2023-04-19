using Mirror;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private WeaponData currentweapon;
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
        if (_weaponManager is not null)
        {
            

            if (Input.GetKeyDown(KeyCode.R) && _weaponManager.currentMagazineSize < currentweapon.magazineSize) //on recharge l'arme lorsque la touche R est appuyee et lorsque le chargeur n'est pas plein
            {
                StartCoroutine(_weaponManager.Reload());
                return; //on ne peut pas tirer lorsqu'on recharge
            }


            currentweapon = _weaponManager.getcurrentWeapon();
            if (currentweapon is not null)
            {
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
        if (_weaponManager is not null)
        {
            GameObject hitEffect = Instantiate(_weaponManager.getcurrentWeaponGraphics().hitPrefab, pos,
                Quaternion.LookRotation(normal));
            Destroy(hitEffect, 2f);
        }
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
        if (_weaponManager is not null)
        {
            if (_weaponManager.getcurrentWeaponGraphics() is not null)
            {
                if (_weaponManager.getcurrentWeaponGraphics().muzzleFlash is not null)
                {
                    _weaponManager.getcurrentWeaponGraphics().muzzleFlash.Play();
                }
        

                AudioSource audioSource = GetComponent<AudioSource>();
                if (currentweapon is not null)
                {
                    audioSource.PlayOneShot(currentweapon.shootSound); // cette méthode nous permet de préciser en paramètre la source audio

                }
            }
            
        }
        
    }
    
    [Client] 
    private void Shoot()
    {
        if (_weaponManager is not null)
        {
            if (!isLocalPlayer || _weaponManager.isReloading)
            {
                return;
            }

            if (_weaponManager.currentMagazineSize <= 0) //Si on n'a plus de balles
            {
                _weaponManager.Reload();
                StartCoroutine(_weaponManager.Reload());
                return; //parce qu'on ne veut pas le code dessous (particules,tir, ...) vu qu'on n'a plus de balles
            }

            _weaponManager.currentMagazineSize--; //decremente une balle dans le chargeur

            Debug.Log("Il nous reste " + _weaponManager.currentMagazineSize + " balles dans le chargeur.");

            CmdOnShoot();

            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentweapon.range, mask))
            {
                //send information if a player is shot
                if (hit.collider.CompareTag("Player"))
                {
                    CmdPlayerShot(hit.collider.name, currentweapon.damage, transform.name);
                }

                if (hit.collider.CompareTag("Intelligence"))
                {
                    IA ai = hit.collider.gameObject.GetComponent<IA>();
                    ai.TakeDamage(currentweapon.damage);
                }

                CmdOnHit(hit.point, hit.normal);

            }
        }
    }

    [Command] //client to server
    private void CmdPlayerShot(string playerId, float damage, string sourceID)
    {
        //print info if a player is shot whether its my player or not
        Debug.Log(playerId + "has been shot by" +sourceID);

        //Give damage to the player who got shot
        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage, sourceID);
        
        
    }
}
