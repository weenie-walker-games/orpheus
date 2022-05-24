using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class Player : MovingObject, IHealth
    {
        [SerializeField] float restartLevelDelay = 1f;

        [SerializeField] GameObject notesSystem;
        [SerializeField] int enemyDamage = 1;
        [SerializeField] float weaponDelay = 0.3f;
        bool canFire = true;
        ParticleSystem notes;

        [SerializeField] private int maxHealth = 100;
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get; private set; }


        Coroutine fireRoutine;


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
            base.Start();

            notes = notesSystem.GetComponent<ParticleSystem>();

        }

        protected void Update()
        {
            if (!GameManager.Instance.playersTurn) return;

            Debug.Log("Player is moving");

            int horizontal = 0;
            int vertical = 0;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxis("Vertical");

            if (horizontal != 0)
                vertical = 0;

            if (horizontal != 0 || vertical != 0)
                AttemptMove<Enemy>(horizontal, vertical);


            ////Check if firing music
            //if (Input.GetAxis("Fire1") > 0.01f && canFire)
            //{
            //    if (fireRoutine != null) StopCoroutine(fireRoutine);

            //    fireRoutine = StartCoroutine(Firing());
            //}
        }

        IEnumerator Firing()
        {
            canFire = false;

            notes.Play();

            float newTime = Time.time + weaponDelay;

            while(Time.time < newTime)
            {
                yield return null;
            }

            canFire = true;
        }


        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            base.AttemptMove<T>(xDir, yDir);

            RaycastHit2D hit;

            if(Move (xDir, yDir, out hit))
            {

            }

            CheckIfGameOver();

            GameManager.Instance.playersTurn = false;
        }

        protected override void OnCantMove<T>(T component)
        {
            Enemy hitEnemy = component as Enemy;
            hitEnemy.TakeDamage(enemyDamage);
            notes.Play();
        }

        public void TakeDamage(int loss)
        {
            CurrentHealth -= loss;
            CheckIfGameOver();
        }

        private void Restart()
        {
            CurrentHealth = maxHealth;
        }
    

        private void CheckIfGameOver()
        {
            if(CurrentHealth <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }
        
    }
}
