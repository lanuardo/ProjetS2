
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class IA : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health = 50f;
    [SerializeField] private GameObject explosionEffect;
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
    }

    private void Update()
    {
        var pl = FindLocalPlayer();
        if (pl is not null)
        {
            player = pl.transform;
        }
        
        
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange)
        {
            Patrolling();
        }
        
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            Explode();
        }
    }

    private GameObject FindLocalPlayer()
    {
        int i = 0;
        var array = FindObjectsOfType<GameObject>();
        int lim = array.Length;
        while (i<lim && array[i].layer != 6)
        {
            i++;
        }

        if (i<lim)
        {
            return array[i];
        }
        else
        {
            return null;
        }
        
        
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
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(Explode), 0.25f);
    }
    
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    
}
