using UnityEngine;

namespace Summoning.Combination
{
    public class CombinationController : MonoBehaviour
    {
        [SerializeField] private CombinationAsset m_combinationAsset;
        [SerializeField] private CombinedMonster m_combinedMonsterPrefab;

        [SerializeField] private Transform m_summonPool;

        public CombinedMonster SummonCombination(CombinationPart p_arm, CombinationPart p_body)
        {
            if (!m_combinationAsset.TryGetPart(p_arm, out var l_armData) ||
                !m_combinationAsset.TryGetPart(p_body, out var l_bodyData))
                return null;

            // TODO Check if can summon

            var l_instance = Instantiate(m_combinedMonsterPrefab, m_summonPool);
            l_instance.Init(l_armData, l_bodyData);
            l_instance.transform.right = Vector3.right;

            return l_instance;
        }
    }
}