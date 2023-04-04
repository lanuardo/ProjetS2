using System.Collections;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [field: SyncVar]
    public bool IsAlive { get; protected set; } = true;


    [SerializeField] private float maxHealth = 100f;

    //SyncVar allow to modify the variable in every instance. Its like ref/global variable.
    [SyncVar] private float _currentHealth;

    [SerializeField] 
    private Behaviour[] disableOnDeath;
    private bool[] _wasEnabledOnStart;
    
    public void Setup()
    {
        //put whether a component was enabled or not in the bool array from the array of components
        _wasEnabledOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            _wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    private void SetDefaults()
    {
        IsAlive = true;
        _currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            //re-enable the components which were enabled at the start (when the player was alive)
            disableOnDeath[i].enabled = _wasEnabledOnStart[i];
        }

        //enable collider
        Physics.IgnoreLayerCollision(6,7,false);
        //Doesn't work ! Look comment below in Die method.

    }

    private IEnumerator Respawn()
    {
        //delay of spawn
        yield return new WaitForSeconds(GameManager.Instance.matchSettings.respawnTimer);
        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        
        //disable character controller , otherwise cannot assign new position because of the update method in FPS controller
        CharacterController _characterController = GetComponent<CharacterController>();
        _characterController.enabled = false;
        
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        SetDefaults();
        
        //enable it again
        _characterController.enabled = true;
    }
    private void Update()
    {
        //for test : pressing "k" kills the player by inflicting 200 damage.
        if (!isLocalPlayer)
            return;
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(200);
        }


    }

    [ClientRpc] //server to client
    public void RpcTakeDamage(float damage)
    {
        if (!IsAlive)
            return;
        
        _currentHealth -= damage;
        Debug.Log(transform.name + " has now " + _currentHealth + " HP");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsAlive = false;

        foreach (var t in disableOnDeath)
        {
            t.enabled = false;
        }
        
        //disable collider
        Physics.IgnoreLayerCollision(6,7,true);
        //!! Doesn't work cuz Character Controller has its own Capsule Collider and cannot do anything about it !!
        //feat: many people complained about it but still no new features !
        

        Debug.Log(transform.name + " has been killed");

        StartCoroutine(Respawn());
    }
}