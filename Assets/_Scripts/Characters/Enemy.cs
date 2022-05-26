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
        [SerializeField] AudioClip[] clips = new AudioClip[2];


        [SerializeField] private int maxHealth = 100;
        public int MaxHealth { get { return maxHealth; } }
        public int CurrentHealth { get; private set; }

        bool isChasingTarget = false;
        Coroutine chaseRoutine;
        Vector3 startLocation;
        Vector3 currentTargetDestination;
        bool isGamePaused = true;

        private void OnEnable()
        {
            GameManager.OnStartGame += Restart;
            GameManager.OnPauseGame += PauseGame;
        }

        private void OnDisable()
        {
            GameManager.OnStartGame -= Restart;
            GameManager.OnPauseGame -= PauseGame;
        }

        private void Start()
        {
            startLocation = transform.position;
            currentTargetDestination = startLocation;
        }

        public void TargetEntered(Player player)
        {
            playerTarget = player;

            isChasingTarget = true;

            currentTargetDestination = playerTarget.transform.position;


            if (chaseRoutine != null) StopCoroutine(chaseRoutine);
            chaseRoutine = StartCoroutine(ChaseTarget());

        }

        public void TargetExited()
        {
            isChasingTarget = false;
            currentTargetDestination = startLocation;
            agent.SetDestination(startLocation);
        }

        IEnumerator ChaseTarget()
        {
            float readyToFireTime = Time.time;

            while (isChasingTarget)
            {
                if (isGamePaused)
                {
                    agent.isStopped = true;
                    yield return null;
                }
                else
                {

                    agent.SetDestination(playerTarget.transform.position);
                    agent.isStopped = false;

                    if ((transform.position - playerTarget.transform.position).sqrMagnitude < distanceCatchTarget)
                    {
                        agent.isStopped = true;

                        if (Time.time >= readyToFireTime)
                        {
                            //Attack target
                            attackSystem.Play();
                            audioSource.clip = clips[0];
                            audioSource.Play();
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
        }

        public void TakeDamage(int loss)
        {

            CurrentHealth -= loss;
            
            if(CurrentHealth <= 0)
            {
                IsAlive = false;
                TargetExited();
                deathSystem.Play();
                audioSource.clip = clips[1];
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

        private void PauseGame(bool isPaused, bool toShowMenu)
        {
            //Only communicate with agent if currently chasing target or it is returning to starting location
            //if (isChasingTarget || agent.destination == startLocation)

            isGamePaused = isPaused;

            if (isPaused)
                agent.SetDestination(transform.position);
            else
                agent.SetDestination(currentTargetDestination);

            agent.isStopped = isPaused;
        }
    }
}
