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
        public static event System.Action<Vector3> OnStartLevel;
        public static event System.Action OnEndLevel;
        public static event System.Action OnGameReverse;

        [SerializeField] float levelDelay = 0.25f;

        [SerializeField] int numLevels = 8;
        [SerializeField] int level = 0;
        bool isPlayingInReverse = false;

        [SerializeField] List<LevelDoorData> doorData = new List<LevelDoorData>();

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

            OnStartLevel?.Invoke(doorData[level].enterDoor.transform.position);
            OnStartGame?.Invoke();
        }

        public void PauseGame(bool toPause)
        {
            OnPauseGame?.Invoke(toPause);
        }

        public void LevelCompleted()
        {
            Debug.Log("Level complete " + level);

            OnEndLevel?.Invoke();


            //Pause the game
            PauseGame(true);

            //fade to black

            if (level == 0 && isPlayingInReverse)
            {
                WinGame();
                return;
            }

            //increment level
            if(level == numLevels && !isPlayingInReverse)
            {
                //Play cutscene
                //Restart final level with player at final door
                isPlayingInReverse = true;
                OnGameReverse?.Invoke();
            }
            else
            {
                if (isPlayingInReverse)
                    level--;
                else
                    level++;
                Debug.Log("Level is " + level);
            }

            //move character
            Vector3 newLocation;
            if (isPlayingInReverse)
                newLocation = doorData[level].exitDoor.transform.position;
            else
                newLocation = doorData[level].enterDoor.transform.position;
            OnStartLevel?.Invoke(newLocation);

            //fade in

            //unpause game
            PauseGame(false);
        }

        private void WinGame()
        {
            Debug.Log("You win!");
        }

    }

    [System.Serializable]
    public class LevelDoorData
    {
        public DoorScript enterDoor;
        public DoorScript exitDoor;

    }
}
