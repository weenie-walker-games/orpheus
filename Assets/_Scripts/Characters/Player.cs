using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class Player : MonoBehaviour, IHealth
    {
        [SerializeField] float restartLevelDelay = 1f;
        [SerializeField] float moveSpeed = 1f;
        float moveLimiter = 0.7f;
        float horizontal = 0;
        float vertical = 0;
        Rigidbody rb;

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

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            notes = notesSystem.GetComponent<ParticleSystem>();

        }

        protected void Update()
        {
            
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxis("Vertical");


            //Check if firing music
            if (Input.GetAxis("Fire1") > 0.01f && canFire)
            {
                if (fireRoutine != null) StopCoroutine(fireRoutine);

                fireRoutine = StartCoroutine(Firing());
            }
        }

        private void FixedUpdate()
        {
            if(horizontal != 0 && vertical != 0)
            {
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            rb.velocity = new Vector3(horizontal * moveSpeed, transform.position.y, vertical * moveSpeed);   
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

        private void FireWeapon()
        { 
            //Enemy hitEnemy = component as Enemy;
            //hitEnemy.TakeDamage(enemyDamage);
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
