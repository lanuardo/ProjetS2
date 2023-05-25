using System.Linq;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static Dictionary<string, Player> _players = new Dictionary<string, Player>();
    private static Dictionary<string, IA> _intelligences = new Dictionary<string, IA>();
    private const string PlayerIdPrefix = "Player";
    private const string aiIdPrefix = "AI";







    public MatchSettings matchSettings;

    public delegate void OnPlaterKilledCallBack(string player, string source);

    public OnPlaterKilledCallBack onPlaterKilledCallBack;

    //making singleton
    public static GameManager Instance;


    [SerializeField] private GameObject sceneCamera;
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        
        Debug.LogError("More than one instance of GameManager in the scene");
    }

    public void SetCameraActive(bool isActive)
    {
        if (sceneCamera is null)
        {
            return;
        }
        
        sceneCamera.SetActive(isActive);
    }
    
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
    
    public static void RegisterAI(string netID, IA ai)
    {
        //Add in the dictionary "Player" + id as key (his name), and the object Player.
        string aiId = aiIdPrefix + netID;
        _intelligences.Add(aiId,ai);
        //Rename the object Player as "Player" + id.
        ai.transform.name = aiId;
        
    }
    
    public static void UnregisterAI(string aiId)
    {
        //Remove the player from the dictionary.
        _intelligences.Remove(aiId);
    }

    public static Player GetPlayer(string playerId)
    {
        return _players[playerId];
    }
    
    public static IA GetAI(string aiId)
    {
        return _intelligences[aiId];
    }

    public static Player[] GetAllPlayers()
    {
        return _players.Values.ToArray();
    }
    
    public static IA[] GetAllAIs()
    {
        return _intelligences.Values.ToArray();
    }
}
