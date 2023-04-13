using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private GameObject playerScoreBoardItem;

    [SerializeField] private Transform playerScoreBoardList;

    private void OnEnable()
    {
        //recuperer une array de tout les joueurs du serv
        Player[] players = GameManager.GetAllPlayers();

        // mise en place d'une ligne par joueur

        foreach (var player in players)
        {
            GameObject itemgo = Instantiate(playerScoreBoardItem, playerScoreBoardList);
            ScoreBoardItem item = itemgo.GetComponent<ScoreBoardItem>();
            if (item != null)
            {
                item.Setup(player);
            }
        }
    }

    private void OnDisable()
    {
        //  vider la liste des joueur
        foreach (Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }
        
    }
}
