using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace WeenieWalker
{
    public class MenuManager : MonoSingleton<MenuManager>
    {

        [SerializeField] GameObject levelNumCanvas;
        [SerializeField] TMP_Text levelText;
        [SerializeField] Color reverseColor = Color.red;
        bool isReversed = false;
        [SerializeField] GameObject winCanvas;
        [SerializeField] GameObject cutsceneObject;
        [SerializeField] GameObject pauseCanvas;

        private void OnEnable()
        {
            GameManager.OnShowLevel += ShowLevel;
            GameManager.OnGameReverse += Reversed;
            GameManager.OnWinGame += WinGame;
            GameManager.OnPauseGame += PauseGame;
        }

        private void OnDisable()
        {
            GameManager.OnShowLevel -= ShowLevel;
            GameManager.OnGameReverse -= Reversed;
            GameManager.OnWinGame -= WinGame;
            GameManager.OnPauseGame -= PauseGame;
        }

        private void ShowLevel(int nextLevel)
        {
            levelText.text = "Level " + nextLevel;
            levelNumCanvas.SetActive(true);

            Invoke("TurnOffCanvas", 2f);
        }

        private void TurnOffCanvas()
        {
            levelNumCanvas.SetActive(false);
        }

        private void Reversed()
        {
            isReversed = true;
            levelText.color = reverseColor;
            Cutscene();
        }

        private void WinGame()
        {
            winCanvas.SetActive(true);
        }

        private void Cutscene()
        {
            cutsceneObject.SetActive(true);
        }

        private void PauseGame(bool isPaused, bool toShowMenu)
        {
            if(toShowMenu)
                pauseCanvas.SetActive(isPaused);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
