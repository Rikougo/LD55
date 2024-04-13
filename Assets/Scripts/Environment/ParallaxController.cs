using System;
using System.Collections.Generic;
using UnityEngine;

namespace Summoning.Environment
{
    public class ParallaxController : MonoBehaviour
    {
        [SerializeField] private Transform m_camera;

        [SerializeField] private List<ParallaxItem> m_items;

        [SerializeField] private Transform m_referencePosition;

        private void Update()
        {
            var l_delta = m_camera.position.x - m_referencePosition.position.x;

            foreach (var l_parallaxItem in m_items)
            {
                var l_position = l_parallaxItem.transform.localPosition;
                l_position.x = -l_delta * l_parallaxItem.ratio;
                l_parallaxItem.transform.localPosition = l_position;
            }
        }

        [Serializable]
        private struct ParallaxItem
        {
            public Transform transform;
            public float ratio;
        }
    }
}