
using Mirror;
using UnityEngine;


public class Throwing : NetworkBehaviour
{
    [Header("References")]
    public Transform cam;
    public GameObject attackPoint;
    public GameObject objectToThrow;
    public KeyCode throwInp;

    [Header("Stats")]
    public int totalThrows;
    public float throwCooldown, timeBetweenThrows;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;
    public float spread;
    public int throwsPerTap;

    int _throwsLeft, _throwsToExecute;

    [Header("RayCasting")]
    public bool useRaycasts;
    public RaycastHit RayHit;
    public LayerMask whatIsEnemy;

    [Header("Extra Settings")]
    public bool allowButtonHold;
    bool _throwing, _readyToThrow, _reloading;

    //Graphics
    ///public CamShake camShake;
    ///public float camShakeMagnitude, camShakeDuration;

    
    private void Start()
    {
        _throwsLeft = totalThrows;

        
        _readyToThrow = true;
    }
    private void Update()
    {
        MyInput();

        // set info text
    }
    
    [Client]
    private void MyInput()
    {
        if (allowButtonHold) _throwing = Input.GetKey(throwInp);
        else _throwing = Input.GetKeyDown(throwInp);

        // throw
        if (_readyToThrow && _throwing && !_reloading && _throwsLeft > 0)
        {
            _throwsToExecute = throwsPerTap;
            Throw();
        }
    }

    [ClientRpc]
    void Rpcthrow()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        GameObject projectile =  Instantiate(objectToThrow, attackPoint.transform.position, cam.rotation);
        
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        
        Vector3 forceDirection = cam.transform.forward + new Vector3(x, y, 0);

        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        
        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.transform.position).normalized + new Vector3(x, y, 0);
        }
        
        // use raycasts if needed
        if (useRaycasts)
        {
            if (Physics.Raycast(cam.transform.position, forceDirection, out RayHit, whatIsEnemy))
            {
                Debug.Log(RayHit.collider.name);

                //if (rayHit.collider.CompareTag("Enemy"))
                //    rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
            }
        }
    }
    
    [Command]
    private void CmdThrow()
    {
        
        Rpcthrow();
        
    }
    
    
    private void Throw()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        _readyToThrow = false;

        // spread
        
        
        // instantiate object to throw
        CmdThrow();
        
        
        
        _throwsLeft--;
        _throwsToExecute--;

        // execute multiple throws per tap
        if (_throwsToExecute > 0 && _throwsLeft > 0)
            Invoke(nameof(Throw), timeBetweenThrows);

        else if(_throwsToExecute <= 0)
            Invoke(nameof(ResetThrow), throwCooldown);
    }
    private void ResetThrow()
    {
        _readyToThrow = true;
    }
}
