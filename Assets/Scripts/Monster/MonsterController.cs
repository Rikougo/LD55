using Summoning.Combination;
using UnityEngine;

namespace Summoning.Monster
{
    public class MonsterController : MonoBehaviour
    {
        [SerializeField] private MonsterAsset m_monsterAsset;
        [SerializeField] private Transform m_monsterPool;

        public CombinedMonster SummonMonster(CombinationPart p_part)
        {
            if (!m_monsterAsset.TryGetMonster(p_part, out var l_prefab)) return null;

            var l_instance = Instantiate(l_prefab, m_monsterPool);
            l_instance.transform.right = Vector3.left;

            return l_instance;
        }
    }
}