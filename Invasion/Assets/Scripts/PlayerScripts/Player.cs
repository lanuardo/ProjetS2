using System;
using System.Collections;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerSetup))]

public class Player : NetworkBehaviour
{
    
    [field: SyncVar]
    public bool IsAlive { get; protected set; } = true;


    [SerializeField] private float maxHealth = 100f;

    //SyncVar allow to modify the variable in every instance. Its like ref/global variable.
    [SyncVar] private float _currentHealth;
    public int startposindex;

    [SyncVar]
    public string team;

    public Material red;
    public Material green;
    public float GetHealthPct()
    {
        return (float)_currentHealth / maxHealth;
    }
    
    public int kills;
    public int deaths;
    
    
    [SerializeField] 
    private Behaviour[] disableOnDeath;
    private bool[] _wasEnabledOnStart;

    [SerializeField] 
    private GameObject[] disableGameObjectsOnDeath;
    
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject spawnEffect;

    private bool firstSetup = true;
    
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip destroySound;

    public void Start()
    {
        if (team == "red")
        {
            transform.Find("Graphics").Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>().material = red;
        }
        else
        {
            transform.Find("Graphics").Find("Beta_Surface").GetComponent<SkinnedMeshRenderer>().material = green;
        }
    }

    public void Setup()
    {
        if (isLocalPlayer)
        {
            // changement de caméra
            GameManager.Instance.SetCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.transform.SetParent(transform);
        }

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupOnAllClient();
    }

    [ClientRpc]
    private void RpcSetupOnAllClient()
    {
        if (firstSetup)
        {
            //put whether a component was enabled or not in the bool array from the array of components
            _wasEnabledOnStart = new bool[disableOnDeath.Length];
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                _wasEnabledOnStart[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;

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

        // reactive les game objects
        foreach (var t in disableGameObjectsOnDeath)
        {
            t.SetActive(true);
        }

        
        //enable collider
        Physics.IgnoreLayerCollision(6,7,false);
        //Doesn't work ! Look comment below in Die method.

        //apparition du systeme de particule de mort
        GameObject _gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

    private IEnumerator Respawn()
    {
        //delay of spawn
        yield return new WaitForSeconds(GameManager.Instance.matchSettings.respawnTimer);

        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();

        if (team=="red")
        {
            spawnPoint = NetworkManager.singleton.GetStartPosition1();
        }
        else
        {
            spawnPoint = NetworkManager.singleton.GetStartPosition2();
        }
        //disable character controller , otherwise cannot assign new position because of the update method in FPS controller
        CharacterController _characterController = GetComponent<CharacterController>();
        _characterController.enabled = false;
        
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);
        
        Setup();
        
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
            RpcTakeDamage(20, transform.name);
        }

        

    }

    [ClientRpc]
    public void RpcSetTeam(string _team)
    {
        team = _team;
    }
    
    [ClientRpc] //server to client
    public void RpcTakeDamage(float damage, [CanBeNull] string sourceID)
    {
        if (!IsAlive)
            return;
        
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(hitSound);
        _currentHealth -= damage;
        if (_currentHealth<0)
        {
            _currentHealth = 0;
        }
        Debug.Log(transform.name + " has now " + _currentHealth + " HP");
        
        if (_currentHealth <= 0)
        {
            Die(sourceID);
        }
    }

    
    
    private void Die([CanBeNull] string sourceID)
    {
        IsAlive = false;
        if (sourceID is not null)
        {
            Player sourcePlayer = GameManager.GetPlayer(sourceID);
            if (sourcePlayer is not null )
            {
                if (sourceID != name)
                {
                    sourcePlayer.kills++;
                }
                
                GameManager.Instance.onPlaterKilledCallBack.Invoke(  transform.name, sourcePlayer.name);

            }
        }
        
        deaths++;
        if (team=="red")
        {
            TeamManager.greenscore++;
        }
        else
        {
            TeamManager.redscore++;
        }
        
        // desactive les components lors de la mort
        foreach (var t in disableOnDeath)
        {
            t.enabled = false;
        }
        
        // desactive les game objects
        foreach (var t in disableGameObjectsOnDeath)
        {
            t.SetActive(false);
        }
        
        //disable collider
        Physics.IgnoreLayerCollision(6,7,true);
        //!! Doesn't work cuz Character Controller has its own Capsule Collider and cannot do anything about it !!
        //feat: many people complained about it but still no new features !
        

        //apparition du systeme de particule de mort
        GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
        Debug.Log(transform.name + " has been killed");

        //changement de camera
        if (isLocalPlayer)
        {
            GameManager.Instance.SetCameraActive(true);
            
        }
        
        StartCoroutine(Respawn());
    }
}