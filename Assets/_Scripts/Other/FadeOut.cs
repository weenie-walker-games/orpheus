using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class FadeOut : MonoBehaviour
    {
        [SerializeField] Animator anim;

        private void OnEnable()
        {
            GameManager.OnGameFadingToBlack += Fade;
        }

        private void OnDisable()
        {
            GameManager.OnGameFadingToBlack -= Fade;
        }

        private void Fade(bool fadeToBlack)
        {
            anim.SetTrigger("Fading");
        }
    }
}
