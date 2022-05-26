using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class DoorScript : MonoBehaviour
    {

        private enum DoorType
        {
            Enter,
            Exit
        }

        [SerializeField] DoorType typeOfDoor;
        [SerializeField] BoxCollider boxCollider;


        private void OnEnable()
        {
            GameManager.OnGameReverse += GameReverse;
            GameManager.OnEndLevel += EndLevel;
        }

        private void OnDisable()
        {
            GameManager.OnGameReverse -= GameReverse;
            GameManager.OnEndLevel -= EndLevel;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player entered" + this.name);

                switch (typeOfDoor)
                {
                    case DoorType.Enter:
                        break;
                    case DoorType.Exit:
                        GameManager.Instance.LevelCompleted();
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player exited " + this.name);

                switch (typeOfDoor)
                {
                    case DoorType.Enter:
                        //Player has left door; set trigger so player can't re-enter
                        boxCollider.isTrigger = false;
                        break;
                    case DoorType.Exit:
                        break;
                    default:
                        break;
                }
            }
        }

        private void GameReverse()
        {
            //Reverse the doors
            if (typeOfDoor == DoorType.Enter)
                typeOfDoor = DoorType.Exit;
            else
                typeOfDoor = DoorType.Enter;
        }

        private void EndLevel()
        {
            boxCollider.isTrigger = true;
        }
    }
}
