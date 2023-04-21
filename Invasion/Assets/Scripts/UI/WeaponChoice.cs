using Mirror;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
    [SerializeField] private GameObject weaponChoiceItem;

    [SerializeField] private Transform weaponChoiceList;

    [SerializeField] public WeaponData[] weaponList;

    public static bool isOn=false;
    private void OnEnable()
    {
        
        foreach (WeaponData weapon in weaponList)
        {
            GameObject itemgo = Instantiate(weaponChoiceItem, weaponChoiceList);
            WeaponChoiceItem item = itemgo.GetComponent<WeaponChoiceItem>();
            if (item != null)
            {
                item.Setup(weapon);
            }
        }
    }

    private void OnDisable()
    {
        //  vider la liste 
        foreach (Transform child in weaponChoiceList)
        {
            Destroy(child.gameObject);
        }
    }
}
