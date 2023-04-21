using UnityEngine;

namespace Mirror
{
    /// <summary>Start position for player spawning, automatically registers itself in the NetworkManager.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Start Position 1")]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-start-position")]
    public class NetworkStartPosition1 : MonoBehaviour
    {
        public void Awake()
        {
            NetworkManager.RegisterStartPosition1(transform);
        }

        public void OnDestroy()
        {
            NetworkManager.UnRegisterStartPosition1(transform);
        }
    }
}
