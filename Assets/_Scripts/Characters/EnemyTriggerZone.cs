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
            
        }

        private void OnDisable()
        {
            
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
                Debug.Log("Player triggered");
                
                playerTarget = other.GetComponent<Player>();

                if (playerTarget != null)
                {
                    //Tell enemies to attack
                }
            }
        }
    }
}
