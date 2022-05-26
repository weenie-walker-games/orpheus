using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WeenieWalker
{
    public class LogoFade : MonoBehaviour
    {
        [SerializeField] Animator anim;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            Invoke("FadeLogo", 1f);
        }

        private void FadeLogo()
        {
            anim.SetTrigger("Fade");
        }

        public void SwitchScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
