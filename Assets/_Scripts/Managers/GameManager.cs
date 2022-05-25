using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeenieWalker
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static event System.Action OnStartGame;
        public static event System.Action<bool> OnPauseGame;
        public static event System.Action<int> OnStartLevel;

        [SerializeField] float levelDelay = 0.25f;

        private int level = 1;


        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            StartGame();
        }

        public void GameOver()
        {
            this.enabled = false;
        }

        private void StartGame()
        {
            level = 1;
            OnStartGame?.Invoke();
        }

        public void PauseGame(bool toPause)
        {
            OnPauseGame?.Invoke(toPause);
        }

        public void LevelCompleted()
        {
            //Pause the game
            //fade to black
            //move character
            //unpause game
        }

    }
}
