
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IA : NetworkBehaviour
{
    public GameObject noisesource;
    public float explosionRadius;

    public float explosionForce;
    public NavMeshAgent agent;
    public Transform player;
    public Player MyPlayer;
    public Player Ennemyplayer = null;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health = 50f;
    [SerializeField] private GameObject explosionEffect; 
    [SerializeField] private float explosionDamage = 50f;
    //[SerializeField] private AudioClip explosionSound;


    //Patrolling
    public Vector3 walkPoint;
    [SerializeField]private bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        MyPlayer = FindMyPlayer();
    }

    private void Update()
    {

       Action();
    }
    
    private void Action()
    {
         if (Ennemyplayer is null) //enemy not found
         {

            //check for sight and attack range

            
            Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange);
            //Debug.Log("colliders size : " + colliders.Length);
            if (colliders.Length != 0)
            {
                int i = 0;
                while (i < colliders.Length && !colliders[i].CompareTag("Player"))
                {
                    //Debug.Log("colliders" + i + " : " + colliders[i]);
                    i++;
                }

                if (i < colliders.Length)
                {
                    Ennemyplayer = colliders[i].GetComponent<Player>();
                    playerInSightRange = Ennemyplayer.team != MyPlayer.team;
                    player = Ennemyplayer.transform;
                }
                else
                {
                    Ennemyplayer = null;
                }
            }
            Collider[] colliders2  = Physics.OverlapSphere(transform.position, attackRange);
            if (colliders2.Length != 0)
            {
                int j = 0;
                while (j < colliders2.Length && !colliders2[j].CompareTag("Player"))
                {
                    j++;
                }

                if (j < colliders2.Length)
                {
                    Ennemyplayer = colliders2[j].GetComponent<Player>();
                    playerInAttackRange = Ennemyplayer.team != MyPlayer.team;
                    player = Ennemyplayer.transform;
                }
                else
                {
                    Ennemyplayer = null;
                }
            }
            

            //Debug.Log(player + " in sight range : "+ playerInSightRange);
            //Debug.Log(player + " in attack range : "+ playerInAttackRange);
            
            if (!playerInSightRange)
            {
                Patrolling();
                Ennemyplayer = null;
            }
        
            else if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }

            else if (playerInSightRange && playerInAttackRange) 
            {
                Debug.Log("going into explosion");
                Invoke(nameof(Explode2), 0.25f);
                //Player uwu = GameManager.GetPlayer(player.name);
                //uwu.RpcTakeDamage(explosionDamage,null);
            }
         }
    }
    
    private Player FindMyPlayer()
    {
        Player[] players = GameManager.GetAllPlayers();
        Player closest = null;
        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;
        foreach (var v in players)
        {
            Vector3 diff = v.transform.position - pos;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = v;
                distance = curDistance;
            }
        }

        return closest;
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        //Walk point reached

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    
    private void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

    private void Explode()
    {
        //Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        GameObject _gfxIns = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns,3f);
        //AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(explosionSound);
        DestroyEnemy();
        
    }
    
    private void Explode2()
    {
        GameObject b = null;
        // spawn explosion effect (if assigned)
        if (explosionEffect != null)
        {
            b = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (b is not null)
        {
            Destroy(b,2f);
        }
        var a=Instantiate(noisesource, transform.position, Quaternion.identity);
        Destroy(a,2f);
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
                    objectsInRange[i].GetComponent<Player>().RpcTakeDamage(explosionDamage,null);
                }



                /*// check if object has a rigidbody
                if (objectsInRange[i].GetComponent<Rigidbody>() != null)
                {
                    // custom explosionForce
                    Vector3 objectPos = objectsInRange[i].transform.position;

                    // calculate force direction
                    Vector3 forceDirection = (objectPos - transform.position).normalized;

                    // apply force to object in range
                    objectsInRange[i].GetComponent<Rigidbody>().AddForceAtPosition(forceDirection * explosionForce + Vector3.up * explosionForce, transform.position + new Vector3(0,-0.5f,0), ForceMode.Impulse);

                    Debug.Log("Kabooom " + objectsInRange[i].name);
                }*/
            }
        }

        DestroyEnemy();
    }

     
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(Explode2), 0.25f);
    }
    
    private void DestroyEnemy()
    {
        //GameManager.UnregisterAI(gameObject.transform.name);
        NetworkServer.Destroy(gameObject);
    }
    
}
