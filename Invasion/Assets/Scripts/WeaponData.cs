
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "My Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string name = "Gun name";
    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 0f;

    public GameObject graphics;
}
