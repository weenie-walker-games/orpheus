using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class LavaFlower : MonoBehaviour
    {

        [SerializeField] Renderer lavaRenderer;
        [SerializeField] int materialNumber = 0;
        [SerializeField] float scrollSpeed = 0.1f;
        Vector2 v2Offset = Vector2.zero;


        private void LateUpdate()
        {

            float offset = Time.time * scrollSpeed;
            v2Offset.x = offset;

            if (lavaRenderer.enabled)
            {
                lavaRenderer.materials[materialNumber].mainTextureOffset = v2Offset;
            }
        }

    }
}
