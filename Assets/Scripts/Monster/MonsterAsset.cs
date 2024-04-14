using AYellowpaper.SerializedCollections;
using Summoning.Combination;
using UnityEngine;

namespace Summoning.Monster
{
    [CreateAssetMenu(menuName = "Summoning/Monster Asset", fileName = "Monsters")]
    public class MonsterAsset : ScriptableObject
    {
        public static CombinationPart[] ExistingMonsters = { CombinationPart.EARTH, 
            CombinationPart.WATER, 
            CombinationPart.PLANT, 
            CombinationPart.DEMON, 
            CombinationPart.FIRE, 
            CombinationPart.ELECTRICITY };

        [SerializeField] private SerializedDictionary<CombinationPart, CombinedMonster> m_monsters;

        public bool TryGetMonster(CombinationPart p_part, out CombinedMonster p_prefab)
        {
            return m_monsters.TryGetValue(p_part, out p_prefab);
        }
    }
}