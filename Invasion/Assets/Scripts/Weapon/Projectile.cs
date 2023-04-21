using Mirror;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public Player thrower;
    [Header("Stats")] 
    public int damage;
    public bool destroyOnHit;

    [Header("Effects")]
    public GameObject muzzleEffect;
    public GameObject hitEffect;

    [Header("Explosive Projectile")]
    public bool isExplosive;
    public float explosionRadius;
    public float explosionForce;
    public float explosionTime;
    public int explosionDamage;
    public GameObject explosionEffect;

    private Rigidbody _rb;

    private bool _hitTarget;

    private void Start()
    {
        
        // get rigidbody component
        _rb = GetComponent<Rigidbody>();

        // spawn muzzleEffect (if assigned)
        if(muzzleEffect != null)
            Instantiate(muzzleEffect, transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, 0.5f);
        thrower = objectsInRange[0].GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hitTarget)
            return;
        else
            _hitTarget = true;

        
        // enemy hit
        if (collision.collider.CompareTag("Player"))
        {
            CmdPlayerHit(collision.collider.name, damage, thrower.name);

            // spawn hit effect (if assigned)
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            
            // destroy projectile
            if (!isExplosive && destroyOnHit)
                Invoke(nameof(DestroyProjectile), 0.1f);
        }
        

        // explode projectile if it's explosive
        if (isExplosive)
        {
            Invoke("Explode",explosionTime);
            return;
        }

        // make sure projectile sticks to surface
        _rb.isKinematic = true;

        // make sure projectile moves with target
        transform.SetParent(collision.transform);
    }

    [Command]
    private void CmdPlayerHit(string playerId, float damage, string sourceID)
    {
        Debug.Log(playerId + " a été touché.");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage, sourceID);
    }

    
    private void Explode()
    {
        
        // spawn explosion effect (if assigned)
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // find all the objects that are inside the explosion range
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);

        // loop through all of the found objects and apply damage and explosion force
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            if (objectsInRange[i].gameObject == gameObject )
            {
                // don't break or return please, thanks
            }
            else
            {
                
                // check if object is enemy, if so deal explosionDamage
                if (objectsInRange[i].CompareTag("Player"))
                {
                    objectsInRange[i].GetComponent<Player>().RpcTakeDamage(explosionDamage,thrower.name);
                }



                // check if object has a rigidbody
                if (objectsInRange[i].GetComponent<Rigidbody>() != null)
                {
                    // custom explosionForce
                    Vector3 objectPos = objectsInRange[i].transform.position;

                    // calculate force direction
                    Vector3 forceDirection = (objectPos - transform.position).normalized;

                    // apply force to object in range
                    objectsInRange[i].GetComponent<Rigidbody>().AddForceAtPosition(forceDirection * explosionForce + Vector3.up * explosionForce, transform.position + new Vector3(0,-0.5f,0), ForceMode.Impulse);

                    Debug.Log("Kabooom " + objectsInRange[i].name);
                }
            }
        }

        // destroy projectile with 0.1 seconds delay
        Invoke(nameof(DestroyProjectile), 0.1f);
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    // just graphics stuff
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}