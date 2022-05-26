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
        [SerializeField] AudioSource audioSource;
        [SerializeField] int enemyDamage = 25;
        [SerializeField] float weaponDamageDistance = 1.5f;
        [SerializeField] float weaponDelay = 0.3f;
        [SerializeField] LayerMask enemyLayer;
        bool canFire = true;
        ParticleSystem notes;

        [SerializeField] private int maxHealth = 100;
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get; private set; }


        Coroutine fireRoutine;


        private void OnEnable()
        {
            GameManager.OnStartGame += Restart;
            GameManager.OnStartLevel += StartNewLevel;
        }

        private void OnDisable()
        {
            GameManager.OnStartGame -= Restart;
            GameManager.OnStartLevel -= StartNewLevel;
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

            FireWeapon();

            //Reset the timer
            float newTime = Time.time + weaponDelay;
            //Delay for cooldown
            while(Time.time < newTime)
            {
                yield return null;
            }

            canFire = true;
        }

        private void FireWeapon()
        {
            Enemy hitEnemy;
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, weaponDamageDistance, hitColliders, enemyLayer);

            for (int i = 0; i < numColliders; i++)
            {
                hitEnemy = null;

                if (hitColliders[i].TryGetComponent(out hitEnemy))
                    hitEnemy.TakeDamage(enemyDamage);
            }

            notes.Play();
            audioSource.Play();
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

        private void StartNewLevel(Vector3 newLocation)
        {
            transform.position = newLocation;
        }
        
    }
}
