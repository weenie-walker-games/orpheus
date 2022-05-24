using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeenieWalker
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static event System.Action OnStartGame;

        [HideInInspector] public bool playersTurn = true;
        [SerializeField] float turnDelay = 0.1f;

        private int level = 1;
        [SerializeField] List<EnemyListData> enemyList = new List<EnemyListData>();
        Dictionary<Enemy, bool> currentLevelEnemies = new Dictionary<Enemy, bool>();
        List<EnemyListData> levelEnemies = new List<EnemyListData>();
        bool enemiesMoving;
        Coroutine enemyRoutine;



        private void OnEnable()
        {
            enemyList.Clear();
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
            Debug.Log("Gamemanager start game");
            level = 1;
            OnStartGame?.Invoke();
        }

        private void GetCurrentEnemies()
        {
            levelEnemies.Clear();
            levelEnemies = enemyList.Where(t => t.level == level && t.isAlive == true).ToList();

        }

        public void AddEnemyToList(Enemy enemy, int level, bool isAlive)
        {
            enemyList.Add(new EnemyListData(enemy, level, isAlive));           
        }

        public void UpdateEnemyInfo(Enemy enemy, bool isAlive)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].enemy == enemy)
                    enemyList[i].isAlive = isAlive;
            }
        }

        private void Update()
        {
            Debug.Log("Players Turn = " + playersTurn);

            if (playersTurn || enemiesMoving)
                return;

            if(enemyRoutine != null)
                StopCoroutine(enemyRoutine);
            enemyRoutine = StartCoroutine(MoveEnemies());
        }

        IEnumerator MoveEnemies()
        {
            Debug.Log("In moveenemies");

            //Update the dictionary for the alive enemies
            GetCurrentEnemies();

            enemiesMoving = true;
            yield return new WaitForSeconds(turnDelay);

            
            if(levelEnemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            for (int i = 0; i < levelEnemies.Count; i++)
            {
                levelEnemies[i].enemy.MoveEnemy();
                yield return new WaitForSeconds(levelEnemies[i].enemy.MoveTime);

            }

            playersTurn = true;
            enemiesMoving = false;
        }
    }

    [System.Serializable]
    public class EnemyListData
    {
        public Enemy enemy;
        public int level;
        public bool isAlive;

        public EnemyListData(Enemy enemy, int level, bool isAlive)
        {
            this.enemy = enemy;
            this.level = level;
            this.isAlive = isAlive;
        }
    }
}
