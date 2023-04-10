using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField] private Text text;

    public void SetUp(string player, string source)
    {
        text.text = source + " as killed " + player;
    }
}
