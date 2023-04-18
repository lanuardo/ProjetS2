
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "My Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string name = "Gun name";
    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 0f;

    public int magazineSize = 10; //capacité du chargeur (valeur par défaut)

    public float reloadTime = 1.5f; //temsp de rechargement de l'arme

    public GameObject graphics;

    public AudioClip shootSound;
    public AudioClip reloadSound;
}
