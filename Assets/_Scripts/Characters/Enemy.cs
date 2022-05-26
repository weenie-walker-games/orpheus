using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WeenieWalker
{
    public class Enemy : MonoBehaviour, IHealth
    {

        public bool IsAlive { get; private set; } = true;
        [SerializeField] int playerDamage = 5;
        [SerializeField] ParticleSystem attackSystem;
        [SerializeField] ParticleSystem deathSystem;
        [SerializeField] AudioSource audioSource;
        [SerializeField] NavMeshAgent agent;
        Player playerTarget;
        [SerializeField] float distanceCatchTarget = 0.75f;
        [SerializeField] float weaponDelay = 0.5f;


        [SerializeField] private int maxHealth = 100;
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get; private set; }

        bool isChasingTarget = false;
        Coroutine chaseRoutine;
        Vector3 startLocation;

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
            startLocation = transform.position;
        }

        public void TargetEntered(Player player)
        {
            playerTarget = player;

            isChasingTarget = true;

            agent.SetDestination(playerTarget.transform.position);
            agent.isStopped = false;

            if (chaseRoutine != null) StopCoroutine(chaseRoutine);
            chaseRoutine = StartCoroutine(ChaseTarget());

        }

        public void TargetExited()
        {
            isChasingTarget = false;
            agent.SetDestination(startLocation);
        }

        IEnumerator ChaseTarget()
        {
            float readyToFireTime = Time.time;

            while (isChasingTarget)
            {
                agent.SetDestination(playerTarget.transform.position);

                if ((transform.position - playerTarget.transform.position).sqrMagnitude < distanceCatchTarget)
                {
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
                IsAlive = false;
                TargetExited();
                deathSystem.Play();
                audioSource.Play();
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
