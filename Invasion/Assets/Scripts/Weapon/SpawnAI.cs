using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnAI : NetworkBehaviour
{
    public Transform cam;
    public GameObject attackPoint;
    public IA objectRed;
    public IA objectGreen;
    public KeyCode spawnInp;
    [Header("Extra Settings")]
    public bool allowButtonHold;
    bool _spawning, _readyToSpawn, _reloading;
    public int spawnsPerTap;
    public int totalSpawns;
    public float spawnCooldown, timeBetweenSpawns;
    public Player MyPlayer;

    int _spawnsLeft, _spawnsToExecute;

    // Start is called before the first frame update
    void Start()
    {
        _spawnsLeft = totalSpawns;
        MyPlayer = GetComponent<Player>();
        _readyToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }


    [Client]
    private void MyInput()
    {
        if (allowButtonHold) _spawning = Input.GetKey(spawnInp);
        else _spawning = Input.GetKeyDown(spawnInp);

        // throw
        if (_readyToSpawn && _spawning && !_reloading && _spawnsLeft > 0)
        {
            _spawnsToExecute = spawnsPerTap;
            SpawnObject();
        }
    }

    [ClientRpc]
    void RpcSpawnObject()
    {
        if (MyPlayer.team == "red")
        {
            IA ai =  Instantiate(objectRed, attackPoint.transform.position, cam.rotation);

        }
        else
        {
            IA ai =  Instantiate(objectGreen, attackPoint.transform.position, cam.rotation);

        }
        //GameManager.RegisterAI(objectToSpawn.GetComponent<NetworkIdentity>().netId.ToString(), objectToSpawn);
        //NetworkServer.Spawn(ai.gameObject);
    }

    [Command]
    private void CmdSpawn()
    {
        RpcSpawnObject();
    }

    private void SpawnObject()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        _readyToSpawn = false;
        
        CmdSpawn();

        _spawnsLeft -= 1;
        _spawnsToExecute -= 1;
        
        if (_spawnsToExecute > 0 && _spawnsLeft > 0)
            Invoke(nameof(SpawnObject), timeBetweenSpawns);

        else if(_spawnsToExecute <= 0)
            Invoke(nameof(ResetSpawn), spawnCooldown);
    }
    private void ResetSpawn()
    {
        _readyToSpawn = true;
    }
}
