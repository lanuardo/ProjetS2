<<<<<<< HEAD
﻿using System;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> parent of 29af99ed (void)
using UnityEngine.UI;

namespace Mirror.Examples.Basic
{
<<<<<<< HEAD


    public class PlayerUI : MonoBehaviour
    {
        [Header("Player Components")] public Image image;

        [Header("Child Text Objects")] public Text playerNameText;
        public Text playerDataText;

        [SerializeField] private GameObject pauseMenu;

=======
    public class PlayerUI : MonoBehaviour
    {
        [Header("Player Components")]
        public Image image;

        [Header("Child Text Objects")]
        public Text playerNameText;
        public Text playerDataText;

>>>>>>> parent of 29af99ed (void)
        // Sets a highlight color for the local player
        public void SetLocalPlayer()
        {
            // add a visual background for the local player in the UI
            image.color = new Color(1f, 1f, 1f, 0.1f);
        }

        // This value can change as clients leave and join
        public void OnPlayerNumberChanged(byte newPlayerNumber)
        {
            playerNameText.text = string.Format("Player {0:00}", newPlayerNumber);
        }

        // Random color set by Player::OnStartServer
        public void OnPlayerColorChanged(Color32 newPlayerColor)
        {
            playerNameText.color = newPlayerColor;
        }

        // This updates from Player::UpdateData via InvokeRepeating on server
        public void OnPlayerDataChanged(ushort newPlayerData)
        {
            // Show the data in the UI
            playerDataText.text = string.Format("Data: {0:000}", newPlayerData);
        }
<<<<<<< HEAD

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}

=======
    }
}
>>>>>>> parent of 29af99ed (void)
