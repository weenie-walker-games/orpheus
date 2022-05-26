using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeenieWalker
{
    public class EnemyTriggerZone : MonoBehaviour
    {

        [SerializeField] List<Enemy> enemiesInZone = new List<Enemy>();
        Player playerTarget;

        List<Renderer> renderers = new List<Renderer>();

        private void OnEnable()
        {
            GameManager.OnEndLevel += PlayerExited;
        }

        private void OnDisable()
        {
            GameManager.OnEndLevel -= PlayerExited;
        }

        private void Start()
        {
            renderers = GetComponents<Renderer>().ToList();
            if(renderers.Count != 0)
            {
                renderers.ForEach(r => r.enabled = false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {

                playerTarget = other.GetComponentInParent<Player>();

                if (playerTarget != null)
                {
                    //Tell enemies to attack
                    enemiesInZone.ForEach(e => { if (e.IsAlive) e.TargetEntered(playerTarget); });
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerExited();
            }
        }

        private void PlayerExited()
        {
            enemiesInZone.ForEach(e => { if (e.IsAlive) e.TargetExited(); });
        }
    }
}
