using UnityEngine;
using Mirror;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private WeaponData currentWeapon;
    private WeaponManager weaponManager;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Pas de caméra renseignée sur le système de tir.");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponManager.getcurrentWeapon();

        if (PauseMenu.isOn)
        {
            return;
        }

        if (currentWeapon is not null)
        {


            if (Input.GetKeyDown(KeyCode.R) && weaponManager.currentMagazineSize < currentWeapon.magazineSize)
            {
                StartCoroutine(weaponManager.Reload());
                return;
            }

            if (currentWeapon.fireRate <= 0f)
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
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
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
        if (weaponManager is not null)
        {
            GameObject hitEffect = Instantiate(weaponManager.getcurrentWeaponGraphics().hitPrefab, pos,
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
             if (weaponManager is not null && currentWeapon is not null)
             {
                 weaponManager.getcurrentWeaponGraphics().muzzleFlash.Play();

                 AudioSource audioSource = GetComponent<AudioSource>();
                 audioSource.PlayOneShot(currentWeapon.shootSound);
             }
         }

    [Client]
    private void Shoot()
    {
        if (weaponManager is not null)
        {
            if(!isLocalPlayer || weaponManager.isReloading)
            {
                return;
            }

        


            if (weaponManager.currentMagazineSize <= 0)
            {
                StartCoroutine(weaponManager.Reload());
                return;
            }

            weaponManager.currentMagazineSize--;

            Debug.Log("Il nous reste " + weaponManager.currentMagazineSize + " balles dans le chargeur.");

            CmdOnShoot();

            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    CmdPlayerShot(hit.collider.name, currentWeapon.damage, transform.name);
                }

                if (hit.collider.CompareTag("Intelligence"))
                {
                    IA ai = hit.collider.gameObject.GetComponent<IA>();
                    ai.TakeDamage(currentWeapon.damage);
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
