using UnityEngine;
using UnityEngine.UI;
public class ScoreBoardItem : MonoBehaviour
{
    [SerializeField] public Text usernameText;

    [SerializeField] public Text killsText;
    [SerializeField] public Text deathText;

    public void Setup(Player player)
    {
        usernameText.text = player.name;
        killsText.text = $"Kills: {player.kills}";
        deathText.text = $"Deaths: {player.deaths}";

    }
    
}
