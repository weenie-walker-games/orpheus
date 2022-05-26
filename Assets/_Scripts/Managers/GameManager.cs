using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeenieWalker
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static event System.Action OnStartGame;
        public static event System.Action<bool, bool> OnPauseGame;
        public static event System.Action<Vector3> OnStartLevel;
        public static event System.Action OnEndLevel;
        public static event System.Action OnGameReverse;
        public static event System.Action<bool> OnGameFadingToBlack;
        public static event System.Action<int> OnShowLevel;
        public static event System.Action OnWinGame;

        [SerializeField] float levelDelay = 0.25f;

        [SerializeField] int numLevels = 8;
        [SerializeField] int level = 0;
        bool isPlayingInReverse = false;
        Coroutine levelCompleteRoutine;

        [SerializeField] List<LevelDoorData> doorData = new List<LevelDoorData>();

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Start()
        {

        }

        public void GameOver()
        {
            this.enabled = false;
        }

        public void StartGame(int numberOfLevels)
        {
            numLevels = numberOfLevels;

            StartCoroutine(StartingGame());
        }

        public IEnumerator StartingGame()
        {
            yield return new WaitForSeconds(0.5f);
            OnShowLevel?.Invoke(level + 1);
            yield return new WaitForSeconds(2.5f);
            OnStartLevel?.Invoke(doorData[level].enterDoor.transform.position);
            PauseGame(false);
            OnGameFadingToBlack?.Invoke(false);
            OnStartGame?.Invoke();

        }

        public void PauseButtonPressed()
        {
            PauseGame(false);
        }

        public void PauseGame(bool toPause, bool toShowMenu = true)
        {
            OnPauseGame?.Invoke(toPause, toShowMenu);
        }

        public void LevelCompleted()
        {
            if (levelCompleteRoutine != null) StopCoroutine(levelCompleteRoutine);
            levelCompleteRoutine = StartCoroutine(CompleteLevelRoutine());
        }

        IEnumerator CompleteLevelRoutine()
        {
            Debug.Log("Level complete " + level);

            OnEndLevel?.Invoke();


            //Pause the game
            PauseGame(true, false);

            //fade to black
            OnGameFadingToBlack?.Invoke(false);


            if (level == 0 && isPlayingInReverse)
            {
                WinGame();
                yield break;
            }

            //increment level
            if(level == numLevels && !isPlayingInReverse)
            {
                //Play cutscene
                //Restart final level with player at final door
                isPlayingInReverse = true;
                OnGameReverse?.Invoke();
                yield break;
            }
            else
            {
                if (isPlayingInReverse)
                    level--;
                else
                    level++;
                Debug.Log("Level is " + level);
            }

            //Show the level to the player, adding one to not show 0-based numbering
            OnShowLevel?.Invoke(level + 1);
            yield return new WaitForSeconds(1.5f);



            //move character
            Vector3 newLocation;
            if (isPlayingInReverse)
                newLocation = doorData[level].exitDoor.transform.position;
            else
                newLocation = doorData[level].enterDoor.transform.position;
            OnStartLevel?.Invoke(newLocation);

            //fade in
            OnGameFadingToBlack?.Invoke(false);
            yield return new WaitForSeconds(1f);

            //unpause game
            PauseGame(false);
        }

        private void WinGame()
        {
            if(levelCompleteRoutine != null)
                StopCoroutine(levelCompleteRoutine);

            OnWinGame?.Invoke();
        }

        public void EndCutscene()
        {
            OnShowLevel?.Invoke(level + 1);

            //move character
            Vector3 newLocation;
            newLocation = doorData[level].exitDoor.transform.position;


            //fade in
            OnGameFadingToBlack?.Invoke(false);


            //unpause game
            PauseGame(false);
        }

    }

    [System.Serializable]
    public class LevelDoorData
    {
        public DoorScript enterDoor;
        public DoorScript exitDoor;

    }
}
