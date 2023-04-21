using UnityEngine;
using Mirror;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private WeaponData _currentWeapon;
    private WeaponManager _weaponManager;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Pas de caméra renseignée sur le système de tir.");
            this.enabled = false;
        }

        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        _currentWeapon = _weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn || WeaponChoice.isOn)
        {
            return;
        }

        if (_currentWeapon is not null)
        {


            if (Input.GetKeyDown(KeyCode.R) && _weaponManager.currentMagazineSize < _currentWeapon.magazineSize)
            {
                StartCoroutine(_weaponManager.Reload());
                return;
            }

            if (_currentWeapon.fireRate <= 0f)
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
                    InvokeRepeating("Shoot", 0f, 1f / _currentWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }
        }
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        if (_weaponManager is not null)
        {
            GameObject hitEffect = Instantiate(_weaponManager.GetCurrentGraphics().hitPrefab, pos,
                Quaternion.LookRotation(normal));
            Destroy(hitEffect, 2f);
        }
    }

    // Fonction appelée sur le serveur lorsque notre joueur tir (On prévient le serveur de notre tir)
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    // Fait apparaitre les effets de tir chez tous les clients / joueurs
    [ClientRpc]
    void RpcDoShootEffect()
         {
             if (_weaponManager is not null)
             {
                 _weaponManager.GetCurrentGraphics().muzzleFlash.Play();

                 AudioSource audioSource = GetComponent<AudioSource>();
                 if (_currentWeapon is not null)
                 {
                     audioSource.PlayOneShot(_currentWeapon.shootSound);
                 }
                 
             }
         }

    [Client]
    private void Shoot()
    {
        if (_weaponManager is not null)
        {
            if(!isLocalPlayer || _weaponManager.isReloading)
            {
                return;
            }
            
            if (_weaponManager.currentMagazineSize <= 0)
            {
                StartCoroutine(_weaponManager.Reload());
                return;
            }

            _weaponManager.currentMagazineSize--;

            Debug.Log("Il nous reste " + _weaponManager.currentMagazineSize + " balles dans le chargeur.");

            CmdOnShoot();

            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, _currentWeapon.range, mask))
            {
                
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (transform.GetComponent<Player>().team != hit.collider.GetComponent<Player>().team)
                        {
                            CmdPlayerShot(hit.collider.name, _currentWeapon.damage, transform.name);
                        }
                    }
                
                

                if (hit.collider.CompareTag("Intelligence"))
                {
                    IA ai = hit.collider.gameObject.GetComponent<IA>();
                    ai.TakeDamage(_currentWeapon.damage);
                }

                CmdOnHit(hit.point, hit.normal);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string playerId, float damage, string sourceID)
    {
        Debug.Log(playerId + " a été touché.");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage, sourceID);
    }

}
