using Summoning.Combination;
using UnityEngine;

namespace Summoning.Monster
{
    public class MonsterDrop : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_dropRenderer;
        [SerializeField] private BoxCollider2D m_collider2D;

        private bool m_isHovered;
        private float m_hoverTime;
        
        public CombinationPart Part { get; private set; }

        public void Init(Sprite p_sprite, CombinationPart p_part)
        {
            m_dropRenderer.sprite = p_sprite;
            Part = p_part;
            m_isHovered = false;
        }

        public void Collect()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            Vector3 l_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool l_isHover = m_collider2D.OverlapPoint(l_pos);
            if (!m_isHovered)
            {
                if (l_isHover)
                {
                    m_isHovered = true;
                    m_hoverTime = 0.25f;
                }
            }
            else
            {
                if (!l_isHover)
                {
                    m_isHovered = false;
                    this.transform.localScale = Vector3.one;
                    return;
                }
                
                if (m_hoverTime >= 0)
                {
                    m_hoverTime -= Time.deltaTime;
                    float l_progress = 1.0f - (m_hoverTime / 0.25f);

                    if (l_progress > 0)
                    {
                        this.transform.localScale = Vector3.one * (1 + (l_progress * 0.1f));
                    }
                }
            }
        }
    }
}