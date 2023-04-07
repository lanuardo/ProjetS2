using System;
using UnityEngine;
using UnityEngine.UI;

    public class PlayerUI : MonoBehaviour
    {
        
        [SerializeField] private GameObject pauseMenu;

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
        }

        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseMenu.isOn = pauseMenu.activeSelf;
        }
    }

