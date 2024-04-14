using UnityEngine;

namespace Summoning
{
    public class GunComponent : MonoBehaviour
    {
        [SerializeField] private Sprite m_idleSprite;
        [SerializeField] private Sprite m_fireSprite;
        [SerializeField] private float m_animationTime = 0.25f;

        private SpriteRenderer m_renderer;
        private AudioSource m_audio;

        private float m_animationStart;
        private bool m_firing;
        
        private void Awake()
        {
            m_renderer = this.GetComponent<SpriteRenderer>();
            m_audio = this.GetComponent<AudioSource>();

            m_animationStart = Time.time;
            m_firing = false;
        }

        private void Update()
        {
            if (m_firing && Time.time - m_animationStart > m_animationTime)
            {
                m_renderer.sprite = m_idleSprite;
                m_firing = false;
            }
        }

        public void Fire()
        {
            m_firing = true;
            m_animationStart = Time.time;
            m_renderer.sprite = m_fireSprite;
            m_audio.Play();
        }
    }
}