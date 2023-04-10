<<<<<<< HEAD
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
=======
using System;
using System.Collections.Generic;
using UnityEngine;

>>>>>>> parent of 29af99ed (void)
public class GameManager : MonoBehaviour
{
    private static Dictionary<string, Player> _players = new Dictionary<string, Player>();
    private const string PlayerIdPrefix = "Player";
    public MatchSettings matchSettings;
<<<<<<< HEAD

    public delegate void OnPlaterKilledCallBack(string player, string source);

    public OnPlaterKilledCallBack onPlaterKilledCallBack;
    
=======
>>>>>>> parent of 29af99ed (void)
    
    //making singleton
    public static GameManager Instance;

<<<<<<< HEAD

    [SerializeField] private GameObject sceneCamera;
=======
>>>>>>> parent of 29af99ed (void)
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        
        Debug.LogError("More than one instance of GameManager in the scene");
    }

<<<<<<< HEAD
    public void SetCameraActive(bool isActive)
    {
        if (sceneCamera is null)
        {
            return;
        }
        
        sceneCamera.SetActive(isActive);
    }
    
=======
>>>>>>> parent of 29af99ed (void)
    public static void RegisterPlayer(string netID, Player player)
    {
        //Add in the dictionary "Player" + id as key (his name), and the object Player.
        string playerId = PlayerIdPrefix + netID;
        _players.Add(playerId,player);
        //Rename the object Player as "Player" + id.
        player.transform.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        //Remove the player from the dictionary.
        _players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return _players[playerId];
    }
<<<<<<< HEAD

    public static Player[] GetAllPlayers()
    {
        return _players.Values.ToArray();
    }
=======
>>>>>>> parent of 29af99ed (void)
}
