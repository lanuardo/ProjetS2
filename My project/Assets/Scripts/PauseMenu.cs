using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PauseMenu : NetworkBehaviour
{
    public static bool isOn = false;

    private NetworkManager _networkManager;
    private void Start()
    {
        _networkManager = NetworkManager.singleton;
    }

    public void LeaveRoomButton()
    {
        if (isClientOnly)
        {
            _networkManager.StopClient();
        }
        else
        {
            _networkManager.StopHost();
        }
    }
}
