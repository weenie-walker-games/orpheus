using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WeenieWalker
{
    public class Enemy : MonoBehaviour, IHealth
    {

        [SerializeField] int enemyLevel;
        bool isAlive = true;
        [SerializeField] int playerDamage = 5;
        [SerializeField] bool isSeeking = false;
        [SerializeField] ParticleSystem attackSystem;
        [SerializeField] ParticleSystem deathSystem;
        [SerializeField] NavMeshAgent agent;
        Player playerTarget;
        [SerializeField] float distanceCatchTarget = 0.75f;
        [SerializeField] float weaponDelay = 0.5f;


        [SerializeField] private int maxHealth = 100;
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get; private set; }

        bool hasCaughtTarget = false;
        bool isChasingTarget = false;

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

        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player triggered");
                hasCaughtTarget = false;
                isChasingTarget = true;
                playerTarget = other.GetComponent<Player>();

                if (playerTarget != null)
                {
                    agent.SetDestination(playerTarget.transform.position);
                    agent.isStopped = false;

                    StartCoroutine(ChaseTarget());
                }
            }
        }



        IEnumerator ChaseTarget()
        {
            float readyToFireTime = Time.time;

            while (isChasingTarget)
            {
                agent.SetDestination(playerTarget.transform.position);

                if ((transform.position - playerTarget.transform.position).sqrMagnitude < distanceCatchTarget)
                {
                    hasCaughtTarget = true;
                    agent.isStopped = true;

                    if (Time.time >= readyToFireTime)
                    {
                        //Attack target
                        attackSystem.Play();
                        playerTarget.TakeDamage(playerDamage);

                        readyToFireTime = Time.time + weaponDelay;
                    }
                }
                else
                {
                    hasCaughtTarget = false;
                    agent.isStopped = false;
                }

                yield return null;
            }
        }

        public void TakeDamage(int loss)
        {
            CurrentHealth -= loss;
            

            if(CurrentHealth <= 0)
            {
                deathSystem.Play();
                isAlive = false;
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
