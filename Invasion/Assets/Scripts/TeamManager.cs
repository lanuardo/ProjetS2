
using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



    public class TeamManager : NetworkBehaviour
    {
        [SerializeField] public Text redscoreText;
        [SerializeField] public Text greenscoreText;

        [SerializeField] public Text GoalText;

        [SerializeField] private GameObject popup;

        public static bool popupisactive;
        public static List<Player> Red = new List<Player>();
        public static List<Player> Green = new List<Player>();

        public static int redscore;
        public static int greenscore;
        
        private NetworkManager _networkManager;
        private void Start()
        {
            popupisactive = false;
            _networkManager = NetworkManager.singleton;
            if (GoalText is not null)
            {
                GoalText.text += GameManager.Instance.matchSettings.goal;

            }
        }

        public void Exit()
        {
            if (isClientOnly)
            {
                popup.SetActive(false);
                _networkManager.StopClient();
            }
            else
            {
                popup.SetActive(false);
                _networkManager.StopHost();
            }
        }
    

        

        private void Update()
        {
            if (redscoreText is not null && greenscoreText is not null)
            {
                redscoreText.text = ""+redscore;
                greenscoreText.text = ""+greenscore;
                if (redscore>=GameManager.Instance.matchSettings.goal && greenscore>=GameManager.Instance.matchSettings.goal)
                {
                    var text=popup.GetComponent<Text>();
                    text.text = "Draw";
                    popup.SetActive(true);
                    popupisactive = true;
                }
                else if (redscore>=GameManager.Instance.matchSettings.goal)
                {
                    var text=popup.GetComponent<Text>();
                    text.text = "Red team win";
                    popup.SetActive(true);
                    popupisactive = true;
                }
                else if (greenscore>=GameManager.Instance.matchSettings.goal)
                {
                    var text=popup.GetComponent<Text>();
                    text.text = "Green team win";
                    popup.SetActive(true);
                    popupisactive = true;
                }

                if (popupisactive)
                {
                    if (Cursor.lockState!=CursorLockMode.None)
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                    
                    foreach (var player in GameManager.GetAllPlayers())
                    {
                        player.deaths = 0;
                        player.kills = 0;
                        var a=player.GetComponent<Throwing>();
                        a.totalThrows = 2;
                        var b = player.GetComponent<SpawnAI>();
                        b.totalSpawns = 2;
                        Invoke("Exit",5f);
                    }
                    
                    
                }
            }
            
        }

        
        public static int GetScore(string teamname)
        {
            int score = 0;
            if (teamname=="red")
            {
                foreach (var player in Green)
                {
                    score += player.deaths;
                }
            }
            else
            {
                foreach (var player in Red)
                {
                    score += player.deaths;
                }
            }

            return score;
        }
        
        public static string GetTeam(Player player)
        {
            if (Red.Contains(player))
            {
                return "red";
            }

            return "green";
        }
        
        public static void AddMember(Player player)
        {
            
            if (Red.Count> Green.Count)
            {
                Green.Add(player);
                player.RpcSetTeam("green");
                
            }
            else
            {
                Red.Add(player);
                player.RpcSetTeam("red");
                
            }
        }
    }
