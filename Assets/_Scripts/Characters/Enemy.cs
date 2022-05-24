using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class Enemy : MovingObject, IHealth
    {

        [SerializeField] int enemyLevel;
        bool isAlive = true;
        [SerializeField] int playerDamage;
        [SerializeField] bool isSeeking = false;
        [SerializeField] ParticleSystem attackSystem;
        [SerializeField] ParticleSystem deathSystem;
        private Transform target;
        private bool skipMove;

        [SerializeField] private int maxHealth = 100;
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get; private set; }



        private void OnEnable()
        {
            GameManager.OnStartGame += Restart;
        }

        private void OnDisable()
        {
            GameManager.OnStartGame -= Restart;
        }

        protected override void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            GameManager.Instance.AddEnemyToList(this, enemyLevel, isAlive);
            base.Start();
        }

        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            if (skipMove)
            {
                skipMove = false;
                return;
            }

            base.AttemptMove<T>(xDir, yDir);

            skipMove = true;
        }

        public void MoveEnemy()
        {
            int xDir = 0;
            int yDir = 0;

            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
                yDir = target.position.y > transform.position.y ? 1 : -1;
            else
                xDir = target.position.x > transform.position.x ? 1 : -1;


            AttemptMove<Player>(xDir, yDir);

        }

        protected override void OnCantMove<T>(T component)
        {
            Player hitPlayer = component as Player;
            attackSystem.Play();
            hitPlayer.TakeDamage(playerDamage);


        }

        public void TakeDamage(int loss)
        {
            CurrentHealth -= loss;
            

            if(CurrentHealth <= 0)
            {
                deathSystem.Play();
                isAlive = false;
                GameManager.Instance.UpdateEnemyInfo(this, isAlive);
                Invoke("DisableEnemy", 1f);
            }
        }

        private void DisableEnemy()
        {
            this.gameObject.SetActive(false);
        }

        private void Restart()
        {
            CurrentHealth = maxHealth;
        }

    }
}
