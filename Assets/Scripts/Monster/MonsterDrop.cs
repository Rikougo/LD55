using Summoning.Combination;
using UnityEngine;

namespace Summoning.Monster
{
    public class MonsterDrop : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_dropRenderer;

        private CombinationPart m_part;

        public CombinationPart Part => m_part;
        
        public void Init(Sprite p_sprite, CombinationPart p_part)
        {
            m_dropRenderer.sprite = p_sprite;
            m_part = p_part;
        }

        public void Collect()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}