using System.Collections;

using UnityEngine;
using UnityEngine.UI;
public class WeaponChoiceItem : MonoBehaviour
{
    [SerializeField] public Text weaponText;
    private WeaponData _weaponData;
    private WeaponManager _weaponManager;
    public void Setup(WeaponData weapon )
    {
        weaponText.text = weapon.name;
        _weaponData = weapon;
        
    }

    private WeaponManager findweapon()
    {
        var nav = transform;
        while (_weaponManager is null)
        {
            _weaponManager = nav.GetComponent<WeaponManager>();
            nav = nav.parent;
        }

        return _weaponManager;
    }
    
    public void EquipNewWeapon()
    {
        Debug.Log(_weaponData.name);
        _weaponManager = findweapon();
        // Détruit l'arme actuelle du joueur
        Destroy(_weaponManager.GetCurrentGraphics().gameObject);

        // Equipe la nouvelle arme
        _weaponManager.EquipWeapon(_weaponData);
        
    }
    
   

    
    
}
