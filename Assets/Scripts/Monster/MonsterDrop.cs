using Summoning.Combination;
using UnityEngine;

namespace Summoning.Monster
{
    public class MonsterDrop : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_dropRenderer;

        public CombinationPart Part { get; private set; }

        public void Init(Sprite p_sprite, CombinationPart p_part)
        {
            m_dropRenderer.sprite = p_sprite;
            Part = p_part;
        }

        public void Collect()
        {
            Destroy(gameObject);
        }
    }
}