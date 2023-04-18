using UnityEngine;
using Mirror;
using System.Collections;
public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private WeaponData primaryWeapon;

    [SerializeField] private string weaponLayerName = "Weapon";

    [SerializeField] private Transform weaponHolder;

    [HideInInspector] public int currentMagazineSize; // Quantité de munitions actuelle de l'arme du joueur

    private WeaponData _currentWeapon;

    private WeaponGraphics _currentGraphics;

    public bool isReloading = false;

    void Start()
    {
        
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(WeaponData _weapon)
    {
        _currentWeapon = _weapon;

        currentMagazineSize = _weapon.magazineSize; //Lorsque l'on s'equipe d'une arme, le chargeur est automatiquement rempli

        GameObject weaponIns = Instantiate(_weapon.graphics,weaponHolder.position,weaponHolder.rotation);
        
        weaponIns.transform.SetParent(weaponHolder);

        _currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if (_currentGraphics==null)
        {
            Debug.LogError("no weapongraphics in : "+ weaponIns.name);
        }

        if (isLocalPlayer)
        {
            Utilitaire.SetLayerRecursively(weaponIns,LayerMask.NameToLayer(weaponLayerName));
        }
        
    }

    public WeaponData getcurrentWeapon()
    {
        return _currentWeapon;
    }
    
    public WeaponGraphics getcurrentWeaponGraphics()
    {
        return _currentGraphics;
    }

    public IEnumerator Reload()
    {
        if (isReloading)
        {
            yield break; //return ne marche pas avec Coroutine
        }

        Debug.Log("Reloading ...");

        isReloading = true;

        //CmdOnReload(); for animation

        yield return new WaitForSeconds(_currentWeapon.reloadTime); //on cree un delai de rechargement

        currentMagazineSize = _currentWeapon.magazineSize;

        isReloading = false;

        Debug.Log("Reloading finished ...");
    }

}
