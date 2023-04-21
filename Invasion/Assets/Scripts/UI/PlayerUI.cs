using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform HealthFill;
        private Player player;

        [SerializeField] private GameObject pauseMenu;

        [SerializeField] private GameObject scoreBoard;

        [SerializeField] private GameObject weaponChoice;

        public void SetPlayer(Player _player)
        {
            player = _player;
        }
        
        
        private void Start()
        {
            PauseMenu.isOn = false;
            WeaponChoice.isOn = false;
        }
        
        
        private void Update()
        {
            SetHealthAmount(player.GetHealthPct());

            

            if ( Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }

            if (!PauseMenu.isOn && !WeaponChoice.isOn && Input.GetKeyDown(KeyCode.Tab))
            {
                scoreBoard.SetActive(true);
            }else if (Input.GetKeyUp(KeyCode.Tab))
            {
                scoreBoard.SetActive(false);
            }

            if ( Input.GetKeyDown(KeyCode.I))
            {
                
                ToggleWeaponMenu();
                
            }

            else if (WeaponChoice.isOn && ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I)))
            {
                ToggleWeaponMenu();
            }
        }

        public void ToggleWeaponMenu()
        {
            weaponChoice.SetActive(!weaponChoice.activeSelf);
            WeaponChoice.isOn = weaponChoice.activeSelf;
           
        }
        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseMenu.isOn = pauseMenu.activeSelf;
            
        }
        
        void SetHealthAmount(float _amount)
        {
            HealthFill.localScale = new Vector3(1f, _amount, 1f);
        }
        
    }

