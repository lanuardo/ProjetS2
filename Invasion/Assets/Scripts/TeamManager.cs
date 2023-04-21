
using System.Collections.Generic;
using UnityEngine;


    public class TeamManager : MonoBehaviour
    {
        public static List<string> Red = new List<string>();
        public static List<string> Green = new List<string>();
        
        
        
        public static void AddMember(Player player)
        {
            
            if (Red.Count> Green.Count)
            {
                Green.Add(player.name);
                player.RpcSetTeam("green");
                
            }
            else
            {
                Red.Add(player.name);
                player.RpcSetTeam("red");
                
            }
        }
    }
