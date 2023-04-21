using System.Collections;

using UnityEngine;
using UnityEngine.UI;
public class WeaponChoiceItem : MonoBehaviour
{
    [SerializeField] public Text weaponText;
    [SerializeField] public Player player;
    private WeaponData weaponData;
    private WeaponManager _weaponManager;
    public void Setup(WeaponData weapon )
    {
        weaponText.text = weapon.name;
        weaponData = weapon;
    }

    
    void EquipNewWeapon()
    {
        _weaponManager= player.GetComponent<WeaponManager>();
        // Détruit l'arme actuelle du joueur
        Destroy(_weaponManager.GetCurrentGraphics().gameObject);

        // Equipe la nouvelle arme
        _weaponManager.EquipWeapon(weaponData);
        
    }
    
   

    
    
}
