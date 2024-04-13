using AYellowpaper.SerializedCollections;
using Summoning.Combination;
using UnityEngine;

namespace Summoning.Monster
{
    [CreateAssetMenu(menuName = "Summoning/Monster drops", fileName = "MonsterDrops")]
    public class MonsterDropAsset : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<CombinationPart, Sprite> m_drops;

        public bool TryGetDrop(CombinationPart p_part, out Sprite p_sprite)
        {
            return m_drops.TryGetValue(p_part, out p_sprite);
        }
    }
}