using System;
using UnityEngine;
using UnityEngine.UI;

    public class PlayerUI : MonoBehaviour
    {
        
        [SerializeField] private GameObject pauseMenu;

        [SerializeField] private GameObject scoreBoard;

        private void Start()
        {
            PauseMenu.isOn = false;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                scoreBoard.SetActive(true);
            }else if (Input.GetKeyUp(KeyCode.Tab))
            {
                scoreBoard.SetActive(false);
            }
        }

        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseMenu.isOn = pauseMenu.activeSelf;
        }
    }

