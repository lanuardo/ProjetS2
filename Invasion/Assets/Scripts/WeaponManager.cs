using UnityEngine;
using Mirror;
public class WeaponManager : NetworkBehaviour
{
    [SerializeField] private WeaponData primaryWeapon;

    [SerializeField] private string weaponLayerName = "Weapon";

    [SerializeField] private Transform weaponHolder;
    
    private WeaponData _currentWeapon;
    private WeaponGraphics _currentGraphics;
    void Start()
    {
        
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(WeaponData _weapon)
    {
        _currentWeapon = _weapon;
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
    
}
