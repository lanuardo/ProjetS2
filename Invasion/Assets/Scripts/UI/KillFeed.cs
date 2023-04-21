using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeed : MonoBehaviour
{
    [SerializeField] private GameObject killFeedItemPrefab;
    
    
    void Start()
    {
        GameManager.Instance.onPlaterKilledCallBack += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject go = Instantiate(killFeedItemPrefab,transform); 
        go.GetComponent<KillFeedItem>().SetUp(player,source);
        Destroy(go, 4f);
    }
}
